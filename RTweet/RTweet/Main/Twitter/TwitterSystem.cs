using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CoreTweet;
using RTweet.Windows.Dialog;

namespace RTweet.Main.Twitter {
	/// <summary>
	/// TwitterAPI(CoreTweet)へのアクセスをするためのクラス。
	/// このクラスはシングルトンクラスです
	/// </summary>
	internal class TwitterSystem {
		private static TwitterSystem _instance;

		public const string ApiKey = "dwZH1nSer18qyIoocJUIZFwmg";
		public const string ApiSecret = "Yj5v6Y4PzfF8FG3sGAXI7HNjZXTxTjLXvJW60rkEObbRr84Bi7";

		/// <summary>
		/// このクラスのインスタンス
		/// </summary>
		public static TwitterSystem Instance => _instance ?? (_instance = new TwitterSystem());


		private TwitterSystem() {}

		/// <summary>
		/// ユーザーのキーを管理するリスト
		/// </summary>
		private List<UserToken> UsetList = new List<UserToken>();

		/// <summary>
		/// 
		/// </summary>
		private UserToken ActiveUser;


		/// <summary>
		/// 初期化されたか
		/// </summary>
		public bool IsInitialized { get; private set; } = false;


		/// <summary>
		/// 初期化します。
		/// ユーザーのアクセストークンなどの確認をします。
		/// </summary>
		public void Initialize() {
			if (IsInitialized) return;
			if (!System.IO.File.Exists("keys.dat")) {//ファイルが存在しない
				MessageBox.Show("アカウントが関連付けられてません。初期設定をします。", "メッセージ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				Tokens token;
				do {
					token = GetTokens(false);
				} while (token == null);

				var u = new UserToken(token.UserId.ToString(), token.AccessToken, token.AccessTokenSecret, token);
				ActiveUser = u;
				UsetList.Add(u);

				using (var sw = new StreamWriter(@"Keys.dat", false, Encoding.UTF8)) {
					foreach (var user in UsetList) {
						sw.WriteLine(user.Id);
						sw.WriteLine(user.AccessToken);
						sw.WriteLine(user.TokenSecret);
					}
				}
				
			}
			else {}


			IsInitialized = true;
		}

		private static Tokens GetTokens(bool canCancel) {
			var session = OAuth.Authorize(ApiKey, ApiSecret);
			System.Diagnostics.Process.Start(session.AuthorizeUri.AbsoluteUri);//認証ページを既定のブラウザで開く
			var pinDialog = new PinInputDialog(canCancel);
			pinDialog.ShowDialog();
			var pin = pinDialog.PinInput.Text;
			try {
				var token = session.GetTokens(pin);
				return token;
			}
			catch (TwitterException) {
				MessageBox.Show("PINコードの認証エラーが発生しました。\n" +
								"PINコードが正しくないかネットワークに通ながってない可能性があります。", "認証エラー");
				return null;
			}

		}
	}
}