using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using RTweet.Main.Twitter;

namespace RTweet.Windows.Dialog {
	/// <summary>
	/// TweetDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class TweetDialog {

		/// <summary>
		/// このウインドウが閉じられるか
		/// </summary>
		public bool CanClose { get; set; } = true;

		private const int MaxText = 140;

		public TweetDialog() {
			InitializeComponent();
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
			TwitterSystem.Instance.Tweet(TweetText.Text);
			Hide();
			TweetText.Text = "";
		}

		private void TweetText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			var textLength = TweetText.Text.Count(text => !text.Equals('\n'));
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
	}
}