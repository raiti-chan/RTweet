using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;

namespace RTweet.Windows.Controls {
	/// <summary>
	/// ウィンドウに直接貼り付けて使用するウィンドウコントロールのベースクラス。
	/// </summary>
	public abstract class WindowPanelBase : System.Windows.Controls.Control {
		#region 依存プロパティ

		/// <summary>
		/// <see cref="AllowsTransparency"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="AllowsTransparency"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty AllowsTransparencyProperty = DependencyProperty.Register("AllowsTransparency", typeof(bool),
			typeof(WindowPanelBase), new PropertyMetadata(false));

		/// <summary>
		/// <see cref="Icon"/>依存関係プロパティを識別します。
		/// </summary>';p[9876763
		/// 
		/// 
		/// <returns><see cref="Icon"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(WindowPanelBase));

		/// <summary>
		/// <see cref="Left"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="Left"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(WindowPanelBase));

		/// <summary>
		/// <see cref="ResizeMode"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="ResizeMode"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(WindowPanelBase),
			new PropertyMetadata(ResizeMode.CanResize));

		/// <summary>
		/// <see cref="ShowActivated"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="ShowActivated"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty ShowActivatedProperty = DependencyProperty.Register("ShowActivated", typeof(bool), typeof(WindowPanelBase),
			new PropertyMetadata(true));

		/// <summary>
		/// <see cref="ShowInTaskbar"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="ShowInTaskbar"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty ShowInTaskbarProperty = DependencyProperty.Register("ShowInTaskbar", typeof(bool), typeof(WindowPanelBase),
			new PropertyMetadata(true));

		/// <summary>
		/// <see cref="SizeToContent"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="SizeToContent"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.Register("SizeToContent", typeof(SizeToContent),
			typeof(WindowPanelBase), new PropertyMetadata(SizeToContent.Manual));

		/// <summary>
		/// <see cref="TaskbarItemInfo"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="TaskbarItemInfo"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.Register("TaskbarItemInfo", typeof(TaskbarItemInfo),
			typeof(WindowPanelBase));

		/// <summary>
		/// <see cref="Title"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="Title"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WindowPanelBase),
			new PropertyMetadata("Title"));

		/// <summary>
		/// <see cref="Topmost"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="Topmost"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty TopmostProperty = DependencyProperty.Register("Topmost", typeof(bool), typeof(WindowPanelBase),
			new PropertyMetadata(false));

		/// <summary>
		/// <see cref="Top"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="Top"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(WindowPanelBase));

		/// <summary>
		/// <see cref="WindowState"/>依存関係プロパティを識別します。
		/// </summary>
		/// <returns><see cref="WindowState"/>依存関係プロパティの識別子。</returns>
		public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState),
			typeof(WindowPanelBase), new PropertyMetadata(WindowState.Normal));

		/// <summary>
		/// <see cref="WindowStyle"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register("WindowStyle", typeof(WindowStyle),
			typeof(WindowPanelBase), new PropertyMetadata(WindowStyle.SingleBorderWindow));
		
		

		/// <summary>
		/// ウィンドウのクライアント領域が透過性をサポートするかどうかを示す値を取得または設定します。
		/// </summary>
		/// <returns>ウィンドウで透過性がサポートされる場合は true。それ以外の場合は false。</returns>
		/// <exception cref="System.InvalidOperationException"><see cref="AllowsTransparency"/>は、ウィンドウが表示された後に変更されます。</exception>
		public bool AllowsTransparency {
			get { return (bool) GetValue(AllowsTransparencyProperty); }
			set { SetValue(AllowsTransparencyProperty, value); }
		}

		/// <summary>
		/// ウィンドウのアイコンを取得または設定します。
		/// </summary>
		/// <returns>アイコンを表す<see cref="ImageSource"/> オブジェクト。</returns>
		public ImageSource Icon {
			get { return (ImageSource) GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		/// <summary>
		/// ウィンドウの左端の位置を、デスクトップとの関係で取得または設定します。
		/// </summary>
		/// <returns>ウィンドウの左端の位置。単位は論理単位 (1/96 インチ)。</returns>
		public double Left {
			get { return (double) GetValue(LeftProperty); }
			set { SetValue(LeftProperty, value); }
		}

		/// <summary>
		/// サイズ変更モードを取得または設定します。
		/// </summary>
		/// <returns>サイズ変更モードを指定する System.Windows.ResizeMode 値。</returns>
		public ResizeMode ResizeMode {
			get { return (ResizeMode) GetValue(ResizeModeProperty); }
			set { SetValue(ResizeModeProperty, value); }
		}

		/// <summary>
		/// ウィンドウをアクティブ状態で初期表示するかどうかを示す値を取得または設定します。
		/// </summary>
		/// <returns>ウィンドウを初期表示するときにアクティブ化する場合は true。それ以外の場合は false。 既定値は、true です。</returns>
		public bool ShowActivated {
			get { return (bool) GetValue(ShowActivatedProperty); }
			set { SetValue(ShowActivatedProperty, value); }
		}

		/// <summary>
		/// ウィンドウにタスク バー ボタンがあるかどうかを示す値を取得または設定します。
		/// </summary>
		/// <returns>ウィンドウにタスク バー ボタンがある場合は true。それ以外の場合は false。 ブラウザー内でウィンドウがホストされている場合は適用されません。</returns>
		public bool ShowInTaskbar {
			get { return (bool) GetValue(ShowInTaskbarProperty); }
			set { SetValue(ShowInTaskbarProperty, value); }
		}

		/// <summary>
		/// ウィンドウのサイズがコンテンツのサイズに合わせて自動的に調整されるかどうかを示す値を取得または設定します。
		/// </summary>
		/// <returns><see cref="System.Windows.SizeToContent"/>値。 既定値は、<see cref="System.Windows.SizeToContent.Manual"/> です。</returns>
		public SizeToContent SizeToContent {
			get { return (SizeToContent) GetValue(SizeToContentProperty); }
			set { SetValue(SizeToContentProperty, value); }
		}

		/// <summary>
		/// <see cref="Window"/>の Windows 7 タスク バーのサムネイルを取得または設定します。
		/// </summary>
		/// <returns><see cref="Window"/>の Windows 7 タスク バーのサムネイル。</returns>
		public TaskbarItemInfo TaskbarItemInfo {
			get { return (TaskbarItemInfo) GetValue(TaskbarItemInfoProperty); }
			set { SetValue(TaskbarItemInfoProperty, value); }
		}

		/// <summary>
		/// ウィンドウのタイトルを取得または設定します。
		/// </summary>
		/// <returns>ウィンドウのタイトルを格納する<see cref="string"/>。</returns>
		public string Title {
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// ウィンドウの上端の位置を、デスクトップとの関係で取得または設定します。
		/// </summary>
		/// <returns>ウィンドウの上端の位置。単位は論理単位 (1/96 インチ)。</returns>
		public double Top {
			get { return (double) GetValue(TopProperty); }
			set { SetValue(TopProperty, value); }
		}

		/// <summary>
		/// ウィンドウが最上位 z オーダーで表示されるかどうかを示す値を取得または設定します。
		/// </summary>
		/// <returns>ウィンドウが最上位の場合は true。それ以外の場合は false。</returns>
		public bool Topmost {
			get { return (bool) GetValue(TopmostProperty); }
			set { SetValue(TopmostProperty, value); }
		}

		/// <summary>
		/// ウィンドウが元のサイズに戻されているか、最小化されているか、最大化されているかを示す値を取得または設定します。
		/// </summary>
		/// <returns>ウィンドウが元のサイズに戻されているか、最小化されているか、最大化されているかを判断する<see cref="System.Windows.WindowState"/>。
		///  既定値は、<see cref=" System.Windows.WindowState.Normal"/>(元のサイズに戻されている) です。</returns>
		public WindowState WindowState {
			get { return (WindowState) GetValue(WindowStateProperty); }
			set { SetValue(WindowStateProperty, value); }
		}

		/// <summary>
		/// ウィンドウの枠線のスタイルを取得または設定します。
		/// </summary>
		/// <returns>ウィンドウの境界線スタイルを指定する <see cref="System.Windows.WindowStyle"/>。 既定値は、<see cref="System.Windows.WindowStyle.SingleBorderWindow"/>です。</returns>
		public WindowStyle WindowStyle {
			get { return (WindowStyle) GetValue(WindowStyleProperty); }
			set { SetValue(WindowStyleProperty, value); }
		}

		#endregion

		/// <summary>
		/// このコントロールが存在するウィンドウ
		/// </summary>
		protected Window ParentWindow { get; private set; }

		/// <summary>
		/// コントロールがウィンドウに追加されたときに発生します。
		/// </summary>
		/// <param name="window">このコントロールが追加された親ウィンドウ</param>
		public virtual void AddedToWindow(Window window) {
			ParentWindow = window;
			
		}

		/// <summary>
		/// ウィンドウを表示する前の初期化処理
		/// </summary>
		public virtual void ShowInit() {}
	}
}
