using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

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
            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = templateDir;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.EnableRaisingEvents = true;


            LoadTemplates();
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
            fileSystemWatcher.Changed -= OnChanged;
            fileSystemWatcher.Dispose();
        }
    }
}
