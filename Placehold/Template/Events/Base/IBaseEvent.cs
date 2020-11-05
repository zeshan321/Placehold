using Placehold.Keyboard.Hook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Placehold.Template.Events.Base
{
    public interface IBaseEvent
    {
        void OnCaptured(object sender, TemplateTriggerHookEvent e);
    }
}
