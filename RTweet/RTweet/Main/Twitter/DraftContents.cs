

namespace RTweet.Main.Twitter {
	public class DraftContents {
		
		#region プロパティー
		/// <summary>
		/// ツイート内容
		/// </summary>
		public string Text { get; private set; }
		/// <summary>
		/// メディアへのファイルパス
		/// </summary>
		public string[] MediaFilePaths { get; private set; }
		#endregion

		/// <summary>
		/// 下書きのインスタンス
		/// </summary>
		/// <param name="text">下書きのツイート内容</param>
		/// <param name="filepath">添付されたメディアのパス</param>
		public DraftContents(string text, params string[] filepath) {
			Text = text;
			MediaFilePaths = filepath;
		}
	}
}