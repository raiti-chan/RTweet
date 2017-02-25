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
		/// <see cref="ParentWindow"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty ParentWindowStyleProperty = DependencyProperty.Register("ParentWindowStyle", typeof(Style), typeof(WindowPanelBase), new PropertyMetadata(default(Style)));
		
		/// <summary>
		/// 親ウィンドウのスタイルを定義します。
		/// </summary>
		/// <returns>親ウィンドウのスタイル</returns>
		public Style ParentWindowStyle {
			get { return (Style) GetValue(ParentWindowStyleProperty); }
			set { SetValue(ParentWindowStyleProperty, value); }
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
