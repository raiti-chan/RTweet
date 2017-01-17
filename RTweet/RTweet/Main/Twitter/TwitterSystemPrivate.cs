using System;
using System.Diagnostics;
using System.Windows.Forms;
using CoreTweet;
using RTweet.Windows.Dialog;

namespace RTweet.Main.Twitter {
	internal partial class TwitterSystem {
		/// <summary>
		/// PINコード入力ダイアログを開き、アクセストークンを取得します、
		/// </summary>
		/// <param name="canCancel">k</param>
		/// <returns></returns>
		private static Tokens GetTokens(bool canCancel) {
			var session = OAuth.Authorize(ApiKey, ApiSecret);
			Trace.WriteLine("OpenURL:" + session.AuthorizeUri.AbsoluteUri);
			Process.Start(session.AuthorizeUri.AbsoluteUri); //認証ページを既定のブラウザで開く
			var pinDialog = new PinInputDialog(canCancel);
			var showDialog = pinDialog.ShowDialog();
			if (showDialog != null && !showDialog.Value) return null;
			var pin = pinDialog.PinInput.Text;
			try {
				var token = session.GetTokens(pin);
				return token;
			}
			catch (TwitterException e) {
				MessageBox.Show(@"PINコードの認証エラーが発生しました。
 PINコードが正しくないかネットワークに通ながってない可能性があります。
 詳しくはログファイルを参照してください。", @"認証エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Instance.ErrorProcessing(e);
				return null;
			}
		}

		/// <summary>
		/// 発生したTwitterAPIのエラーを処理しっます。
		/// </summary>
		/// <param name="exception"></param>
		private void ErrorProcessing(TwitterException exception) {
			Trace.WriteLine("TwitterAPIError!!");
			Trace.WriteLine("StackTrace");
			Trace.WriteLine(exception);

			foreach (var exceptionError in exception.Errors) {
				Trace.WriteLine("ErrorCode:" + exceptionError.Code);
				Trace.WriteLine("Message:" + exceptionError.Message);
				switch (exceptionError.Code) {
					case 0:
						break;
					case 187:
						App.Notification("ツイートが重複しています。");
						break;
					case 190:
						App.Notification("ツイートが長すぎます。");
						break;
					case 191:
						Trace.WriteLine("Reset:" + exception.RateLimit.Reset.ToString("g"));
						App.Notification("画像送信制限に達しました。\r\n" + (exception.RateLimit.Reset - DateTimeOffset.Now).ToString("HH時間,MM分") + "後に再度送信してみてください。");
						break;
					case 226:
						App.Notification("アカウントがスパム判定されています。\r\nしばらく経ってから再送信してみてください。");
						break;
					case 326:
						App.Notification("アカウントがハック判定されています。\r\nメールが届いてないか確認してください。");
						break;
					default:
						App.Notification("不明なエラーが発生しました\r\nエラーコード:" + exceptionError.Code + "\r\n メッセージ:" + exceptionError.Message);
						break;
				}

			}
		}
	}
}