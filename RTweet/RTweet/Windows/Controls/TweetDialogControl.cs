using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoreTweet;
using Microsoft.Win32;
using RTweet.Main.Config;
using RTweet.Main.Twitter;
using ToriatamaText;

namespace RTweet.Windows.Controls {
	/// <summary>
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
	///     <MyNamespace:TweetDialogControl/>
	///
	/// </summary>
	public class TweetDialogControl : WindowPanelBase {
		#region コンストラクタ

		/// <summary>
		/// staticコンストラクタ
		/// </summary>
		static TweetDialogControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TweetDialogControl), new FrameworkPropertyMetadata(typeof(TweetDialogControl)));
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TweetDialogControl() {
			KeyDown += ThisOnKeyDown;
		}

		#endregion

		#region privateフィールド

		/// <summary>
		/// ツイートの最大文字数
		/// </summary>
		private const int MaxText = 140;

		/// <summary>
		/// 画像最大数
		/// </summary>
		private const byte MaxPicture = 4;

		/// <summary>
		/// UPする画像
		/// </summary>
		private readonly List<FileInfo> _upPictures = new List<FileInfo>(4);

		/// <summary>
		/// 文字数計算用のExtractor
		/// </summary>
		private readonly Extractor _extractor = new Extractor();

		/// <summary>
		/// ホットキーの処理中の場合true
		/// </summary>
		private bool _isHotKey;
		
		/// <summary>
		/// 画像選択ダイアログ
		/// </summary>
		private readonly OpenFileDialog _fileDialog = new OpenFileDialog {
			Filter = "画像ファイル(*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif",
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
		};

		#endregion

		#region コントロール

		/// <summary>
		/// 画像追加ボタン
		/// </summary>
		private Button _mediaButton;

		/// <summary>
		/// 下書きボタン
		/// </summary>
		private Button _draftButton;

		/// <summary>
		/// ツイート入力テキストボックス
		/// </summary>
		private TextBox _tweetText;

		/// <summary>
		/// 画像プレビューパネル
		/// </summary>
		private PicturePreviewStackPanelControl _previewStackPanel;

		#endregion

		#region プロパティ

		public bool CanClose { set; private get; } = true;

		#region 依存プロパティ

		#region 文字数関係プロパティ

		/// <summary>
		/// <see cref="TextLength"/>依存関係プロパティを識別します。
		/// </summary>
		private static readonly DependencyPropertyKey TextLengthPropertyKey = DependencyProperty.RegisterAttachedReadOnly("TextLength", typeof(int),
			typeof(TweetDialogControl), new PropertyMetadata(MaxText));

		/// <summary>
		/// <see cref="TextLength"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty TextLengthProperty = TextLengthPropertyKey.DependencyProperty;

		/// <summary>
		/// 残りの入力可能な文字数を取得します。
		/// </summary>
		/// <returns>残り入力可能な文字数。超えていると負の値。</returns>
		public int TextLength {
			get { return (int) GetValue(TextLengthProperty); }
			private set { SetValue(TextLengthPropertyKey, value); }
		}


		/// <summary>
		/// <see cref="IsTextLengthOver"/>依存関係プロパティを識別します。
		/// </summary>
		private static readonly DependencyPropertyKey IsTextLengthOverPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsTextLengthOver",
			typeof(bool), typeof(TweetDialogControl), new PropertyMetadata(false));

		/// <summary>
		/// <see cref="IsTextLengthOver"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty IsTextLengthOverProperty = IsTextLengthOverPropertyKey.DependencyProperty;

		/// <summary>
		/// 最大文字数が超えているかを取得します。
		/// </summary>
		/// <returns>最大文字数を超えていた場合true。</returns>
		public bool IsTextLengthOver {
			get { return (bool) GetValue(IsTextLengthOverProperty); }
			private set { SetValue(IsTextLengthOverPropertyKey, value); }
		}

		#endregion

		#region 画像関係プロパティ

		/// <summary>
		/// <see cref="PictureCount"/>依存関係プロパティを識別します。
		/// </summary>
		private static readonly DependencyPropertyKey PictureCountPropertyKey = DependencyProperty.RegisterAttachedReadOnly("PictureCount", typeof(byte),
			typeof(TweetDialogControl), new PropertyMetadata((byte) 0));

		/// <summary>
		/// <see cref="PictureCount"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty PictureCountProperty = PictureCountPropertyKey.DependencyProperty;

		/// <summary>
		/// レジスタに登録されている画像の数。
		/// </summary>
		/// <returns>レジスタに登録されている画像の数。</returns>
		public byte PictureCount {
			get { return (byte) GetValue(PictureCountProperty); }
			private set { SetValue(PictureCountPropertyKey, value); }
		}


		/// <summary>
		/// <see cref="IsNonPicture"/>関係プロパティを識別します。
		/// </summary>
		private static readonly DependencyPropertyKey IsNonPicturePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsNonPicture", typeof(bool),
			typeof(TweetDialogControl), new PropertyMetadata(true));

		/// <summary>
		/// <see cref="IsNonPicture"/>関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty IsNonPictureProperty = IsNonPicturePropertyKey.DependencyProperty;

		/// <summary>
		/// 画像が登録されてないか。
		/// </summary>
		/// <returns>登録されてない場合trueを返します。</returns>
		public bool IsNonPicture {
			get { return (bool) GetValue(IsNonPictureProperty); }
			private set { SetValue(IsNonPicturePropertyKey, value); }
		}


		/// <summary>
		/// <see cref="IsMaxImage"/>依存関係プロパティを識別します。
		/// </summary>
		private static readonly DependencyPropertyKey IsMaxImagePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsMaxImage", typeof(bool),
			typeof(TweetDialogControl), new PropertyMetadata(false));

		/// <summary>
		/// <see cref="IsMaxImage"/>依存関係プロパティを識別します。
		/// </summary>
		public static readonly DependencyProperty IsMaxImageProperty = IsMaxImagePropertyKey.DependencyProperty;

		/// <summary>
		/// 画像が最大枚数登録されているか。
		/// </summary>
		/// <returns>登録画像数が最大の場合true。</returns>
		public bool IsMaxImage {
			get { return (bool) GetValue(IsMaxImageProperty); }
			private set { SetValue(IsMaxImagePropertyKey, value); }
		}

		#endregion

		#endregion

		#endregion

		#region publicメソッド

		#endregion

		#region privateメソッド

		private void Close() {
			if (!CanClose) return; //CanCloseがfalseの場合無視
			ParentWindow.Hide();
		}

		/// <summary>
		/// 送信レジスターに画像を登録します。
		/// </summary>
		private void AddPicture() {
			IsNonPicture = false;
			Width = 500;
			/*
			CanClose = false; //ウィンドウを閉じられないようにする
			var showDialog = _fileDialog.ShowDialog(); //画像選択ダイアログを開く
			
			if (showDialog != null && (bool) showDialog) {
				if (_upPictures.Count < MaxPicture) {
					_upPictures.Add(new FileInfo(_fileDialog.FileName));
					_previewStackPanel?.AddImage(new BitmapImage(new Uri(_fileDialog.FileName)));
				}
			}

			var pictureCount = (byte)_upPictures.Count; //現在の画像数を取得する。
			
			PictureCount = pictureCount; //現在の画像数を依存プロパティに反映
			IsNonPicture = pictureCount == 0; //画像数が0(Non)かを依存プロパティに反映。
			IsMaxImage = pictureCount == MaxPicture; //画像数が最大かを依存プロパティに反映
			
			if (_mediaButton != null) _mediaButton.IsEnabled = pictureCount < MaxPicture; //画像ボタンを条件によりアクティブの切り替え。
			
			ParentWindow.Activate(); //Windowをアクティブ化する。
			_tweetText?.Focus(); //テキストボックスにフォーカスを渡す。
			CanClose = true; //Windowを閉じられるようにする。
			*/
		}

		/// <summary>
		/// 送信レジスターに登録された画像を消去します。
		/// </summary>
		/// <param name="index">消去する画像のインデックス</param>
		private void RemovePicture(int index) {
			_upPictures.RemoveAt(index);
			_previewStackPanel?.RemoveImage(index);
			if (_mediaButton != null) _mediaButton.IsEnabled = _upPictures.Count < 4;
		}

		/// <summary>
		/// ツイートを送信します。
		/// </summary>
		private void TransmitTweet() {
			if (TextLength < 0) {
				//文字数がオーバーしていた場合
				CanClose = false;
				MessageBox.Show("文字数が制限をオーバーしています。", "警告", MessageBoxButton.OK, MessageBoxImage.Error); //警告ダイアログの表示
				ParentWindow.Activate();
				_tweetText?.Focus();
				CanClose = true;
				return;
			}

			if (_upPictures.Count > 0) {
				//画像がレジスターに追加されていた場合
				var upResults = new MediaUploadResult[_upPictures.Count];
				for (var i = 0; i < upResults.Length; i++) upResults[i] = TwitterSystem.Instance.UploadPicture(_upPictures[i]); //画像をアップロード
				var ids = new long[upResults.Length]; //ID格納配列
				for (var i = 0; i < ids.Length; i++) ids[i] = upResults[i].MediaId; //IDを代入
				TwitterSystem.Instance.Tweet(_tweetText.Text, ids); //メディアを添付してツイートを送信
			} else {
				TwitterSystem.Instance.Tweet(_tweetText.Text); //テキストをツイート
			}
			Close(); //ウィンドウを閉じる
		}

		/*
		/// <summary>
		/// コントロールをオーバーレイ表示させます。
		/// </summary>
		private void ShowOverlayControle() {
			
		}
		*/

		#endregion

		#region Override

		/// <summary>
		/// テンプレートが更新されたときに実行されます。
		/// </summary>
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			//TweetText
			if (_tweetText != null) {
				_tweetText.PreviewTextInput -= TweetTextOnPreviewTextInput;
				_tweetText.TextChanged -= TweetTextOnTextChanged;
			}
			//PreviewStackPanel
			BitmapImage[] images = null;
			if (_previewStackPanel != null) {
				images = _previewStackPanel?.GetBitmapImages();
				_previewStackPanel?.Clear();
				_previewStackPanel.RemoveClick -= PreviewStackPanelOnRemoveClick;
			}
			//MediaButton
			if (_mediaButton != null) _mediaButton.Click -= MediaButtonOnClick;

			//コントロールをテンプレートから取得
			_tweetText = GetTemplateChild("TweetText") as TextBox;
			_previewStackPanel = GetTemplateChild("PreviewStackPanel") as PicturePreviewStackPanelControl;
			_mediaButton = GetTemplateChild("MediaButton") as Button;
			_draftButton = GetTemplateChild("DraftButton") as Button;

			//TweetText
			if (_tweetText != null) {
				_tweetText.PreviewTextInput += TweetTextOnPreviewTextInput;
				_tweetText.TextChanged += TweetTextOnTextChanged;
			}
			//PreviewStackPanel
			if (_previewStackPanel != null) {
				if (images != null) foreach (var image in images) _previewStackPanel.AddImage(image);
				_previewStackPanel.RemoveClick += PreviewStackPanelOnRemoveClick;
			}
			//MediaButton
			if (_mediaButton != null) _mediaButton.Click += MediaButtonOnClick;
		}

		/// <summary>
		/// コントロールがウィンドウに追加されたときに発生します。
		/// </summary>
		/// <param name="window">このコントロールが追加された親ウィンドウ</param>
		public override void AddedToWindow(Window window) {
			base.AddedToWindow(window);
			MouseLeftButtonDown += (sender, args) => { window.DragMove(); }; //ドラッグ処理を可能にする。
			window.Deactivated += (sender, args) => { Close(); };
		}


		/// <summary>
		/// 表示する前の初期化
		/// </summary>
		public override void ShowInit() {
			base.ShowInit();
			if (MainConfig.Instance.IsRelativeCoordinatesFromTheMouse) {
				var devicePoint = System.Windows.Forms.Cursor.Position;

				ParentWindow.Top = devicePoint.Y + MainConfig.Instance.TweetDialogPopuPoint.Y;
				ParentWindow.Left = devicePoint.X + MainConfig.Instance.TweetDialogPopuPoint.X;
				//PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice(null);
			} else {
				Top = MainConfig.Instance.TweetDialogPopuPoint.X;
				Left = MainConfig.Instance.TweetDialogPopuPoint.Y;
			}
		}

		#endregion

		#region イベント

		/// <summary>
		/// このコントロールにフォーカスがあるときにキーが押されると発生します。
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="eventArgs">eventArgs</param>
		private void ThisOnKeyDown(object sender, KeyEventArgs eventArgs) {
			_isHotKey = false;
			switch (eventArgs.Key) {
				case Key.Escape: //ウィンドウを閉じる
					if (!CanClose) break;
					Close();
					break;
				case Key.Enter: //ツイートを送信
					if (Keyboard.Modifiers != ModifierKeys.Control) return;
					TransmitTweet();
					break;
				case Key.P: //画像を添付
					if (Keyboard.Modifiers != ModifierKeys.Control || _upPictures.Count >= 4) return;
					AddPicture();
					break;
				default:
					return;
			}
			_isHotKey = true;
		}

		/// <summary>
		/// ツイートテキストボックスがデバイスに依存しない方法でテキストを取得したときに発生します。
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="eventArgs">eventArgs</param>
		private void TweetTextOnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs) {
			eventArgs.Handled = _isHotKey;
			_isHotKey = false;
		}

		/// <summary>
		/// ツイートテキストボックスのテキスト要素が変更されたときに発生します。
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="textChangedEventArgs">event</param>
		private void TweetTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) {
			var result = _extractor.ExtractUrls(_tweetText.Text);
			var textLength = _tweetText.Text.Count(text => !char.IsLowSurrogate(text)) - result.Sum(x => x.Length) + 23 * result.Count;
			TextLength = MaxText - textLength;
			IsTextLengthOver = TextLength < 0;
		}

		/// <summary>
		/// 画像消去ボタンのクリック
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="index">index</param>
		private void PreviewStackPanelOnRemoveClick(object sender, int index) {
			RemovePicture(index);
		}

		/// <summary>
		/// メディアボタンがクリックされたときに発生します
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="routedEventArgs">event</param>
		private void MediaButtonOnClick(object sender, RoutedEventArgs routedEventArgs) {
			AddPicture();
		}

		#endregion
	}
}
