using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using CoreTweet;
using Microsoft.Win32;
using RTweet.Main.Config;
using RTweet.Main.Twitter;
using ToriatamaText;


namespace RTweet.Windows.Dialog {
	/// <summary>
	/// TweetDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class TweetDialog {
		/// <summary>
		/// ツイートの最大文字数
		/// </summary>
		private const int MaxText = 140;

		/// <summary>
		/// このウインドウが閉じられるか
		/// </summary>
		public bool CanClose { get; set; } = true;

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
		/// ファイル選択ダイアログ
		/// </summary>
		private readonly OpenFileDialog _fileDialog = new OpenFileDialog();


		/// <summary>
		/// 初期化
		/// </summary>
		public TweetDialog() {
			InitializeComponent();
			_fileDialog.Filter = "画像ファイル(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
			_fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			Topmost = true;
		}

		/// <summary>
		/// 表示する前の初期化
		/// </summary>
		public void ShowInit() {
			TweetText.Focus();
			if (MainConfig.Instance.IsRelativeCoordinatesFromTheMouse) {
				var point = System.Windows.Forms.Cursor.Position;
				Top = point.Y + MainConfig.Instance.TweetDialogPopuPoint.X;
				Left = point.X + MainConfig.Instance.TweetDialogPopuPoint.Y;
			} else {
				Top = MainConfig.Instance.TweetDialogPopuPoint.X;
				Left = MainConfig.Instance.TweetDialogPopuPoint.Y;
			}

		}

		/// <summary>
		/// 送信レジスターに画像を登録します。
		/// </summary>
		private void AddPicture() {
			CanClose = false;
			var showDialog = _fileDialog.ShowDialog();
			if (showDialog != null && (bool) showDialog) {
				if (_upPictures.Count < 4) {
					_upPictures.Add(new FileInfo(_fileDialog.FileName));
					PreviewStackPanel.AddImage(new BitmapImage(new Uri(_fileDialog.FileName)));
					Width = _upPictures.Count > 0 ? 430 : 310;
				}
			}
			Picture.IsEnabled = _upPictures.Count < 4;
			Focus();
			TweetText.Focus();
			CanClose = true;
		}

		/// <summary>
		/// 送信レジスターに登録された画像を消去します。
		/// </summary>
		/// <param name="index">消去する画像のインデックス</param>
		private void RemovePicture(int index) {
			_upPictures.RemoveAt(index);
			PreviewStackPanel.RemoveImage(index);
			Width = _upPictures.Count > 0 ? 430 : 310;
			Picture.IsEnabled = _upPictures.Count < 4;

		}


		/// <summary>
		/// ウィンドウを閉じようと試みます。
		/// <see cref="CanClose"/>がfalseの場合閉じません
		/// </summary>
		private void WindowClose() {
			if (!CanClose) return; //CanCloseがfalseの場合無視
			Hide(); //ウィンドウを隠す
			_upPictures.Clear(); //画像レジスターをクリア
			PreviewStackPanel.Clear(); //プレビューイメージを初期化
			TweetText.Text = ""; //テキストを空に

			//ウィンドウサイズを初期値にする。
			Width = 310;
		}

		//--------------------------------------------------------------------------------------ここから先はイベントメソッド

		/// <summary>
		/// ウィンドウが非アクティブ化された場合、ウィンドウを閉じます。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Window_Deactivated(object sender, EventArgs e) {
			WindowClose();
		}

		/// <summary>
		/// ツイート入力欄に入力された時のイベント
		/// コマンド入力の場合、文字入力させないようにする。　　
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			e.Handled = _isHotKey;
			_isHotKey = false;
		}

		/// <summary>
		/// ホットキーの処理。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Grid_KeyDown(object sender, KeyEventArgs e) {
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (e.Key) {
				case Key.Escape: //ウィンドウを閉じる
					if (!CanClose) break;
					WindowClose();
					break;
				case Key.P: //画像を添付
					if (Keyboard.Modifiers != ModifierKeys.Control || _upPictures.Count >= 4) break;
					_isHotKey = true;
					AddPicture();
					break;
			}
		}

		/// <summary>
		/// ツイート文入力欄に入力された時のイベント。
		/// 文字数の計算をします。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void TweetText_TextChanged(object sender, TextChangedEventArgs e) {
			var result = _extractor.ExtractUrls(TweetText.Text);
			var textLength = TweetText.Text.Count(text => !char.IsLowSurrogate(text)) - result.Sum(x => x.Length) +
							23 * result.Count;
			TextCount.Content = MaxText - textLength;
			TextCount.Foreground = MaxText - textLength < 0 ? Brushes.Red : Brushes.Black;
		}

		/// <summary>
		/// Ctrl + Enter が押された場合の送信処理
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void TweetText_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.Enter) return; //Ctrl + Enter でない場合無視

			if (int.Parse(TextCount.Content.ToString()) < 0) {
				//文字数がオーバーしていた場合
				CanClose = false;
				MessageBox.Show("文字数が制限をオーバーしています。", "警告", MessageBoxButton.OK, MessageBoxImage.Error); //警告ダイアログの表示
				Activate();
				TweetText.Focus();
				CanClose = true;
				return;
			}

			if (_upPictures.Count > 0) {
				//画像がレジスターに追加されていた場合
				var upResults = new MediaUploadResult[_upPictures.Count];
				for (var i = 0; i < upResults.Length; i++) upResults[i] = TwitterSystem.Instance.UploadPicture(_upPictures[i]); //画像をアップロード
				var ids = new long[upResults.Length]; //ID格納配列
				for (var i = 0; i < ids.Length; i++) ids[i] = upResults[i].MediaId; //IDを代入
				TwitterSystem.Instance.Tweet(TweetText.Text, ids); //メディアを添付してツイートを送信
			} else {
				TwitterSystem.Instance.Tweet(TweetText.Text); //テキストをツイート
			}
			WindowClose(); //ウィンドウを閉じる
		}

		/// <summary>
		/// 画像ボタンのクリック
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Picture_Click(object sender, RoutedEventArgs e) {
			AddPicture(); //画像を追加する
		}

		/// <summary>
		/// 画像消去ボタンのクリック
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">発信元インデックス</param>
		private void PreviewStackPanel_RemoveClick(object sender, int e) {
			RemovePicture(e);
			PreviewStackPanel[e].RemoveButton.Visibility = Visibility.Hidden;
		}

		/// <summary>
		/// パネルの上で左ボタンを押す
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			DragMove();
		}
	}
}