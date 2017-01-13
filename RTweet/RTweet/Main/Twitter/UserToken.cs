using System.IO;
using System.Net;
using CoreTweet;

namespace RTweet.Main.Twitter {
	/// <summary>
	/// ユーザーのアクセストークンを表す構造体
	/// </summary>
	internal class UserToken {
		/// <summary>
		/// ユーザーの数値ID
		/// </summary>
		public long Id { get; private set; }

		/// <summary>
		/// ユーザーの名前(@ID)
		/// </summary>
		public string ScreenName => UserResponse.ScreenName;


		/// <summary>
		/// アクセストークン
		/// </summary>
		public string AccessToken { get; private set; }

		/// <summary>
		/// シークレットアクセストークン
		/// </summary>
		public string TokenSecret { get; private set; }

		public string ProfileImgPath => TwitterSystem.CashDirectoryPath + Id + @"\profileImg.jpg";

		private UserResponse _userResponse;

		/// <summary>
		/// ユーザーの情報
		/// </summary>
		public UserResponse UserResponse
			=> _userResponse ?? (_userResponse = Tokens.Account.VerifyCredentials(true, false, false));

		private Tokens _tokens;

		/// <summary>
		/// トークンオブジェクト
		/// </summary>
		public Tokens Tokens {
			get {
				if (_tokens != null) return _tokens;
				_tokens = Tokens.Create(TwitterSystem.ApiKey, TwitterSystem.ApiSecret, AccessToken, TokenSecret);
				_tokens.Account.VerifyCredentials();
				return _tokens;
			}
		}

		/// <summary>
		/// 構造体の生成
		/// </summary>
		/// <param name="id">ユーザーID</param>
		/// <param name="accessToken">アクセストークン</param>
		/// <param name="tokenSecret">シークレットアクセストークン</param>
		/// <param name="tokens">トークンオブジェクト デフォルトでnull</param>
		public UserToken(long id, string accessToken, string tokenSecret, Tokens tokens = null) {
			Id = id;
			AccessToken = accessToken;
			TokenSecret = tokenSecret;
			_tokens = tokens;
			ProfileImageDownload();
		}


		public override string ToString() {
			return ScreenName;
		}

		public void ProfileImageDownload() {
			if (!Directory.Exists(TwitterSystem.CashDirectoryPath + Id + @"\"))
				Directory.CreateDirectory(TwitterSystem.CashDirectoryPath + Id + @"\");
			using (var wc = new WebClient()) {
				var dUrl = UserResponse.ProfileImageUrl;
				var url = dUrl.Remove(dUrl.Length - 11) + ".jpg";
				wc.DownloadFile(url, Path.GetFullPath(ProfileImgPath));
				wc.Dispose();
			}
		}
	}
}