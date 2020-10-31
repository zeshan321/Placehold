using Placehold.Keyboard.Hook;
using Placehold.Keyboard.Key;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Placehold.Keyboard
{
    public class KeyboardManager : IDisposable
    {
        private readonly KeyboardHook keyboardHook;
        
        private const char symbol = '$'; // only start listening once this symbol is typed and stop once typed again
        private DateTimeOffset? capture; // when not null, typed values will be stored in captured array
        private TimeSpan timeout = new TimeSpan(0, 0, 30); // Cancel listening if final symbol key wasn't pressed within given timespan
        private List<string> capturedKeys = new List<string>();
        private const string input = "$nabz$";
        private char[] output = "yesterday on wallstreetbets one guy put 450K into SPY and another put in 800K into spy there about to be millionaires today while we sit here and code myadmin".ToCharArray();

        public KeyboardManager()
        {
            this.keyboardHook = new KeyboardHook();
            keyboardHook.KeyboardPressed += OnKeyPressed;
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
                if (capturedString.EndsWith(input))
                {
                    new Thread(() =>
                    {
                        Earse(input.Length);
                        Thread.Sleep(100);
                        WriteString();
                    }).Start();
                }


                if ((DateTimeOffset.UtcNow - capture.Value).TotalSeconds >= timeout.TotalSeconds || character == symbol)
                {
                    capture = null;
                    capturedKeys.Clear();
                }
            }
        }

        private void WriteString()
        {
            var window = GetCorrectWindow();

            foreach (var c in output)
            {
                KeyboardHook.PostMessage(window, (uint) KeyboardState.WmChar, (IntPtr)c, IntPtr.Zero);
            }
        }

        private void Earse(int amount)
        {
            var window = GetCorrectWindow();

            for (var i = 0; i < amount; i++)
            {
                KeyboardHook.PostMessage(window, (uint)KeyboardState.KeyDown, (int) KeyCode.Back, 0);
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
            keyboardHook.Dispose();
        }
    }
}
