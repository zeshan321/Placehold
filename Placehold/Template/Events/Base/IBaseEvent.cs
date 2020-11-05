using Placehold.Keyboard.Hook;

namespace Placehold.Template.Events.Base
{
    public interface IBaseEvent
    {
        void OnCaptured(object sender, TemplateTriggerHookEvent e);
    }
}
