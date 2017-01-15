using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CoreTweet;
using Microsoft.Win32;
using RTweet.Main.Twitter;
using ToriatamaText;

namespace RTweet.Windows.Dialog {
	/// <summary>
	/// TweetDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class TweetDialog {
		/// <summary>
		/// このウインドウが閉じられるか
		/// </summary>
		public bool CanClose { get; set; } = true;

		/// <summary>
		/// UPする画像
		/// </summary>
		private readonly List<FileInfo> _upPictures = new List<FileInfo>(4);

		private const int MaxText = 140;

		public TweetDialog() {
			InitializeComponent();
			_fileDialog.Filter = "画像ファイル(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
			_fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		}

		public void ShowInit() {
			TweetText.Text = "";
			TweetText.Focus();
		}

		private void Window_Deactivated(object sender, System.EventArgs e) {
			if (!CanClose) return;
			Hide();
			TweetText.Text = "";
		}


		private void TweetText_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.Enter) return;
			if (int.Parse(TextCount.Content.ToString()) < 0) {
				CanClose = false;
				MessageBox.Show("文字数が制限をオーバーしています。", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
				Focus();
				TweetText.Focus();
				CanClose = true;
				return;
			}
			if (_upPictures.Count > 0) {
				var results = new MediaUploadResult[_upPictures.Count];
				for (var i = 0; i < results.Length; i++) results[i] = TwitterSystem.Instance.UploadPicture(_upPictures[i]);
				var ids = new long[results.Length];
				for (var i = 0; i < ids.Length; i++) ids[i] = results[i].MediaId;
				TwitterSystem.Instance.Tweet(TweetText.Text, ids);
				_upPictures.Clear();
				Picture.IsEnabled = true;
			}
			else {
				TwitterSystem.Instance.Tweet(TweetText.Text);
			}
			Hide();
			TweetText.Text = "";
		}

		private readonly Extractor _extractor = new Extractor();

		private void TweetText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			var result = _extractor.ExtractUrls(TweetText.Text);
			var textLength = TweetText.Text.Count(text => !char.IsLowSurrogate(text)) - result.Sum(x => x.Length) +
							23 * result.Count;

			TextCount.Content = MaxText - textLength;
			TextCount.Foreground = MaxText - textLength < 0 ? Brushes.Red : Brushes.Black;
		}

		private void Grid_KeyDown(object sender, KeyEventArgs e) {
			if (!CanClose || e.Key != Key.Escape) return;
			Hide();
			TweetText.Text = "";
		}

		private void TweetText_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (!CanClose || TweetText.IsFocused) return;
			Hide();
			TweetText.Text = "";
		}

		private readonly OpenFileDialog _fileDialog = new OpenFileDialog();

		private void Picture_Click(object sender, RoutedEventArgs e) {
			CanClose = false;
			var showDialog = _fileDialog.ShowDialog();
			if (showDialog != null && (bool) showDialog) {
				if (_upPictures.Count < 4) {
					_upPictures.Add(new FileInfo(_fileDialog.FileName));
				}
			}
			if (_upPictures.Count >= 4) Picture.IsEnabled = false;
			Focus();
			TweetText.Focus();
			CanClose = true;
		}
	}
}