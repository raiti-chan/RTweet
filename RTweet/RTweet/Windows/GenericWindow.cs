using System.Windows;
using System.Windows.Data;
using RTweet.Windows.Controls;

namespace RTweet.Windows {
	public sealed class GenericWindow : Window {
		#region プロパティ

		/// <summary>
		/// このウィンドウが保持している
		/// </summary>
		public WindowPanelBase WindowContent { get; }

		#endregion

		#region コンストラクタ

		/// <summary>
		/// ウィンドウを生成します。
		/// </summary>
		/// <param name="panel">貼り付けるパネル</param>
		public GenericWindow(WindowPanelBase panel) {
			panel.AddedToWindow(this);
			AddChild(panel);
			WindowContent = panel;

			SetBinding(StyleProperty, new Binding {Source = panel, Path = new PropertyPath("ParentWindowStyle")});

		}

		#endregion

		#region publicメソッド

		/// <summary>
		/// 表示する前の初期化処理
		/// </summary>
		public void ShowInit() {
			WindowContent.ShowInit();
		}

		#endregion
	}
}
