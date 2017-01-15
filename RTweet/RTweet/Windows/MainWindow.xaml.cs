using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RTweet.Main;
using RTweet.Main.Twitter;
using RTweet.Windows.Dialog;
using static RTweet.Main.Twitter.TwitterSystem;

namespace RTweet.Windows{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
			//初期化
			if (!Directory.Exists(LogDirectryPath)) Directory.CreateDirectory(LogDirectryPath);
			var date = DateTime.Now;
			var sw = new StreamWriter(LogDirectryPath + date.ToString("yyyy-MM-dd-HH-mm-ss") + ".log") { AutoFlush = true };
			var tw = TextWriter.Synchronized(sw);
			var twtl = new TextWriterTraceListener(tw, "LogFile");
			Trace.Listeners.Add(twtl);

			if (!Directory.Exists(ConfigDirectryPath)) Directory.CreateDirectory(ConfigDirectryPath);
			if (!Directory.Exists(CashDirectoryPath)) Directory.CreateDirectory(CashDirectoryPath); 

			Debug.WriteLine("Start!!");
			Debug.WriteLine("Date: " + date.ToString("yyyy-M-d dddd"));
			Debug.WriteLine("Time: " + date.ToString("HH:mm:ss tt zz"));
			//Twitterシステムの初期化
			Instance.Initialize();

			UserList.ItemsSource = Instance.UsetList;
			UserList.Text = Instance.ActiveUser.ScreenName;
			var userIcon = new BitmapImage();
			userIcon.BeginInit();
			userIcon.UriSource = new Uri(Path.GetFullPath(Instance.ActiveUser.ProfileImgPath));
			userIcon.EndInit();
			UserImage.Source = userIcon;

			//ホットキーの設定
			var keyBinde = new HotKeyRegister(ModKey.AltCtlWin, Keys.T, this);
			keyBinde.HotKeyPressed += HotKeyPush;
		}

		/// <summary>
		/// ユーザー追加ボタン
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void AddUser_Click(object sender, System.Windows.RoutedEventArgs e) {
			Instance.AddUser();
		}
		
		/// <summary>
		/// コンボボックスの切り替えイベント
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void UserList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			var newUser = e.AddedItems[0];
			Instance.ChengeUser(newUser as UserToken);
			var userIcon = new BitmapImage();
			userIcon.BeginInit();
			userIcon.UriSource = new Uri(Path.GetFullPath(Instance.ActiveUser.ProfileImgPath));
			userIcon.EndInit();
			UserImage.Source = userIcon;
		}

		private readonly TweetDialog _tweetWindw = new TweetDialog();

		private void HotKeyPush(object sender) {
			_tweetWindw.Show();
			_tweetWindw.ShowInit();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			e.Cancel = true;
			Hide();
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
			if (e.Key == Key.Escape) Hide();
		}
	}
}
