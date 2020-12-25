using Placehold.Keyboard;
using Placehold.Keyboard.Hook;
using Placehold.Keyboard.Key;
using Placehold.Template.Data;
using Placehold.Template.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static Placehold.Keyboard.Hook.KeyboardHook;

namespace Placehold.Template
{
    public class TemplateManager : IDisposable
    {
        private readonly string templateDir;
        private readonly string fileseDir;
        private readonly string symbol;
        private readonly Dictionary<string, TemplateData> templates;
        private readonly Dictionary<string, string> files;
        private readonly FileSystemWatcher fileSystemWatcher;

        // Events
        private readonly FileEvent fileEvent;

        public TemplateManager()
        {
            this.templateDir = ConfigurationManager.AppSettings["templateDir"];
            this.fileseDir = ConfigurationManager.AppSettings["filesDir"];
            this.symbol = ConfigurationManager.AppSettings["symbol"];
            this.templates = new Dictionary<string, TemplateData>();
            this.files = new Dictionary<string, string>();

            // Watch for dir changes
            this.fileSystemWatcher = new FileSystemWatcher();
            this.fileSystemWatcher.Path = templateDir;
            this.fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            this.fileSystemWatcher.Filter = "*.*";
            this.fileSystemWatcher.Changed += OnChanged;
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.IncludeSubdirectories = true;

            // Load templates
            LoadTemplates();
            LoadFiles();

            this.fileEvent = new FileEvent();
            KeyboardManager.templateTriggerHook += this.fileEvent.OnCaptured;
        }

        public TemplateData? GetTemplateByName(string name)
        {
            KeyValuePair<string, TemplateData>? template = templates.FirstOrDefault(t => t.Key == name);
            return template?.Value;
        }

        public TemplateData? GetTemplateByCaptured(string captured)
        {
            foreach (var key in templates.Keys)
            {
                if (captured.EndsWith(key))
                {
                    return templates[key];
                }
            }

            return null;
        }

        public FileData? GetFile(string name)
        {
            var key = files.Keys.FirstOrDefault(t => t.EndsWith(name));
            if (key == null)
            {
                return null;
            }

            return new FileData(key, files[key]);
        }

        private void LoadTemplates()
        {
            templates.Clear();

            foreach (var filePath in Directory.GetFiles(templateDir, "*.txt", SearchOption.AllDirectories))
            {
                var fileName = $"{symbol}{Path.GetFileNameWithoutExtension(filePath)}{symbol}";
                templates.Add(fileName, new TemplateData(fileName, File.ReadAllText(filePath)));
            }
        }

        private void LoadFiles()
        {
            files.Clear();

            foreach (var filePath in Directory.GetFiles(fileseDir, "*.*", SearchOption.AllDirectories))
            {
                var fileName = $"{symbol}{Path.GetFileNameWithoutExtension(filePath)}{symbol}";
                files.Add(fileName, filePath);
            }
        }

        public void Paste()
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

            Input controlKeyUp = new Input();
            controlKeyUp.Type = 1;
            controlKeyUp.Data.Keyboard.Vk = (ushort)KeyCode.ControlKey;
            controlKeyUp.Data.Keyboard.Flags = 0x0002;
            controlKeyUp.Data.Keyboard.Scan = 0;
            keyList.Add(controlKeyUp);

            KeyboardHook.SendInput((uint)keyList.Count, keyList.ToArray(), Marshal.SizeOf(typeof(Input)));
        }

        public void Earse(int window, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                KeyboardHook.PostMessage(window, (uint)KeyboardState.KeyDown, (int)KeyCode.Back, 0);
                KeyboardHook.PostMessage(window, (uint)KeyboardState.KeyUp, (int)KeyCode.Back, 0);
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            LoadTemplates();
            LoadFiles();
        }

        public void Dispose()
        {
            fileSystemWatcher.Changed -= OnChanged;
            fileSystemWatcher.Dispose();
        }
    }
}
