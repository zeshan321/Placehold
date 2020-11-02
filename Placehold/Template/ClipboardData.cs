using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Placehold.Template
{
    [Serializable]
    public class ClipboardData
    {
        public Dictionary<string, object> Data { get; set; }

        public ClipboardData() { }
    }
}
