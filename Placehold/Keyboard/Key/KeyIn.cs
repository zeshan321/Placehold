using System.Collections.Generic;

namespace Placehold.Keyboard.Key
{
    public class KeyIn
    {
        public KeyCode KeyCode { get; set; }
        public bool Capped { get; set; }
        public bool Shifted { get; set; }

        internal KeyIn(KeyCode keyCode, bool shifted, bool capped)
        {
            this.KeyCode = keyCode;
            this.Shifted = shifted;
            this.Capped = capped;
        }

        public override string ToString()
        {
            if (!KeyCodes.TryGetValue(KeyCode, out string character))
            {
                character = string.Empty;
            }

            if (IsAlphabeticKey())
            {
                if (Shifted == Capped)
                {
                    return character;
                }
                else if (Shifted || Capped)
                {
                    return character.ToUpper();
                }
                else
                {
                    return character.ToUpper();
                }
            }
            else if (Shifted)
            {
                if (ShiftedKeyCodes.TryGetValue(KeyCode, out string shiftedCharacter))
                {
                    return shiftedCharacter;
                }

                return character;
            }

            return character;
        }

        private bool IsAlphabeticKey()
        {
            return (int)KeyCode > 64 && (int)KeyCode < 91;
        }

        Dictionary<KeyCode, string> KeyCodes = new Dictionary<KeyCode, string>
        {
            { KeyCode.F1, "<F1>" },
            { KeyCode.F2, "<F2>" },
            { KeyCode.F3, "<F3>" },
            { KeyCode.F4, "<F4>" },
            { KeyCode.F5, "<F5>" },
            { KeyCode.F6, "<F6>" },
            { KeyCode.F7, "<F7>" },
            { KeyCode.F8, "<F8>" },
            { KeyCode.F9, "<F9>" },
            { KeyCode.F10, "<F10>" },
            { KeyCode.F11, "<F11>" },
            { KeyCode.F12, "<F12>" },
            { KeyCode.Snapshot, "<print screen>" },
            { KeyCode.Scroll, "<scroll>" },
            { KeyCode.Pause, "<pause>" },
            { KeyCode.Insert, "<insert>" },
            { KeyCode.Home, "<home>" },
            { KeyCode.Delete, "<delete>" },
            { KeyCode.End, "<end>" },
            { KeyCode.Prior, "<page up>" },
            { KeyCode.Next, "<page down>" },
            { KeyCode.Escape, "<esc>" },
            { KeyCode.NumLock, "<numlock>" },
            { KeyCode.Tab, "<tab>" },
            { KeyCode.Back, "<backspace>" },
            { KeyCode.Return, "<enter>" },
            { KeyCode.Space, " " },
            { KeyCode.Left, "<left>" },
            { KeyCode.Up, "<up>" },
            { KeyCode.Right, "<right>" },
            { KeyCode.Down, "<down>" },
            { KeyCode.LMenu, "<alt>" },
            { KeyCode.RMenu, "<alt>" },
            { KeyCode.LWin, "<win>" },
            { KeyCode.RWin, "<win>" },
            { KeyCode.Capital, "<capsLock>" },
            { KeyCode.LControlKey, "<ctrl>" },
            { KeyCode.RControlKey, "<ctrl>" },
            { KeyCode.LShiftKey, "<shift>" },
            { KeyCode.RShiftKey, "<shift>" },
            { KeyCode.VolumeDown, "<volumeDown>" },
            { KeyCode.VolumeUp, "<volumeUp>" },
            { KeyCode.VolumeMute, "<volumeMute>" },
            { KeyCode.Multiply, "*" },
            { KeyCode.Add, "+" },
            { KeyCode.Separator, "|" },
            { KeyCode.Subtract, "-" },
            { KeyCode.Divide, "/" },
            { KeyCode.Oemplus, "=" },
            { KeyCode.Oemcomma, "," },
            { KeyCode.OemMinus, "-" },
            { KeyCode.OemPeriod, "." },
            { KeyCode.NumPadDot, "." },
            { KeyCode.Decimal, "." },
            { KeyCode.Oem1, " }," },
            { KeyCode.Oem2, "/" },
            { KeyCode.Oem3, "`" },
            { KeyCode.Oem4, "[" },
            { KeyCode.Oem5, "\\" },
            { KeyCode.Oem6, "]" },
            { KeyCode.Oem7, "'" },
            { KeyCode.NumPad0, "0" },
            { KeyCode.NumPad1, "1" },
            { KeyCode.NumPad2, "2" },
            { KeyCode.NumPad3, "3" },
            { KeyCode.NumPad4, "4" },
            { KeyCode.NumPad5, "5" },
            { KeyCode.NumPad6, "6" },
            { KeyCode.NumPad7, "7" },
            { KeyCode.NumPad8, "8" },
            { KeyCode.NumPad9, "9" },
            { KeyCode.Q, "q" },
            { KeyCode.W, "w" },
            { KeyCode.E, "e" },
            { KeyCode.R, "r" },
            { KeyCode.T, "t" },
            { KeyCode.Y, "y" },
            { KeyCode.U, "u" },
            { KeyCode.I, "i" },
            { KeyCode.O, "o" },
            { KeyCode.P, "p" },
            { KeyCode.A, "a" },
            { KeyCode.S, "s" },
            { KeyCode.D, "d" },
            { KeyCode.F, "f" },
            { KeyCode.G, "g" },
            { KeyCode.H, "h" },
            { KeyCode.J, "j" },
            { KeyCode.K, "k" },
            { KeyCode.L, "l" },
            { KeyCode.Z, "z" },
            { KeyCode.X, "x" },
            { KeyCode.C, "c" },
            { KeyCode.V, "v" },
            { KeyCode.B, "b" },
            { KeyCode.N, "n" },
            { KeyCode.M, "m" },
            { KeyCode.D0, "0" },
            { KeyCode.D1, "1" },
            { KeyCode.D2, "2" },
            { KeyCode.D3, "3" },
            { KeyCode.D4, "4" },
            { KeyCode.D5, "5" },
            { KeyCode.D6, "6" },
            { KeyCode.D7, "7" },
            { KeyCode.D8, "8" },
            { KeyCode.D9, "9" }
        };

        Dictionary<KeyCode, string> ShiftedKeyCodes = new Dictionary<KeyCode, string>
        {
            { KeyCode.D1, "!" },
            { KeyCode.D2, "@" },
            { KeyCode.D3, "#" },
            { KeyCode.D4, "$" },
            { KeyCode.D5, "%" },
            { KeyCode.D6, "^" },
            { KeyCode.D7, "&" },
            { KeyCode.D8, "*" },
            { KeyCode.D9, "(" },
            { KeyCode.D0, ")" },
            { KeyCode.Oemcomma, "<" },
            { KeyCode.OemMinus, "_" },
            { KeyCode.OemPeriod, ">" },
            { KeyCode.Oemplus, "+" },
            { KeyCode.LatinKeyboardBar, "?" },
            { KeyCode.Oem1, ":" },
            { KeyCode.Oem2, "?" },
            { KeyCode.Oem3, "~" },
            { KeyCode.Oem4, "{" },
            { KeyCode.Oem5, "|" },
            { KeyCode.Oem6, "}" },
            { KeyCode.Oem7, "\"" }
        };
    }
}