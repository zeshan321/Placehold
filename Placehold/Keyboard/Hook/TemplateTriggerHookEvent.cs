using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Placehold.Keyboard.Hook
{
    public class TemplateTriggerHookEvent : HandledEventArgs
    {
        public string TemplateKey { get; set; }
        public string TemplateValue { get; set; }
        public int WindowId { get; set; }

        public TemplateTriggerHookEvent(string templateKey, string templateValue, int windowId)
        {
            TemplateKey = templateKey;
            TemplateValue = templateValue;
            WindowId = windowId;
        }
    }
}
