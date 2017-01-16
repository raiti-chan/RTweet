using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace RTweet.Main {
	internal class HotKeyWinApi {

		public const int WmHotKey = 0x312;

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModKey fsModifiers, Keys vk);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

	}


	/// <summary>
	/// グローバルホットキー登録クラス
	/// </summary>
	public sealed class HotKeyRegister {

		/// <summary>
		/// ホットキーが押されたときに発生するイベント
		/// </summary>
		public event Action<HotKeyRegister> HotKeyPressed;

		private readonly int _id;
		private bool _isKeyRegistered;
		private readonly IntPtr _handle;

		/// <summary>
		/// ホットキーに使用されるキー
		/// </summary>
		public Keys Key { get; private set; }


		/// <summary>
		/// ホットキーに使用される修飾キー
		/// </summary>
		public ModKey KeyModifier { get; private set; }

		/// <summary>
		/// 新規のホットキーを登録します。
		/// </summary>
		/// <param name="modKey">修飾キー</param>
		/// <param name="key">キー</param>
		/// <param name="window">親ウィンドウ</param>
		public HotKeyRegister(ModKey modKey, Keys key, Window window) {
			var windowHandle = new WindowInteropHelper(window).Handle;
			// ReSharper disable once InvocationIsSkipped
			Contract.Requires(modKey != ModKey.None || key != Keys.None);
			// ReSharper disable once InvocationIsSkipped
			Contract.Requires(windowHandle != IntPtr.Zero);

			Key = key;
			KeyModifier = modKey;
			var r = new Random();
			_id = r.Next();
			_handle = windowHandle;
			RegisterHotKey();

			ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
		}

		private void RegisterHotKey() {
			if (Key == Keys.None)
				return;
			if (_isKeyRegistered)
				UnregisterHotKey();
			_isKeyRegistered = HotKeyWinApi.RegisterHotKey(_handle, _id, KeyModifier, Key);
			if (!_isKeyRegistered)
				throw new ApplicationException("Hotkey already in use");
		}

		~HotKeyRegister() {
			UnregisterHotKey();
		}

		public void UnregisterHotKey() {
			_isKeyRegistered = !HotKeyWinApi.UnregisterHotKey(_handle, _id);
		}


		private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled) {
			if (handled) return;
			if (msg.message != HotKeyWinApi.WmHotKey || (int) (msg.wParam) != _id) return;
			OnHotKeyPressed();
			handled = true;
		}

		private void OnHotKeyPressed() {
			HotKeyPressed?.Invoke(this);
		}


	}



	public enum ModKey {
		None = 0x0000,

		Alt = 0x0001,
		Ctl = 0x0002,

		AltCtl = 0x0003,

		Shi = 0x0004,

		AltShi = 0x0005,
		CtlShi = 0x0006,
		AltCtlShi = 0x0007,

		Win = 0x0008,

		AltWin = 0x0009,
		CtlWin = 0x000A,
		AltCtlWin = 0x000B,
		ShiWin = 0x000C,
		AltShiWin = 0x000D,
		CtlShiWin = 0x000E,
		AltCtlShiWin = 0x000F,
	}
}
