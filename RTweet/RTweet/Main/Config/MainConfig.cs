using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Windows;
using RTweet.Main.Twitter;
using RTweet.Windows.Dialog;

namespace RTweet.Main.Config {
	/// <summary>
	/// アプリケーションのコンフィグ
	/// </summary>
	internal class MainConfig {
		/// <summary>
		/// コンフィグファイルへのパス
		/// </summary>
		private const string ConfigFilePath = TwitterSystem.ConfigDirectryPath + @"config.json";

		/// <summary>
		/// コンフィグのインスタンス
		/// </summary>
		private static MainConfig _instance;

		/// <summary>
		/// インスタンスプロパティ―
		/// </summary>
		public static MainConfig Instance {
			get {
				if (_instance != null) return _instance; //インスタンスが既に存在していたら返す
				if (!File.Exists(ConfigFilePath)) {
					//ファイルが存在していなかったら新規作成
					_instance = new MainConfig();
					var jsonOut = JsonConvert.SerializeObject(_instance, Formatting.Indented);
					using (var sr = new StreamWriter(ConfigFilePath, false, Encoding.UTF8)) {
						sr.Write(jsonOut);
					}
					return _instance;
				}
				//ファイルが存在していたら読み込み
				string json;
				using (var sr = new StreamReader(ConfigFilePath, Encoding.UTF8)) {
					json = sr.ReadToEnd();
				}
				_instance = JsonConvert.DeserializeObject<MainConfig>(json) ?? new MainConfig();
				return _instance;
			}
		}

		/// <summary>
		/// Json形式でデータを保存します。
		/// </summary>
		public void SaveJson() {
			var jsonOut = JsonConvert.SerializeObject(_instance, Formatting.Indented);
			using (var sr = new StreamWriter(ConfigFilePath, false, Encoding.UTF8)) {
				sr.Write(jsonOut);
			}
		}

		/// <summary>
		/// 設定ウィンドウを開きます。
		/// </summary>
		public void OpenSettingWindow() {
			var window = new SettingWindow();
			window.ShowDialog();
		}


		/// <summary>
		/// デフォルトのユーザーID
		/// </summary>
		[JsonProperty(PropertyName = "defaultUserId")]
		public long DefaultUserId { get; set; } = 0;

		/// <summary>
		/// スタイル定義をXAMLリソースで行うか
		/// </summary>
		[JsonProperty(PropertyName = "IsStylingAtXAML")]
		public bool IsStylingAtXAML { get; set; } = false;

		/// <summary>
		/// スタイルが定義されたXAMLファイルへのパス
		/// </summary>
		[JsonProperty(PropertyName = "xamlFilePath")]
		public string xamlFilePath { get; set; } = "null";

		/// <summary>
		/// アプリケーションのStyle定義ファイル
		/// </summary>
		[JsonProperty(PropertyName = "style")]
		public StyleProperty Style { get; set; } = new StyleProperty();

		/// <summary>
		/// ツイートダイアログのポップアップ座標
		/// </summary>
		[JsonProperty(PropertyName = "tweetDialogPopuPoint")]
		public Point TweetDialogPopuPoint { get; set; } = new Point(20,20);

		/// <summary>
		/// マウスとの相対座標か
		/// </summary>
		[JsonProperty(PropertyName = "IsRelativeCoordinatesFromTheMouse")]
		public bool IsRelativeCoordinatesFromTheMouse { get; set; } = true;
	}
}