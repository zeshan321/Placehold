using Placehold.Keyboard.Key;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Placehold.Keyboard.Hook
{
    public class KeyboardHookEvent : HandledEventArgs
    {
        public KeyboardState KeyboardState { get; private set; }
        public LowLevelKeyboardInputEvent KeyboardData { get; private set; }
        public KeyIn KeyIn { get; private set; }

        public KeyboardHookEvent(LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState)
        {
            this.KeyboardData = keyboardData;
            this.KeyboardState = keyboardState;
            this.KeyIn = new KeyIn((KeyCode) keyboardData.VirtualCode, keyboardData.Shifted, keyboardData.Capped);
        }
    }
}
