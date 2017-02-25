using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RTweet.Windows.Controls {
	/// <summary>
	/// 画像のプレビュー用コントロール
	/// 
	/// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
	///
	/// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:RTweet.Windows.Controls"
	///
	///
	/// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:RTweet.Windows.Controls;assembly=RTweet.Windows.Controls"
	///
	/// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
	/// リビルドして、コンパイル エラーを防ぐ必要があります:
	///
	///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
	///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
	///
	///
	/// 手順 2)
	/// コントロールを XAML ファイルで使用します。
	///
	///     <MyNamespace:PicturePreviewPanelControl/>
	///
	/// </summary>
	public class PicturePreviewPanelControl : System.Windows.Controls.Control {
		#region コンストラクタ

		/// <summary>
		/// staticコンストラクタ
		/// </summary>
		static PicturePreviewPanelControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PicturePreviewPanelControl), new FrameworkPropertyMetadata(typeof(PicturePreviewPanelControl)));
		}

		/// <summary>
		/// 初期化
		/// <param name="index">この要素のインデックス。デフォルト0</param>
		/// <exception cref="ArgumentOutOfRangeException">indexが0以下の場合スローされます</exception>
		/// </summary>
		public PicturePreviewPanelControl(int index) {
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
			Index = index;
			Height = 0;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public PicturePreviewPanelControl() {}

		#endregion

		#region プロパティ

		/// <summary>
		/// この要素のインデックス
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 表示する画像ソース
		/// </summary>
		public ImageSource ImageSource {
			set {
				if (_image == null) return;
				if (value == null) {
					Height = 0;
					_image.Height = 0;
					_image.Source = null;
					return;
				}
				var x = value.Width;
				var y = value.Height;
				var y2 = 100 / x * y;
				if (y2 < 30) y2 = 30;
				Height = y2;
				_image.Height = y2;
				_image.Source = value;
			}

			get { return _image.Source; }
		}

		#endregion

		#region フィールド

		private Button _removeButton;
		private Image _image;

		private RoutedEventHandler _eventHandler;

		#endregion

		/// <summary>
		/// テンプレートの行進が発生したとき実行される
		/// </summary>
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			if (_removeButton != null) {
				_removeButton.MouseEnter -= RemoveButton_MouseEnter;
				_removeButton.Click -= _eventHandler;
			}
			ImageSource source = null;
			double imageHight = 0;
			if (_image != null) {
				_image.MouseLeave -= Image_MouseLeave;
				_image.MouseEnter -= Image_MouseEnter;
				source = _image.Source;
				imageHight = _image.Height;
			}

			_removeButton = GetTemplateChild("RemoveButton") as Button;
			_image = GetTemplateChild("Image") as Image;

			if (_removeButton != null) {
				_removeButton.MouseEnter += RemoveButton_MouseEnter;
				_removeButton.Click += _eventHandler;
			}
			if (_image == null) return;
			_image.MouseLeave += Image_MouseLeave;
			_image.MouseEnter += Image_MouseEnter;
			_image.Source = source;
			_image.Height = imageHight;
		}

		/// <summary>
		/// 消去ボタンのイベントを追加します。
		/// </summary>
		/// <param name="eventHandler">イベント動作</param>
		public void AddRemoveButtonClickEvent(RoutedEventHandler eventHandler) {
			_eventHandler = eventHandler;
			if (_removeButton != null) _removeButton.Click += eventHandler;
		}

		/// <summary>
		/// 消去ボタンのイベントを消去します。
		/// </summary>
		/// <param name="eventHandler">イベント動作</param>
		public void RemoveRemoveButtonClickEvent() {
			if (_removeButton != null) _removeButton.Click -= _eventHandler;
			_eventHandler = null;
		}

		#region イベント

		/// <summary>
		/// 画像の上にマウスカーソルが来た時、消去ボタンを表示する。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Image_MouseEnter(object sender, MouseEventArgs e) {
			_removeButton.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// 画像上からマウスが外れたら、消去ボタンを隠す。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Image_MouseLeave(object sender, MouseEventArgs e) {
			_removeButton.Visibility = Visibility.Hidden;
		}

		/// <summary>
		/// ボタン上にカーソルが行っても消えないようにする。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void RemoveButton_MouseEnter(object sender, MouseEventArgs e) {
			_removeButton.Visibility = Visibility.Visible;
		}

		#endregion
	}
}
