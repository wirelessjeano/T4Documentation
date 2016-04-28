using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4Documentation.Generator.Abstract;

namespace T4Documentation.Generator.Domain
{
    public class ProjectItemInfo : CommentItem
    {
        public ProjectItemInfo()
        {

        }

        public List<UsingStatement> UsingStatements { get; set; } = new List<UsingStatement>();
        public List<PropertyInfo> PropertyInfos { get; set; } = new List<PropertyInfo>();
        public List<MethodInfo> MethodInfos { get; set; } = new List<MethodInfo>();

        public string Name { get; set; } = "";
        public string Type { get; set; } = "Unknown";
        public string TypePlural { get; set; } = "Unknowns";
        public string Namespace { get; set; } = "";
        public string Description { get; set; } = "";
        public string Folder { get; set; } = "";

    }
}
