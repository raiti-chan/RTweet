using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace RTweet.Main {
	internal class Config {

		private const string ConfigFilePath = @"config\config.json";

		private static Config _instance;

		public static Config Instance {
			get {
				if (_instance != null) return _instance;//インスタンスが既に存在していたら返す
				if (!File.Exists(ConfigFilePath)) {
					//ファイルが存在していなかったら新規作成
					_instance = new Config();
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
				_instance = JsonConvert.DeserializeObject<Config>(json);
				return _instance;
			}
		}

		[JsonProperty(PropertyName = "defaultUserID")]
		public long DefaultUserID { get; set; } = 0;


		public void SaveJson() {
			var jsonOut = JsonConvert.SerializeObject(_instance, Formatting.Indented);
			using (var sr = new StreamWriter(ConfigFilePath, false, Encoding.UTF8)) {
				sr.Write(jsonOut);
			}
		}

	}
}
