using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Config = RTweet.Main.Config.MainConfig;
using Color = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace RTweet {
	// Appクラスの一部、
	// リソースの管理をします。
	public partial class App {

		/// <summary>
		/// ユーザー定義リソース
		/// </summary>
		private ResourceDictionary _userResourceDictionary;

		/// <summary>
		/// ユーザー定義リソース
		/// </summary>
		public ResourceDictionary UserResourceDictionary {
			get {
				if (_userResourceDictionary != null) return _userResourceDictionary;
				_userResourceDictionary = new ResourceDictionary();
				Current.Resources.MergedDictionaries.Add(_userResourceDictionary);
				return _userResourceDictionary;
			}
		}


		/// <summary>
		/// スタイル情報を更新します。
		/// </summary>
		public void StyleUpdate() {
			if (Config.Instance.IsStylingAtXaml) {
				var xamlUri = new Uri(Path.GetFullPath(Config.Instance.XamlFilePath));
				if (!File.Exists(xamlUri.AbsolutePath)) {
					MessageBox.Show("指定されたXAMLファイルが存在しません。\r\nデフォルトスタイルを適用します。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				UserResourceDictionary.Source = xamlUri;
				return;
			}

			var tweetDialogStyle = new Style();
			tweetDialogStyle.Setters.Add(new Setter(Border.BackgroundProperty,
				new SolidColorBrush(ColorCodeToColor(Config.Instance.Style.TweetDialogBackgroundColor))));
			Resources["TweetDialog.Backgrounnd"] = tweetDialogStyle;
		}


		/// <summary>
		/// カラーコードをカラーオブジェクトに変換します。
		/// </summary>
		/// <param name="colorCode">カラーコード</param>
		/// <returns>カラー</returns>
		public static Color ColorCodeToColor(string colorCode) {
			return DColorToColor(ColorTranslator.FromHtml(colorCode));
		}

		/// <summary>
		/// Colorを変換します。
		/// </summary>
		/// <param name="color">変換元カラー</param>
		/// <returns>カラー</returns>
		public static Color DColorToColor(DColor color) {
			return new Color() {A = color.A, R = color.R, G = color.G, B = color.B};
		}
	}
}