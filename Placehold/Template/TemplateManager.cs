using Placehold.Keyboard;
using Placehold.Keyboard.Hook;
using Placehold.Keyboard.Key;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Placehold.Template
{
    public class TemplateManager : IDisposable
    {
        private readonly string templateDir;
        private readonly string symbol;
        private readonly Dictionary<string, string> templates;
        private readonly FileSystemWatcher fileSystemWatcher;

        public TemplateManager()
        {
            this.templateDir = ConfigurationManager.AppSettings["templateDir"];
            this.symbol = ConfigurationManager.AppSettings["symbol"];
            this.templates = new Dictionary<string, string>();

            // Watch for dir changes
            this.fileSystemWatcher = new FileSystemWatcher();
            this.fileSystemWatcher.Path = templateDir;
            this.fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            this.fileSystemWatcher.Filter = "*.*";
            this.fileSystemWatcher.Changed += OnChanged;
            this.fileSystemWatcher.EnableRaisingEvents = true;

            // Load templates and listen for when triggered
            LoadTemplates();
            KeyboardManager.templateTriggerHook += OnTemplateTriggered;
        }

        private void OnTemplateTriggered(object sender, TemplateTriggerHookEvent e)
        {
            string[] lines = e.TemplateValue.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                foreach (var letter in line)
                {
                    KeyboardHook.PostMessage(e.WindowId, (uint)KeyboardState.WmChar, (IntPtr)letter, IntPtr.Zero);
                }

                KeyboardHook.PostMessage(e.WindowId, (uint)KeyboardState.KeyDown, (IntPtr)KeyCode.LShiftKey, IntPtr.Zero);
                KeyboardHook.PostMessage(e.WindowId, (uint)KeyboardState.KeyDown, (IntPtr)KeyCode.Enter, IntPtr.Zero);
                KeyboardHook.PostMessage(e.WindowId, (uint)KeyboardState.KeyUp, (IntPtr)KeyCode.Enter, IntPtr.Zero);
                KeyboardHook.PostMessage(e.WindowId, (uint)KeyboardState.KeyUp, (IntPtr)KeyCode.LShiftKey, IntPtr.Zero);
                Thread.Sleep(100);
            }
        }

        public string? GetTemplateByName(string name)
        {
            KeyValuePair<string, string>? template = templates.FirstOrDefault(t => t.Key == name);
            return template?.Value;
        }

        public KeyValuePair<string, string>? GetTemplateByCaptured(string captured)
        {
            foreach (var key in templates.Keys)
            {
                if (captured.EndsWith(key))
                {
                    return new KeyValuePair<string, string>(key, templates.GetValueOrDefault(key));
                }
            }

            return null;
        }

        private void LoadTemplates()
        {
            templates.Clear();

            foreach (var filePath in Directory.GetFiles(templateDir, "*.txt", SearchOption.AllDirectories))
            {
                var fileName = $"{symbol}{Path.GetFileNameWithoutExtension(filePath)}{symbol}";
                templates.Add(fileName, File.ReadAllText(filePath));
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            LoadTemplates();
        }

        public void Dispose()
        {
            KeyboardManager.templateTriggerHook -= OnTemplateTriggered;
            fileSystemWatcher.Changed -= OnChanged;
            fileSystemWatcher.Dispose();
        }
    }
}
