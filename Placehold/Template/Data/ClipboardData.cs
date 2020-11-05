using System;
using System.Collections.Generic;

namespace Placehold.Template.Data
{
    [Serializable]
    public class ClipboardData
    {
        public Dictionary<string, object> Data { get; set; }

        public ClipboardData() { }
    }
}
