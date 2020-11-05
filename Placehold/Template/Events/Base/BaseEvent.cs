using Placehold.Keyboard.Hook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placehold.Template.Events.Base
{
    public class BaseEvent : IBaseEvent
    {
        private readonly TemplateManager templateManager;

        public BaseEvent(TemplateManager templateManager) 
        {
            this.templateManager = templateManager;
        }

        public virtual void OnCaptured(object sender, TemplateTriggerHookEvent e)
        {
            e.Complete = true;
        }
    }
}
