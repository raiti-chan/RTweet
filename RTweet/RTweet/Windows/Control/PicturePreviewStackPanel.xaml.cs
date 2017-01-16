using System;
using System.Windows.Media.Imaging;

namespace RTweet.Windows.Control {
	/// <summary>
	/// PicturePreviewStackPanel.xaml の相互作用ロジック
	/// 画像を複数表示するためのスタックパネル
	/// </summary>
	public partial class PicturePreviewStackPanel {

		/// <summary>
		/// 画像消去ボタンがクリックされたときに発生するイベント
		/// </summary>
		public event EventHandler<int> RemoveClick;

		/// <summary>
		/// 保持している固定の<see cref="PicturePreviewPanel"/>
		/// </summary>
		private readonly PicturePreviewPanel[] _panels = {
			new PicturePreviewPanel(), new PicturePreviewPanel(1),
			new PicturePreviewPanel(2), new PicturePreviewPanel(3)
		};

		/// <summary>
		/// 保持しているイメージ
		/// </summary>
		private readonly BitmapImage[] _images = new BitmapImage[4];

		/// <summary>
		/// イメージインデックスへのリンク
		/// </summary>
		private readonly int[] _links = {-1, -1, -1, -1};

		/// <summary>
		/// 格納されてるイメージ数
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// 保持している<see cref="PicturePreviewPanel"/>への参照。範囲は0 ～ 3。
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public PicturePreviewPanel this[int index] => _panels[index];

		/// <summary>
		/// 初期化
		/// </summary>
		public PicturePreviewStackPanel() {
			InitializeComponent();
			foreach (var panel in _panels) {
				StackPanel.Children.Add(panel);
				panel.RemoveButton.Click += (sender, e) => OnRemoveClick(panel.Index);
			}
		}

		/// <summary>
		/// 画像消去のイベント発生
		/// </summary>
		/// <param name="index">画像インデックス</param>
		protected virtual void OnRemoveClick(int index) {
			RemoveClick?.Invoke(this, index);
		}

		/// <summary>
		/// 画像の追加
		/// </summary>
		/// <param name="image">画像</param>
		public void AddImage(BitmapImage image) {
			if (Count >= 4) throw new IndexOutOfRangeException("格納できる限界を超えました。");
			var index = 0;
			for (; index < _images.Length; index++) if (_images[index] == null) break; //一番小さい空き領域インデックスを求める
			_images[index] = image; //空き領域に格納
			_links[Count] = index; //格納されたインデックスをリンクとして保持
			_panels[Count].Image.Source = image; //特に更新する必要が無いのでそのまま画像を入れる。
			++Count;
			Height = Count * 105;
		}

		/// <summary>
		/// 画像の消去
		/// </summary>
		/// <param name="index">消去する画像のインデックス</param>
		public void RemoveImage(int index) {
			if (index >= 4) throw new IndexOutOfRangeException("Indexが有効範囲外です。");
			_images[_links[index]] = null;
			for (var i = index; i < _links.Length - 1; i++) {
				_links[i] = _links[i + 1];
			}
			_links[3] = -1;
			--Count;
			SourceUpdate();
		}

		/// <summary>
		/// 表示する画像を更新します。
		/// </summary>
		public void SourceUpdate() {
			for (var i = 0; i < _links.Length; i++) {
				_panels[i].Image.Source = _links[i] < 0 ? null : _images[_links[i]];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear() {
			for (var i = 0; i < _images.Length; i++) {
				_images[i] = null;
				_links[i] = -1;
				_panels[i].Image.Source = null;
			}
			Count = 0;
			Height = 100;
		}
	}
}