using System.Windows;
using RTweet.Windows;

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

	}
}
