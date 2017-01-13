using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RTweet.Main.Twitter.TwitterSystem;

namespace RTweet.Windows{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			//初期化
			if (!System.IO.Directory.Exists("logs/")) System.IO.Directory.CreateDirectory("logs/");
			var date = DateTime.Now;
			var sw = new StreamWriter("logs/" + date.ToString("yyyy-MM-dd-HH-mm-ss") + ".log") { AutoFlush = true };
			var tw = TextWriter.Synchronized(sw);
			var twtl = new TextWriterTraceListener(tw, "LogFile");
			Trace.Listeners.Add(twtl);

			Debug.WriteLine("Start!!");
			Debug.WriteLine("Date: " + date.ToString("yyyy-M-d dddd"));
			Debug.WriteLine("Time: " + date.ToString("HH:mm:ss tt zz"));
			//Twitterシステムの初期化
			Instance.Initialize();
		}
	}
}
