using System.Windows.Controls;
using RTweet.Main.Twitter;

namespace RTweet.Windows.Control {
	/// <summary>
	/// DraftListItem.xaml の相互作用ロジック
	/// </summary>
	public partial class DraftListItem {

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DraftListItem(DraftContents contents) {
			InitializeComponent();

			Text.Text = contents.Text;
			if (contents.MediaFilePaths.Length > 0) {
				Status.Content = "メディア:" + contents.MediaFilePaths.Length;
			} else {
				Status.Content = "メディア無し";
			}
		}


	}
}
