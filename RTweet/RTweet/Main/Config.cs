using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TwitterClient.Main {
	internal class Config {

		private const string ConfigFilePath = "config/config.json";

		private static Config _instance;

		public static Config Instance {
			get {
				if (_instance != null) return _instance;//インスタンスが既に存在していたら返す
				if (!File.Exists(ConfigFilePath)) return (_instance = new Config());//ファイルが存在していなかったら新規作成
				//ファイルが存在していたら読み込み
				string json;
				using (var sr = new StreamReader(ConfigFilePath, Encoding.UTF8)) {
					json = sr.ReadToEnd();
				}
				_instance = JsonConvert.DeserializeObject<Config>(json);
				return _instance;
			}
		}
	}
}
