using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace RTweet.Main {
	/// <summary>
	/// Shell32.dll
	/// 内のアイコンを取得します。
	/// </summary>
	internal class GetShell32Icon {
		/// <summary>
		/// importするDLL名
		/// (Win32AIPのShell32.dll)
		/// </summary>
		private const string DllName = "shell32.dll";

		/// <summary>
		/// アイコンを参照するファイルパス
		/// </summary>
		private const string DllPath = @"C:\Windows\System32\shell32.dll";

		/// <summary>
		/// Shell32.dll内の
		/// ExtractIconEx 関数
		/// DLL内などのアイコンを取得します。
		/// </summary>
		/// <param name="lpszFile">参照ファイルパス</param>
		/// <param name="nIconIndex">アイコンのインデックス</param>
		/// <param name="phiconLarge">大きいアイコンのハンドルのポインタ格納配列</param>
		/// <param name="phiconSmall">小さいアイコンのハンドルのポインタ格納配列</param>
		/// <param name="nIcons">取得するアイコンの数</param>
		/// <returns>取得できたアイコンの数</returns>
		[DllImport(DllName, CharSet = CharSet.Unicode)]
		private static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall,
			uint nIcons);

		/// <summary>
		/// 指定されたハンドルを解放します
		/// </summary>
		/// <param name="hIcon">ハンドルポインタ</param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DestroyIcon(IntPtr hIcon);

		/// <summary>
		/// 指定したインデックスのアイコンを取得します。
		/// </summary>
		/// <param name="iconIndex">アイコンインデックス</param>
		/// <param name="isSmallIcon">小さいアイコンの場合true デフォルト:false</param>
		/// <returns>取得したアイコンの<see cref="BitmapSource"/>(取得できなかった場合null)</returns>
		public static BitmapSource GetIcon(int iconIndex, bool isSmallIcon = false) {
			IntPtr[] hLargeIcon = {IntPtr.Zero};
			IntPtr[] hSmallIcon = {IntPtr.Zero};

			try {
				if (ExtractIconEx(DllPath, iconIndex, hLargeIcon, hSmallIcon, 1) < 1) return null;

				BitmapSource returnIcon;

				if (isSmallIcon) {
					if (hSmallIcon[0] == IntPtr.Zero) return null;
					returnIcon = Imaging.CreateBitmapSourceFromHIcon(hSmallIcon[0], Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				} else {
					if (hLargeIcon[0] == IntPtr.Zero) return null;
					returnIcon = Imaging.CreateBitmapSourceFromHIcon(hLargeIcon[0], Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				}

				return returnIcon;
			} finally {
				foreach (var intPtr in hLargeIcon) if (intPtr != IntPtr.Zero) DestroyIcon(intPtr);

				foreach (var intPtr in hSmallIcon) if (intPtr != IntPtr.Zero) DestroyIcon(intPtr);
			}
		}
	}
}