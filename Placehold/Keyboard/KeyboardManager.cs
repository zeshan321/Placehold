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
using Placehold.Extensions;
using System.Linq;

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
            if (e.KeyboardState != KeyboardState.KeyDown && e.KeyboardState != KeyboardState.SysKeyDown)
                return;

            var value = e.KeyIn.ToString();
            if (!char.TryParse(value, out char character))
            {
                if (value.Contains("backspace") && capturedKeys.Any())
                {
                    capturedKeys.RemoveAt(capturedKeys.Count - 1);
                }
                return;
            }

            if (!capture.HasValue)
            {
                if (character == symbol)
                {
                    capture = DateTimeOffset.UtcNow;
                    capturedKeys.Clear();
                    capturedKeys.Add(value);
                }
            }
            else
            {
                capturedKeys.Add(value);
                var capturedString = string.Join("", capturedKeys);
                Debug.WriteLine(capturedString);
                string? argumentString = null;
                var eraseAmount = 0;

                if (capturedString.Contains('(') && capturedString.Contains(')'))
                {
                    argumentString = capturedString.ExtractFromString("(", ")").FirstOrDefault() ?? "";
                    capturedString = capturedString.Replace($"({argumentString})", "");
                    eraseAmount += (2 + (argumentString?.Length ?? 0));
                }

                var template = templateManager.GetTemplateByCaptured(capturedString);
                if (template.HasValue)
                {
                    eraseAmount += template.Value.Key.Length;

                    var window = GetCorrectWindow();

                    Thread.Sleep(300);
                    Earse(eraseAmount);

                    TemplateTriggerHookEvent templateTriggerHookEvent = new TemplateTriggerHookEvent(template.Value.Key, template.Value.Value, window, argumentString?.Split("|"));
                    templateTriggerHook?.Invoke(this, templateTriggerHookEvent);

                    capture = null;
                    capturedKeys.Clear();
                    return;
                }

                if (capture != null && (DateTimeOffset.UtcNow - capture.Value).TotalSeconds >= timeout.TotalSeconds)
                {
                    capture = null;
                    capturedKeys.Clear();
                }

                if (capturedString.Count(c => c == symbol) >= 2)
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
