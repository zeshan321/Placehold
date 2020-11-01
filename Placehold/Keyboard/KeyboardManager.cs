using Placehold.Keyboard.Hook;
using Placehold.Keyboard.Key;
using Placehold.Template;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Placehold.Keyboard
{
    public class KeyboardManager : IDisposable
    {
        private readonly TemplateManager templateManager;
        private readonly KeyboardHook keyboardHook;
        public static event EventHandler<TemplateTriggerHookEvent> templateTriggerHook;

        private readonly char symbol; // only start listening once this symbol is typed and stop once typed again
        private readonly TimeSpan timeout; // Cancel listening if final symbol key wasn't pressed within given timespan
        private DateTimeOffset? capture; // when not null, typed values will be stored in captured array
        private List<string> capturedKeys = new List<string>();

        public KeyboardManager(TemplateManager templateManager)
        {
            this.symbol = char.Parse(ConfigurationManager.AppSettings["symbol"]);
            this.timeout = TimeSpan.Parse(ConfigurationManager.AppSettings["timeout"]);


            this.templateManager = templateManager;
            this.keyboardHook = new KeyboardHook();
            this.keyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyboardHookEvent e)
        {
            if (e.KeyboardState != KeyboardState.KeyDown)
                return;

            var value = e.KeyIn.ToString();
            if (!char.TryParse(value, out char character))
            {
                return;
            }

            if (!capture.HasValue)
            {
                if (character == symbol)
                {
                    capture = DateTimeOffset.UtcNow;
                    capturedKeys.Add(value);
                }
            }
            else
            {
                capturedKeys.Add(value);
                var capturedString = string.Join("", capturedKeys);
                var template = templateManager.GetTemplateByCaptured(capturedString);
                if (template.HasValue)
                {
                    var window = GetCorrectWindow();

                    Thread.Sleep(300);
                    Earse(template.Value.Key.Length);

                    TemplateTriggerHookEvent templateTriggerHookEvent = new TemplateTriggerHookEvent(template.Value.Key, template.Value.Value, window);
                    templateTriggerHook?.Invoke(this, templateTriggerHookEvent);
                }


                if ((DateTimeOffset.UtcNow - capture.Value).TotalSeconds >= timeout.TotalSeconds || character == symbol)
                {
                    capture = null;
                    capturedKeys.Clear();
                }
            }
        }

        private void Earse(int amount)
        {
            var window = GetCorrectWindow();

            for (var i = 0; i < amount; i++)
            {
                KeyboardHook.PostMessage(window, (uint)KeyboardState.KeyDown, (int)KeyCode.Back, 0);
                KeyboardHook.PostMessage(window, (uint)KeyboardState.KeyUp, (int)KeyCode.Back, 0);
            }
        }

        private int GetCorrectWindow()
        {
            KeyboardHook.GetWindowThreadProcessId(KeyboardHook.GetForegroundWindow(), out uint processId);

            var process = Process.GetProcessById((int)processId);
            switch (process.ProcessName)
            {
                case "notepad":
                    return KeyboardHook.FindWindowEx(KeyboardHook.GetForegroundWindow(), IntPtr.Zero, "edit", null).ToInt32();
                default:
                    return KeyboardHook.GetForegroundWindow().ToInt32();
            }
        }

        public void Dispose()
        {
            keyboardHook.KeyboardPressed -= OnKeyPressed;
            keyboardHook.Dispose();
        }
    }
}
