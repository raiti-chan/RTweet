using Newtonsoft.Json;

namespace RTweet.Main.Config {
	internal class StyleProperty {

		/// <summary>
		/// ツイートダイアログの背景色
		/// </summary>
		[JsonProperty(PropertyName = "tweetDialogBackgroundColor")]
		public string TweetDialogBackgroundColor { get; set; } = "#FFFFFF";

	}
}
