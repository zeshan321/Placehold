using Placehold.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Placehold.Keyboard.Hook
{
    public class TemplateTriggerHookEvent : HandledEventArgs
    {
        public TemplateTriggerHookEvent(string templateKey, TemplateData templateValue, int windowId, string[] arguments)
        {
            TemplateKey = templateKey;
            TemplateValue = templateValue;
            WindowId = windowId;
            Arguments = arguments;
        }

        public string TemplateKey { get; set; }
        public TemplateData TemplateValue { get; set; }
        public int WindowId { get; set; }
        public string[] Arguments { get; set; }
    }
}
