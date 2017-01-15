using System;
using System.ComponentModel;


namespace RTweet.Windows {
	public partial class AppNotifyIcon : Component {
		public AppNotifyIcon() {
			InitializeComponent();

			Open.Click += OpenOnClick;
			Exit.Click += ExitOnClick;

		}



		public AppNotifyIcon(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private void notifyIcon_DoubleClick(object sender, EventArgs e) {
			App.AppInstance.AppWindow.Show();
		}

		private static void ExitOnClick(object sender, EventArgs eventArgs) {
			App.AppInstance.Shutdown();
		}

		private static void OpenOnClick(object sender, EventArgs e) {
			App.AppInstance.AppWindow.Show();
		}


	}
}
