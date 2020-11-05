using Placehold.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Placehold.Template.Data
{
    public class TemplateData
    {
        public TemplateData(string name, string data)
        {
            this.Name = name;
            this.Data = data;
            SearchForParams();
        }

        public string Name { get; set; }
        public string Data { get; set; }
        public List<string> Arguments { get; set; }

        private void SearchForParams()
        {
            this.Arguments = new List<string>();

            var placeholders = Data.ExtractFromString();
            foreach (var placeholder in placeholders)
            {
                var name = $"%{placeholder}%";
                if (!Arguments.Contains(name))
                {
                    Arguments.Add(name);
                }
            }
        }
    }
}
