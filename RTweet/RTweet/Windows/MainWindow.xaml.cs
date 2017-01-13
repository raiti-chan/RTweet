using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RTweet.Main.Twitter;
using static RTweet.Main.Twitter.TwitterSystem;

namespace RTweet.Windows{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
			//初期化
			if (!Directory.Exists(@"logs\")) Directory.CreateDirectory(@"logs\");
			var date = DateTime.Now;
			var sw = new StreamWriter(@"logs\" + date.ToString("yyyy-MM-dd-HH-mm-ss") + ".log") { AutoFlush = true };
			var tw = TextWriter.Synchronized(sw);
			var twtl = new TextWriterTraceListener(tw, "LogFile");
			Trace.Listeners.Add(twtl);

			if (!Directory.Exists(@"config\")) Directory.CreateDirectory(@"config\");
			

			Debug.WriteLine("Start!!");
			Debug.WriteLine("Date: " + date.ToString("yyyy-M-d dddd"));
			Debug.WriteLine("Time: " + date.ToString("HH:mm:ss tt zz"));
			//Twitterシステムの初期化
			Instance.Initialize();

			UserList.ItemsSource = Instance.UsetList;
		}
	}
}
