using System.Windows;
using System.Windows.Input;
using RTweet.Main.Twitter;

namespace RTweet.Windows.Dialog {
	/// <summary>
	/// TweetDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class TweetDialog {
		public TweetDialog() {
			InitializeComponent();
			TweetText.Focus();
		}

		private void Window_Deactivated(object sender, System.EventArgs e) {
			Hide();
		}

		private void TweetText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
			if ((e.SystemKey != Key.LeftCtrl && e.SystemKey != Key.RightCtrl) || e.Key != Key.Enter) return;
		}

		private void TweetText_KeyUp(object sender, KeyEventArgs e) {}

		private void TweetText_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.Enter) return;
			TwitterSystem.Instance.Tweet(TweetText.Text);
			Hide();
		}
	}
}