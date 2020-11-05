using Placehold.Keyboard.Hook;

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
