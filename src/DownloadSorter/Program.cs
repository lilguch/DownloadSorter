using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadsSorter
{
    internal static class Program
    {
        public static NotifyIcon _notifyIcon;
        public static DownloadOrganizer _organizer;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetupTrayIcon();

            if (IsAutoStart())
            {
                _organizer = new DownloadOrganizer(_notifyIcon);

                _organizer.StartWatching();
                Application.Run();
            }
            else
            {
                Application.Run(new MainForm());
            }            
        }

        public static bool IsAutoStart()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Any(arg => arg.Equals("/autostart", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        private static void SetupTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                Text = "Download Organizer",
                Visible = true
            };
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Открыть", null, OnOpenClicked);
            contextMenu.Items.Add("Закрыть", null, OnCloseClicked);
            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += OnOpenClicked;
        }

        private static void OnOpenClicked(object sender, EventArgs e)
        {
            var form = new MainForm();
            form.Show();
        }

        private static void OnCloseClicked(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false; 
            Application.Exit();
        }
    }
}
