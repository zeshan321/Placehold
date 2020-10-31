using Placehold.Keyboard;
using Placehold.Resources;
using System;
using System.ComponentModel;
using System.Windows;
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

        public MainWindow()
        {
            // Init and display icon
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resource.favicon;
            notifyIcon.Visible = true;

            // Start keyboard manager
            keyboardManager = new KeyboardManager();

            // Init and hide component
            InitializeComponent();
            this.Hide();
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
