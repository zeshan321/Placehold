using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Placehold.Keyboard.Hook
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LowLevelKeyboardInputEvent
    {
        public int VirtualCode;
        public int HardwareScanCode;
        public int Flags;
        public int TimeStamp;
        public IntPtr AdditionalInformation;
        public bool Shifted;
        public bool Capped;
    }
}
