using System;
using System.Windows;
using System.Windows.Input;

namespace RTweet.Windows.Control {
	/// <summary>
	/// PicturePreviewPanel.xaml の相互作用ロジック
	/// 画像のプレビュー用コントロール
	/// </summary>
	public partial class PicturePreviewPanel {

		/// <summary>
		/// この要素のインデックス
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// 初期化
		/// <param name="index">この要素のインデックス。デフォルト0</param>
		/// <exception cref="ArgumentOutOfRangeException">indexが0以下の場合スローされます</exception>
		/// </summary>
		public PicturePreviewPanel(int index = 0) {
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
			Index = index;
			InitializeComponent();
		}

		/// <summary>
		/// 画像の上にマウスカーソルが来た時、消去ボタンを表示する。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Image_MouseEnter(object sender, MouseEventArgs e) {
			RemoveButton.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// 画像上からマウスが外れたら、消去ボタンを隠す。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void Image_MouseLeave(object sender, MouseEventArgs e) {
			RemoveButton.Visibility = Visibility.Hidden;
		}

		/// <summary>
		/// ボタン上にカーソルが行っても消えないようにする。
		/// </summary>
		/// <param name="sender">イベント発生元</param>
		/// <param name="e">イベント</param>
		private void RemoveButton_MouseEnter(object sender, MouseEventArgs e) {
			RemoveButton.Visibility = Visibility.Visible;
		}

		
	}
}