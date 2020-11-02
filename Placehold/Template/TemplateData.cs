using Placehold.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Placehold.Template
{
    public class TemplateData
    {
        public TemplateData(string data)
        {
            this.Data = data;
            SearchForParams();
        }

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
