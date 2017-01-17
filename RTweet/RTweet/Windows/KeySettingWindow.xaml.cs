using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using RTweet.Main;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace RTweet.Windows
{
    /// <summary>
    /// KeySettingWindow.xaml の相互作用ロジック
    /// </summary>
    //とんでもないうんこーどがあるので、改善できるならどんどんいじってくださいね
    public partial class KeySettingWindow
    {
        public KeySettingWindow()
        {
            InitializeComponent();
        }
        //Memo Ctrl>Alt>Shift>Win
        private void RegisterPushedKey(TextBox sender, KeyEventArgs e)
        {
            var keys = sender.Text.Split('+');
            ModKey modKey = ModKey.None;
            Keys key;
            int modKeyCount = 0;
            int size = keys.Length;
            for (int i = 0; i < size; ++i)
            {
                keys[i] = keys[i].Trim();
            }
            if (keys.Contains("Ctrl"))
            {
                modKey += 0x0002;
                modKeyCount++;
            }
            if (keys.Contains("Alt"))
            {
                modKey += 0x0001;
                modKeyCount++;
            }
            if (keys.Contains("Shift"))
            {
                modKey += 0x0004;
                modKeyCount++;
            }
            if (keys.Contains("Window"))
            {
                modKey += 0x0008;
                modKeyCount++;
            }
            if (modKeyCount != size)
            {
                if (Enum.TryParse(keys[modKeyCount], out key))
                {
                    MainWindow.HotKeyses[sender.Name].ChangeHotkey(modKey, key);
                }
            }
        }

        public void KeySettingBox_KeyDown(object sender, KeyEventArgs e)
        {
            var text = "";
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                text += "Ctrl + ";
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                text += "Alt + ";
            }
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                text += "Shift + ";
            }
            if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin))
            {
                text += "Window + ";
            }
            switch (e.Key)
            {
                case Key.RightCtrl:
                case Key.LeftCtrl:
                case Key.RightAlt:
                case Key.LeftAlt:
                case Key.RightShift:
                case Key.LeftShift:
                case Key.RWin:
                case Key.LWin:break;
                default:
                    text += e.Key.ToString();break;
            }
            if (text.EndsWith(" + "))
            {
                text = text.Remove(text.LastIndexOf('+') - 1);
            }
            ((TextBox) sender).Text = text;
        }

        private void KeySettingBox_KeyUp(object sender, KeyEventArgs e)
        {
            RegisterPushedKey(sender as TextBox, e);
        }

        private void CancelInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
