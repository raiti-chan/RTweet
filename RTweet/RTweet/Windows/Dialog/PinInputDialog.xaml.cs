using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace RTweet.Windows.Dialog {
	/// <summary>
	/// PinInputDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class PinInputDialog {
		private readonly bool _canCancel;

		public PinInputDialog(bool canCancel) {
			_canCancel = canCancel;
			InitializeComponent();
			Cancel.IsEnabled = canCancel;
		}

		/// <summary>
		/// 入力の制限
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void PinInput_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
		}

		private bool IsOk { get; set; }

		/// <summary>
		/// Windowの閉じるボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (IsOk) return;
			if (_canCancel) {
				DialogResult = false;
				return;
			}
			var result = MessageBox.Show("アプリケーションを終了しますか?", "アプリケーションの終了",
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
			if (result == MessageBoxResult.Yes) Environment.Exit(0);
			else e.Cancel = true;
		}



		/// <summary>
		/// 決定ボタン
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void Button_Click(object sender, RoutedEventArgs e) {
			if (!CanClose()) return;
			DialogResult = true;
			Close();
		}

		/// <summary>
		/// PINコードが入力されているかのチェック
		/// </summary>
		/// <returns></returns>
		private bool CanClose() {
			if (Regex.IsMatch(PinInput.Text, "^[0-9]+$")) {
				IsOk = true;
				return true;
			}

			MessageBox.Show("PINコードを入力してください");
			return false;
		}

		/// <summary>
		/// ボックス内でのエンターキー処理
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void PinInput_KeyDown(object sender, KeyEventArgs e) {
			if (e.Key != Key.Enter) return;
			if (!CanClose()) return;
			DialogResult = true;
			Close();
		}


		/// <summary>
		/// キャンセル
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}
	}
}