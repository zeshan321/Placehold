using Placehold.Keyboard.Hook;
using Placehold.Template.Data;
using Placehold.Template.Events.Base;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace Placehold.Template.Events
{
    public class TemplateEvent : BaseEvent
    {
        private readonly TemplateManager templateManager;

        public TemplateEvent(TemplateManager templateManager) : base(templateManager)
        {
            this.templateManager = templateManager;
        }

        public override void OnCaptured(object sender, TemplateTriggerHookEvent e)
        {
            if (e.Complete)
            {
                return;
            }

            var template = templateManager.GetTemplateByCaptured(e.CapturedString);
            if (template == null)
            {
                e.Complete = false;
                return;
            }

            e.EarseAmount += template.Name.Length;

            Thread.Sleep(300);
            templateManager.Earse(e.WindowId, e.EarseAmount);

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
            templateManager.Paste();

            base.OnCaptured(sender, e);
        }
    }
}
