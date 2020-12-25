using Placehold.Keyboard;
using Placehold.Plugin;
using Placehold.Resources;
using Placehold.Template;
using Placehold.Template.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;

namespace Placehold
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private TemplateManager templateManager;
        private KeyboardManager keyboardManager;
        private readonly NotifyIcon notifyIcon;
        private readonly ContextMenuStrip contextMenuStrip;
        private readonly string[] supportedDataFormats;
        private readonly string path;

        public MainWindow()
        {
            // Create template directory if not found
            path = ConfigurationManager.AppSettings["templateDir"];
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(ConfigurationManager.AppSettings["filesDir"]);
            Directory.CreateDirectory(ConfigurationManager.AppSettings["pluginDir"]);

            supportedDataFormats = new string[] { DataFormats.Html, DataFormats.Text, /*DataFormats.UnicodeText, DataFormats.CommaSeparatedValue, DataFormats.OemText, DataFormats.Serializable*/ };

            // Init tray
            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Restart", null, this.Restart);
            contextMenuStrip.Items.Add("Exit", null, this.MenuExit);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Resource.favicon;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.DoubleClick += OnDoubleClick;

            // Start keyboard manager
            templateManager = new TemplateManager();
            keyboardManager = new KeyboardManager(templateManager);
            new PluginManager().InitPlugins();

            // Init and hide component
            InitializeComponent();
            this.Hide();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            ClipboardData clipboardData = new ClipboardData();
            clipboardData.Data = new Dictionary<string, object>();
            foreach (var format in supportedDataFormats)
            {
                if (Clipboard.ContainsData(format))
                {
                    clipboardData.Data.Add(format, Clipboard.GetData(format));
                }
            }

            var inputDialog = new InputDialog("Enter placeholder name:");
            if (inputDialog.ShowDialog() == true)
            {
                var serialize = JsonSerializer.Serialize(clipboardData, new JsonSerializerOptions() { MaxDepth = 100 });
                var name = inputDialog.Answer;
                var path = Path.Combine(ConfigurationManager.AppSettings["templateDir"], $"{name}.txt");

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, serialize);
                }
            }
        }

        private void MenuExit(object sender, EventArgs e)
        {
            Dispose();
            this.Close();
        }

        private void Restart(object sender, EventArgs e)
        {
            Dispose();
            templateManager = new TemplateManager();
            keyboardManager = new KeyboardManager(templateManager);
            new PluginManager().InitPlugins();
        }

        public void Dispose()
        {
            keyboardManager.Dispose();
            templateManager.Dispose();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.Hide();

            base.OnStateChanged(e);
        }
    }
}
