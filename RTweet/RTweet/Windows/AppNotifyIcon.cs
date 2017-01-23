using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using RTweet.Main;
using MessageBox = System.Windows.MessageBox;


namespace RTweet.Windows {

	/// <summary>
	/// タスクトレイ駐在アイコン。通知関連
	/// </summary>
	public partial class AppNotifyIcon : Component {
		
		/// <summary>
		/// 初期化
		/// </summary>
		public AppNotifyIcon() {
			InitializeComponent();

			Open.Click += OpenOnClick;
			Exit.Click += ExitOnClick;
			Setting.Click += SettingOnClick;
		}


		/// <summary>
		/// アプリケーションの通知を起こします。
		/// </summary>
		/// <param name="text">通知メッセージ</param>
		/// <param name="time">表示時間</param>
		/// <param name="title">通知タイトル</param>
		/// <param name="icon">通知メッセージアイコン</param>
		public void Notification(string text, int time, string title, ToolTipIcon icon) {
			notifyIcon.BalloonTipText = text;
			notifyIcon.BalloonTipTitle = title;
			notifyIcon.BalloonTipIcon = icon;
			notifyIcon.ShowBalloonTip(time);
		}




		//---------------------------------------------------------------------------------------------------ここから下イベント
		/// <summary>
		/// アイコンをダブルクリック
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event</param>
		private void notifyIcon_DoubleClick(object sender, EventArgs e) {
			App.AppInstance.AppWindow.Show();
		}

		/// <summary>
		/// 設定ボタン
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="eventArgs">event</param>
		public void SettingOnClick(object sender, EventArgs eventArgs) {
			Config.Instance.OpenSettingWindow();
		}

		/// <summary>
		/// アプリケーションを終了ボタン
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="eventArgs">event</param>
		private static void ExitOnClick(object sender, EventArgs eventArgs) {
			App.AppInstance.Shutdown();
		}

		/// <summary>
		/// ウィンドウを開くボタン
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">evevt</param>
		private static void OpenOnClick(object sender, EventArgs e) {
			App.AppInstance.AppWindow.Show();
		}
	}
}