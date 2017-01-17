using System.Windows;
using System.Windows.Forms;
using RTweet.Windows;
using Application = System.Windows.Application;

namespace RTweet {
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application {
		public static App AppInstance { get; private set; }

		public AppNotifyIcon NotifyIcon { get; private set; }

		public MainWindow AppWindow { get; private set; }

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);
			AppInstance = this;
			ShutdownMode = ShutdownMode.OnExplicitShutdown;
			NotifyIcon = new AppNotifyIcon();
			AppWindow = new MainWindow();
			AppWindow.Show();
		}

		protected override void OnExit(ExitEventArgs e) {
			base.OnExit(e);
			NotifyIcon.Dispose();
		}

		/// <summary>
		/// アプリケーションの通知を起こします。
		/// </summary>
		/// <param name="text">通知メッセージ</param>
		/// <param name="time">表示時間 デフォルト:500</param>
		/// <param name="title">通知タイトル デフォルト:RTweet</param>
		/// <param name="icon">通知メッセージアイコン デフォルト:None</param>
		public static void Notification(string text, int time = 500, string title = "RTweet",
			ToolTipIcon icon = ToolTipIcon.None) {
			AppInstance.NotifyIcon.Notification(text, time, title, icon);
		}

	}
}