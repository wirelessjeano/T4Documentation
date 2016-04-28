using System;
using System.Data;

namespace T4Documentation.Generator.Abstract
{
    public abstract class CommentItem
    {
        public string Summary { get; set; } = "";
        public string Remarks { get; set; } = "";
        public string Example { get; set; } = "";
        public string ExampleCode { get; set; } = "";

        public string Test()
        {
            return "";
        }
    }
}
