using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CoreTweet;
using RTweet.Main.Config;

namespace RTweet.Main.Twitter {
	/// <summary>
	/// TwitterAPI(CoreTweet)へのアクセスをするためのクラス。
	/// このクラスはシングルトンクラスです
	/// </summary>
	internal partial class TwitterSystem {
		private static TwitterSystem _instance;

		public const string ApiKey = "dwZH1nSer18qyIoocJUIZFwmg";
		public const string ApiSecret = "Yj5v6Y4PzfF8FG3sGAXI7HNjZXTxTjLXvJW60rkEObbRr84Bi7";
		public const string LogDirectryPath = @"log\";
		public const string ConfigDirectryPath = @"config\";
		public const string CashDirectoryPath = @"cash\";

		/// <summary>
		/// このクラスのインスタンス
		/// </summary>
		public static TwitterSystem Instance => _instance ?? (_instance = new TwitterSystem());

		/// <summary>
		/// ユーザーのキーを管理するリスト
		/// </summary>
		public List<UserToken> UsetList = new List<UserToken>();

		/// <summary>
		/// Active状態のユーザー
		/// </summary>
		public UserToken ActiveUser { get; private set; }


		/// <summary>
		/// 初期化されたか
		/// </summary>
		public bool IsInitialized { get; private set; }


		/// <summary>
		/// 初期化します。
		/// ユーザーのアクセストークンなどの確認をします。
		/// </summary>
		public void Initialize() {
			if (IsInitialized) return;
			if (!File.Exists(@"keys.dat")) {
				//ファイルが存在しない
				MessageBox.Show("アカウントが関連付けられてません。初期設定をします。", "メッセージ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				Tokens token;
				do {
					token = GetTokens(false);
				} while (token == null);

				var u = new UserToken(token.UserId, token.AccessToken, token.AccessTokenSecret, token);
				ActiveUser = u;
				UsetList.Add(u);
				MainConfig.Instance.DefaultUserId = u.Id;
				MainConfig.Instance.SaveJson();

				using (var sw = new StreamWriter(@"Keys.dat", false, Encoding.UTF8)) {
					foreach (var user in UsetList) {
						sw.WriteLine(user.Id);
						sw.WriteLine(user.AccessToken);
						sw.WriteLine(user.TokenSecret);
					}
				}
			}
			else {
				//ファイルが存在する
				using (var sw = new StreamReader(@"Keys.dat")) {
					while (true) {
						var id = sw.ReadLine();
						if (id == null) break;
						var token = sw.ReadLine();
						var secret = sw.ReadLine();
						var user = new UserToken(long.Parse(id), token, secret);
						UsetList.Add(user);
						if (user.Id == MainConfig.Instance.DefaultUserId) ActiveUser = user;
					}

					//デフォルトユーザーの定義が無かったら
					if (ActiveUser == null) {
						ActiveUser = UsetList[0];
						MainConfig.Instance.DefaultUserId = ActiveUser.Id;
						MainConfig.Instance.SaveJson();
					}
				}
			}


			IsInitialized = true;
		}

		/// <summary>
		/// ユーザーを追加します。
		/// </summary>
		public void AddUser() {
			var token = GetTokens(true);
			if (token == null) return;
			Trace.WriteLine("UserAdding!!:");
			if (UsetList.Any(userToken => userToken.Id == token.UserId)) {
				MessageBox.Show("そのアカウントは既に登録されています。");
				Trace.WriteLine("AddUser:" + token.UserId + " This user is already registered!!");
				return;
			}
			var u = new UserToken(token.UserId, token.AccessToken, token.AccessTokenSecret, token);

			UsetList.Add(u);
			Trace.WriteLine("AddUser:" + token.UserId);
			using (var sw = new StreamWriter(@"Keys.dat", false, Encoding.UTF8)) {
				foreach (var user in UsetList) {
					sw.WriteLine(user.Id);
					sw.WriteLine(user.AccessToken);
					sw.WriteLine(user.TokenSecret);
				}
			}
		}

		/// <summary>
		/// ツイートします。
		/// </summary>
		/// <param name="text">ツイートするテキスト</param>
		/// <param name="mediaIds">メディアID</param>
		public void Tweet(string text, IEnumerable<long> mediaIds = null) {
			if (text == null) return;
			Trace.WriteLine("Tweet user:" + ActiveUser.ScreenName + "\r\n" + text + "\r\nend");
			try {
				ActiveUser.Tokens.Statuses.Update(text, media_ids: mediaIds);
			}
			catch (TwitterException e) {
				ErrorProcessing(e);
			}
		}


		/// <summary>
		/// 画像をアップロードします。
		/// </summary>
		/// <param name="file">アップローする画像ファイル</param>
		/// <returns></returns>
		public MediaUploadResult UploadPicture(FileInfo file) {
			Trace.WriteLine("Upload:" + file.FullName);
			return ActiveUser.Tokens.Media.Upload(file);
		}


		public void ChengeUser(UserToken token) {
			Trace.WriteLine("ChengeUser:" + Instance.ActiveUser.ScreenName);
			ActiveUser = token;
		}
	}
}