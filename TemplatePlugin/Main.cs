using Placehold.Keyboard.Hook;
using Placehold.Plugin;
using Placehold.Template.Data;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace TemplatePlugin
{
    public class Main : IPlaceholdEvent
    {
        public void OnCaptured(object sender, TemplateTriggerHookEvent e)
        {
            if (e.Complete)
            {
                return;
            }

            var template = e.TemplateManager.GetTemplateByCaptured(e.CapturedString);
            if (template == null)
            {
                e.Complete = false;
                return;
            }

            e.EarseAmount += template.Name.Length;

            Thread.Sleep(300);
            e.TemplateManager.Earse(e.WindowId, e.EarseAmount);

            var data = template.Data;

            // Add arguments to template
            if (e.Arguments != null)
            {
                for (var i = 0; i < template.Arguments.Count; i++)
                {
                    var argument = template.Arguments.ElementAtOrDefault(i);
                    var value = e.Arguments.ElementAtOrDefault(i);

                    if (argument != null && value != null)
                    {
                        data = data.Replace(argument, value);
                        continue;
                    }

                    data = data.Replace(argument, "");
                }
            }

            IDataObject dataObject = new DataObject();
            var templateData = JsonSerializer.Deserialize<ClipboardData>(data);
            foreach (var key in templateData.Data.Keys)
            {
                var clipboardData = templateData.Data[key];

                dataObject.SetData(key, clipboardData);
            }

            Clipboard.Clear();
            Clipboard.SetDataObject(dataObject);
            e.TemplateManager.Paste();
            e.Complete = true;
        }
    }
}
