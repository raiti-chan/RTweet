using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

		private readonly List<BitmapImage> _previewImages = new List<BitmapImage>(4);

		private readonly Image[] _previewPanels = new Image[4];

		private const int MaxText = 140;

		public TweetDialog() {
			InitializeComponent();
			_fileDialog.Filter = "画像ファイル(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
			_fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			_previewPanels[0] = Image1;
			_previewPanels[1] = Image2;
			_previewPanels[2] = Image3;
			_previewPanels[3] = Image4;
			foreach (var panel in _previewPanels) {
				panel.MouseEnter += PreviewPanel_MouseEnter;
				panel.MouseLeave += PreviewPanel_MouseLeave;
			}
			Remove1.MouseEnter += PreviewPanel_MouseEnter;
			Remove2.MouseEnter += PreviewPanel_MouseEnter;
			Remove3.MouseEnter += PreviewPanel_MouseEnter;
			Remove4.MouseEnter += PreviewPanel_MouseEnter;

			Remove1.Click += Remove_Click;
			Remove2.Click += Remove_Click;
			Remove3.Click += Remove_Click;
			Remove4.Click += Remove_Click;

			Topmost = true;
		}

		public void ShowInit() {
			TweetText.Focus();
			var point = System.Windows.Forms.Cursor.Position;
			Top = point.Y;
			Left = point.X;
		}

		private void Window_Deactivated(object sender, EventArgs e) {
			WindowClose();
		}

		private void Remove_Click(object sender, RoutedEventArgs e) {
			if (Equals(sender, Remove1)) {
				RemovePicture(0);
			}
			else if (Equals(sender, Remove2)) {
				RemovePicture(1);
			}
			else if (Equals(sender, Remove3)) {
				RemovePicture(2);
			}
			else if (Equals(sender, Remove4)) {
				RemovePicture(3);
			}
		}

		private void PreviewPanel_MouseEnter(object sender, MouseEventArgs e) {
			if (Equals(sender, Image1) || Equals(sender, Remove1)) {
				Remove1.Visibility = Visibility.Visible;
			}
			else if (Equals(sender, Image2) || Equals(sender, Remove2)) {
				Remove2.Visibility = Visibility.Visible;
			}
			else if (Equals(sender, Image3) || Equals(sender, Remove3)) {
				Remove3.Visibility = Visibility.Visible;
			}
			else if (Equals(sender, Image4) || Equals(sender, Remove4)) {
				Remove4.Visibility = Visibility.Visible;
			}
		}

		private void PreviewPanel_MouseLeave(object sender, MouseEventArgs e) {
			if (Equals(sender, Image1)) {
				Remove1.Visibility = Visibility.Hidden;
			}
			else if (Equals(sender, Image2)) {
				Remove2.Visibility = Visibility.Hidden;
			}
			else if (Equals(sender, Image3)) {
				Remove3.Visibility = Visibility.Hidden;
			}
			else if (Equals(sender, Image4)) {
				Remove4.Visibility = Visibility.Hidden;
			}
		}


		private void TweetText_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.Enter) return;
			if (int.Parse(TextCount.Content.ToString()) < 0) {
				CanClose = false;
				MessageBox.Show("文字数が制限をオーバーしています。", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
				Activate();
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
				Picture.IsEnabled = true;
			}
			else {
				TwitterSystem.Instance.Tweet(TweetText.Text);
			}
			WindowClose();
		}

		private readonly Extractor _extractor = new Extractor();

		private void TweetText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			var result = _extractor.ExtractUrls(TweetText.Text);
			var textLength = TweetText.Text.Count(text => !char.IsLowSurrogate(text)) - result.Sum(x => x.Length) +
							23 * result.Count;

			TextCount.Content = MaxText - textLength;
			TextCount.Foreground = MaxText - textLength < 0 ? Brushes.Red : Brushes.Black;
		}

		private bool _isFunction;

		private void Grid_KeyDown(object sender, KeyEventArgs e) {
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (e.Key) {
				case Key.Escape:
					if (!CanClose) break;
					WindowClose();
					break;
				case Key.P:
					if (Keyboard.Modifiers != ModifierKeys.Control && _upPictures.Count >= 4) break;
					_isFunction = true;
					AddPicture();
					break;
			}
		}


		private readonly OpenFileDialog _fileDialog = new OpenFileDialog();

		private void Picture_Click(object sender, RoutedEventArgs e) {
			AddPicture();
		}

		private void AddPicture() {
			CanClose = false;
			var showDialog = _fileDialog.ShowDialog();
			if (showDialog != null && (bool) showDialog) {
				if (_upPictures.Count < 4) {
					_upPictures.Add(new FileInfo(_fileDialog.FileName));
					_previewImages.Add(new BitmapImage(new Uri(_fileDialog.FileName)));
					_previewPanels[_previewImages.Count - 1].Source = _previewImages[_previewImages.Count - 1];
					Width = _upPictures.Count > 0 ? 430 : 310;
					PicturePreviewPanel.Height = _upPictures.Count * 105;
				}
			}
			if (_upPictures.Count >= 4) Picture.IsEnabled = false;
			Focus();
			TweetText.Focus();
			CanClose = true;
		}

		private void RemovePicture(int index) {
			_upPictures.RemoveAt(index);
			_previewImages.RemoveAt(index);
			if (_upPictures.Count >= 4) Picture.IsEnabled = false;
			for (var i = 0; i < _previewImages.Count; i++) _previewPanels[i].Source = _previewImages[i];
			PicturePreviewPanel.Height = _upPictures.Count * 105;
			Width = _upPictures.Count > 0 ? 430 : 310;
		}


		private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			e.Handled = _isFunction;
			_isFunction = false;
		}

		private void WindowClose() {
			if (!CanClose) return;
			Hide();
			_upPictures.Clear();
			_previewImages.Clear();
			TweetText.Text = "";
			Width = 310;
			PicturePreviewPanel.Height = 100;
		}
	}
}