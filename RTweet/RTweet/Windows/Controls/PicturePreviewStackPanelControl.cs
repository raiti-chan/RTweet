using System;
using System.Windows;
using System.Windows.Media.Imaging;


namespace RTweet.Windows.Controls {
	/// <summary>
	/// 画像を複数表示するためのスタックパネル
	/// 
	/// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
	///
	/// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:RTweet.Windows.Controls"
	///
	///
	/// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:RTweet.Windows.Controls;assembly=RTweet.Windows.Controls"
	///
	/// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
	/// リビルドして、コンパイル エラーを防ぐ必要があります:
	///
	///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
	///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
	///
	///
	/// 手順 2)
	/// コントロールを XAML ファイルで使用します。
	///
	///     <MyNamespace:PicturePreviewStackPanelControl/>
	///
	/// </summary>
	public class PicturePreviewStackPanelControl : System.Windows.Controls.Control {
		#region コンストラクタ

		/// <summary>
		/// staticコンストラクタ
		/// </summary>
		static PicturePreviewStackPanelControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PicturePreviewStackPanelControl),
				new FrameworkPropertyMetadata(typeof(PicturePreviewStackPanelControl)));
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public PicturePreviewStackPanelControl() {
			Height = 0;
		}

		#endregion

		#region イベント

		/// <summary>
		/// 画像消去ボタンがクリックされたときに発生するイベント
		/// </summary>
		public event EventHandler<int> RemoveClick;

		/// <summary>
		/// 画像消去のイベント発生
		/// </summary>
		/// <param name="index">画像インデックス</param>
		protected virtual void OnRemoveClick(int index) {
			RemoveClick?.Invoke(this, index);
		}

		#endregion

		#region フィールド

		/// <summary>
		/// 保持しているイメージ
		/// </summary>
		private readonly BitmapImage[] _images = new BitmapImage[4];

		/// <summary>
		/// 画像表示パネル0
		/// </summary>
		private PicturePreviewPanelControl _panel0;

		/// <summary>
		/// 画像表示パネル1
		/// </summary>
		private PicturePreviewPanelControl _panel1;

		/// <summary>
		/// 画像表示パネル2
		/// </summary>
		private PicturePreviewPanelControl _panel2;

		/// <summary>
		/// 画像表示パネル3
		/// </summary>
		private PicturePreviewPanelControl _panel3;

		#endregion

		#region プロパティ

		/// <summary>
		/// 保持している<see cref="PicturePreviewPanelControl"/>
		/// インデックスは0 ~ 3
		/// </summary>
		public PicturePreviewPanelControl this[int index] {
			set {
				switch (index) {
					case 0:
						_panel0 = value;
						break;
					case 1:
						_panel1 = value;
						break;
					case 2:
						_panel2 = value;
						break;
					case 3:
						_panel3 = value;
						break;
					default:
						throw new IndexOutOfRangeException("PicturePreviewStackPanelControlのインデックスは0 ～ 3の範囲です");
				}
			}
			get {
				switch (index) {
					case 0:
						return _panel0;
					case 1:
						return _panel1;
					case 2:
						return _panel2;
					case 3:
						return _panel3;
					default:
						throw new IndexOutOfRangeException("PicturePreviewStackPanelControlのインデックスは0 ～ 3の範囲です");
				}
			}
		}


		/// <summary>
		/// イメージインデックスへのリンク
		/// </summary>
		private readonly int[] _links = {-1, -1, -1, -1};

		/// <summary>
		/// 格納されてるイメージ数
		/// </summary>
		public int Count { get; private set; }

		#endregion

		/// <summary>
		/// テンプレートが更新された時に実行されます。
		/// </summary>
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			_panel0?.RemoveRemoveButtonClickEvent();
			_panel1?.RemoveRemoveButtonClickEvent();
			_panel2?.RemoveRemoveButtonClickEvent();
			_panel3?.RemoveRemoveButtonClickEvent();

			_panel0 = GetTemplateChild("Panel_0") as PicturePreviewPanelControl;
			_panel1 = GetTemplateChild("Panel_1") as PicturePreviewPanelControl;
			_panel2 = GetTemplateChild("Panel_2") as PicturePreviewPanelControl;
			_panel3 = GetTemplateChild("Panel_3") as PicturePreviewPanelControl;

			_panel0?.AddRemoveButtonClickEvent((sender, args) => OnRemoveClick(0));
			_panel1?.AddRemoveButtonClickEvent((sender, args) => OnRemoveClick(1));
			_panel2?.AddRemoveButtonClickEvent((sender, args) => OnRemoveClick(2));
			_panel3?.AddRemoveButtonClickEvent((sender, args) => OnRemoveClick(3));
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
			this[Count].ImageSource = image; //特に更新する必要が無いのでそのまま画像を入れる。
			Height += this[Count].Height + 5;
			++Count;
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
			double height = 0;
			for (var i = 0; i < _links.Length; i++) {
				this[i].ImageSource = _links[i] < 0 ? null : _images[_links[i]];
				if (_links[i] < 0) continue;
				height += this[i].Height + 5;
			}
			Height = height;
		}

		/// <summary>
		/// 全画像のクリア
		/// </summary>
		public void Clear() {
			for (var i = 0; i < _images.Length; i++) {
				_images[i] = null;
				_links[i] = -1;
				this[i].ImageSource = null;
			}
			Count = 0;
			Height = 0;
		}

		/// <summary>
		/// イメージの配列を取得します。
		/// イメージオブジェクトはコピーされません
		/// </summary>
		/// <returns></returns>
		public BitmapImage[] GetBitmapImages() {
			var returnImages = new BitmapImage[Count];
			for (var i = 0; i < Count; i++) returnImages[i] = _images[_links[i]];
			return returnImages;
		}
		
	}
}