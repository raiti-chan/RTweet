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

			SetBinding(AllowsTransparencyProperty, new Binding {Source = panel, Path = new PropertyPath("AllowsTransparency")});
			SetBinding(IconProperty, new Binding {Source = panel, Path = new PropertyPath("Icon")});
			SetBinding(LeftProperty, new Binding {Source = panel, Path = new PropertyPath("Left")});
			SetBinding(ResizeModeProperty, new Binding {Source = panel, Path = new PropertyPath("ResizeMode")});
			SetBinding(ShowActivatedProperty, new Binding {Source = panel, Path = new PropertyPath("ShowActivated")});
			SetBinding(ShowInTaskbarProperty, new Binding {Source = panel, Path = new PropertyPath("ShowInTaskbar")});
			SetBinding(SizeToContentProperty, new Binding {Source = panel, Path = new PropertyPath("SizeToContent")});
			SetBinding(TaskbarItemInfoProperty, new Binding {Source = panel, Path = new PropertyPath("TaskbarItemInfo")});
			SetBinding(TitleProperty, new Binding {Source = panel, Path = new PropertyPath("Title")});
			SetBinding(TopProperty, new Binding {Source = panel, Path = new PropertyPath("Top")});
			SetBinding(TopmostProperty, new Binding {Source = panel, Path = new PropertyPath("Topmost")});
			SetBinding(WindowStateProperty, new Binding {Source = panel, Path = new PropertyPath("WindowState")});
			SetBinding(WindowStyleProperty, new Binding {Source = panel, Path = new PropertyPath("WindowStyle")});
			/*
			SetBinding(MaxHeightProperty, new Binding {Source = panel, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay});
			SetBinding(MaxWidthProperty, new Binding {Source = panel, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay});
			SetBinding(MinHeightProperty, new Binding {Source = panel, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay});
			SetBinding(MinWidthProperty, new Binding {Source = panel, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay});
			*/
			SetBinding(HeightProperty, new Binding {Source = panel, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay});
			SetBinding(WidthProperty, new Binding {Source = panel, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay});
			
			//Background = null;
			//Foreground = null;

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
