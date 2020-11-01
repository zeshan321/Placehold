using Placehold.Keyboard;
using Placehold.Keyboard.Hook;
using Placehold.Keyboard.Key;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;
using IDataObject = System.Windows.IDataObject;
using DataObject = System.Windows.DataObject;
using static Placehold.Keyboard.Hook.KeyboardHook;
using System.Runtime.InteropServices;

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
            IDataObject dataObject = new DataObject();
            var templateData = JsonSerializer.Deserialize<TemplateData>(e.TemplateValue);
            foreach (var key in templateData.Data.Keys)
            {
                var data = templateData.Data[key];

                dataObject.SetData(key, data);
            }

            Clipboard.SetDataObject(dataObject);
            Paste();
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

        private void Paste()
        {
            List<Input> keyList = new List<Input>();

            Input controlKeyDown = new Input();
            controlKeyDown.Type = 1;
            controlKeyDown.Data.Keyboard.Vk = (ushort)KeyCode.ControlKey;
            controlKeyDown.Data.Keyboard.Flags = 0;
            controlKeyDown.Data.Keyboard.Scan = 0;
            keyList.Add(controlKeyDown);

            Input vKeyDown = new Input();
            vKeyDown.Type = 1;
            vKeyDown.Data.Keyboard.Vk = (ushort)KeyCode.V;
            vKeyDown.Data.Keyboard.Flags = 0;
            vKeyDown.Data.Keyboard.Scan = 0;
            keyList.Add(vKeyDown);

            Input vKeyUp = new Input();
            vKeyUp.Type = 1;
            vKeyUp.Data.Keyboard.Vk = (ushort)KeyCode.V;
            vKeyUp.Data.Keyboard.Flags = 0x0002;
            vKeyUp.Data.Keyboard.Scan = 0;
            keyList.Add(vKeyUp);

            Input controlKeyUp = new Input();
            controlKeyUp.Type = 1;
            controlKeyUp.Data.Keyboard.Vk = (ushort)KeyCode.ControlKey;
            controlKeyUp.Data.Keyboard.Flags = 0x0002;
            controlKeyUp.Data.Keyboard.Scan = 0;
            keyList.Add(controlKeyUp);

            KeyboardHook.SendInput((uint)keyList.Count, keyList.ToArray(), Marshal.SizeOf(typeof(Input)));
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
