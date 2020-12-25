using Placehold.Keyboard.Hook;

namespace Placehold.Plugin
{
    public interface IPlaceholdEvent
    {
        void OnCaptured(object sender, TemplateTriggerHookEvent e);
    }
}
