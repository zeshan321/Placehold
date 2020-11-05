using System;
using System.Collections.Generic;
using System.Text;

namespace Placehold.Template.Data
{
    [Serializable]
    public class FileData
    {
        public FileData(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
