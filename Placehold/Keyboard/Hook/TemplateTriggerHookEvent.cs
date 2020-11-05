using Placehold.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Placehold.Keyboard.Hook
{
    public class TemplateTriggerHookEvent : HandledEventArgs
    {
        public TemplateTriggerHookEvent(string capturedString, int windowId, string[] arguments, int earseAmount)
        {
            CapturedString = capturedString;
            WindowId = windowId;
            Arguments = arguments;
            EarseAmount = earseAmount;
        }

        public string CapturedString { get; set; }
        public int WindowId { get; set; }
        public string[] Arguments { get; set; }
        public int EarseAmount { get; set; }
        public bool Complete { get; set; }
    }
}
