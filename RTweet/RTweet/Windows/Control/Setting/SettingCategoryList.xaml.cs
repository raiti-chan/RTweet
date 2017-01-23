using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RTweet.Main;

namespace RTweet.Windows.Control.Setting {
	/// <summary>
	/// SettingCategoryList.xaml の相互作用ロジック
	/// </summary>
	public partial class SettingCategoryList {
		public SettingCategoryList() {
			InitializeComponent();
			AddCategory("aaa", GetShell32Icon.GetIcon(56));
			AddCategory("bbb");
		}

		public void AddCategory(string categoryName, ImageSource image = null) {
			object content;
			if (image != null) {
				var imageContent = new StackPanel {
					Orientation = Orientation.Horizontal,
					Margin = new Thickness(4, 2, 4, 2)
				};
				imageContent.Children.Add(new Image {
					Source = image,
					VerticalAlignment = VerticalAlignment.Center
				});

				imageContent.Children.Add(new Label {
					Content = categoryName,
					VerticalAlignment = VerticalAlignment.Center,
					Margin = new Thickness(10, 0, 0, 0)
				});
				content = imageContent;
			}
			else content = categoryName;

			Panel.Children.Add(new RadioButton {
				Content = content
			});
		}

		public void AddCategory(string categoryName, Uri imageUri) {
			AddCategory(categoryName, imageUri != null ? new BitmapImage(imageUri) : null);
		}
	}
}