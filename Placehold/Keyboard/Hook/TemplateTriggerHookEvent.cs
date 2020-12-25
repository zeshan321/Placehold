using Placehold.Template;
using System.ComponentModel;

namespace Placehold.Keyboard.Hook
{
    public class TemplateTriggerHookEvent : HandledEventArgs
    {
        public TemplateTriggerHookEvent(TemplateManager templateManager, string capturedString, int windowId, string[] arguments, int earseAmount)
        {
            TemplateManager = templateManager;
            CapturedString = capturedString;
            WindowId = windowId;
            Arguments = arguments;
            EarseAmount = earseAmount;
        }

        public TemplateManager TemplateManager { get;  }
        public string CapturedString { get; set; }
        public int WindowId { get; set; }
        public string[] Arguments { get; set; }
        public int EarseAmount { get; set; }
        public bool Complete { get; set; }
    }
}
