using Placehold.Keyboard;
using Placehold.Resources;
using Placehold.Template;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Placehold
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly KeyboardManager keyboardManager;
        private readonly NotifyIcon notifyIcon;
        private readonly ContextMenuStrip contextMenuStrip;

        public MainWindow()
        {
            // Init tray
            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Exit", null, this.MenuExit);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resource.favicon;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            // Start keyboard manager
            keyboardManager = new KeyboardManager();

            // Init and hide component
            InitializeComponent();
            this.Hide();
        }

        private void MenuExit(object sender, EventArgs e)
        {
            Dispose();
            Environment.Exit(0);
        }

        public void Dispose()
        {
            keyboardManager.Dispose();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.Hide();

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();

            base.OnClosing(e);
        }
    }
}
