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
				ErrorProcessing(e);
				return null;
			}
		}

		/// <summary>
		/// 発生したTwitterAPIのエラーを処理しっます。
		/// </summary>
		/// <param name="exception"></param>
		private static void ErrorProcessing(TwitterException exception) {
			Trace.WriteLine("TwitterAPIError!!");
			Trace.WriteLine("StackTrace");
			Trace.WriteLine(exception);

			foreach (var exceptionError in exception.Errors) {
				Trace.WriteLine("ErrorCode:" + exceptionError.Code);
				Trace.WriteLine("Message:" + exceptionError.Message);
				switch (exceptionError.Code) {
					case 0:
						break;
					case 34:
						App.Notification("指定されたAPIが存在しません。\r\n通常起こるはずのないエラーです。");
						break;
					case 44:
						App.Notification("attachment_url には引用リツイートするツイートのURLしか渡せません。");
						break;
					case 50:
						App.Notification("ダイレクトメッセ―ジ指定のツイートに指定されたユーザーは存在しません。");
						break;
					case 67:
						App.Notification("Twitterのバックエンドサービスがストップしています。\r\n回復までお待ちください。");
						break;
					case 68: 
						App.Notification("廃止されたTwitterAPI 1.0を呼んでいます。\r\n通常起こるはずのないエラーです。");
						break;
					case 88:
						Trace.WriteLine("Reset:" + exception.RateLimit.Reset.ToString("g"));
						App.Notification("API利用制限に達しました。\r\n" + (exception.RateLimit.Reset - DateTimeOffset.Now).ToString("HH時間,mm分") + "後に再度試してください。");
						break;
					case 89:
						App.Notification("Bearer tokenが正しくありません。\r\n通常起こるはずのないエラーです。");
						break;
					case 93:
						App.Notification("ダイレクトメッセージのアクセスが許可されてません。\r\n通常起こるはずのないエラーです。");
						break;
					case 99:
						App.Notification("OAuth2 Bearer Token の認証に失敗しました。\r\n通常起こるはずのないエラーです。");
						break;
					case 130:
						App.Notification("Twitter側のエラーです。\r\nしばらく時間をおいてから再度試してしてください。");
						break;
					case 131:
						App.Notification("Twitter側で原因不明のエラーが発生しました。\r\nしばらく時間をおいてから再度試してしてみてください。");
						break;
					case 135:
						App.Notification("OAuthの認証に失敗しました。\r\n通常起こるはずのないエラーです。");
						break;
					case 150:
						App.Notification("ダイレクトメッセージ指定のツイートが送信できませんでした。");
						break;
					case 185:
						App.Notification("ツイート数の制限に達しました。\r\nしばらく時間をおいてから再度送信してみてください。");
						break;
					case 186:
						App.Notification("ツイートが140字を超えました。");
						break;
					case 187:
						App.Notification("ツイートが重複しています。");
						break;
					case 188:
						App.Notification("添付されたURLが不正なサイトと判定されました。");
						break;
					case 189:
						App.Notification("原因不明なエラーが発生しました。\r\n少し時間をおいてから再送信してみてください。");
						break;
					case 190:
						App.Notification("ツイートが長すぎます。");
						break;
					case 191:
						Trace.WriteLine("Reset:" + exception.RateLimit.Reset.ToString("g"));
						App.Notification("画像送信制限に達しました。\r\n" + (exception.RateLimit.Reset - DateTimeOffset.Now).ToString("HH時間,mm分") + "後に再度送信してみてください。");
						break;
					case 226:
						App.Notification("アカウントがスパム判定されています。\r\nしばらく経ってから再送信してみてください。");
						break;
					case 326:
						App.Notification("アカウントがハック判定されています。\r\nメールが届いてないか確認してください。");
						break;
					default:
						App.Notification("不明なエラーが発生しました。\r\nエラーコード:" + exceptionError.Code + "\r\n メッセージ:" + exceptionError.Message);
						break;
				}

			}
		}
	}
}