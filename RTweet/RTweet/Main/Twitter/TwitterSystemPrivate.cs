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
 詳しくはログファイルを参照してください。", @"認証エラー");
				Trace.WriteLine(e.Message);
				Trace.WriteLine(e.StackTrace);
				return null;
			}
		}
	}
}