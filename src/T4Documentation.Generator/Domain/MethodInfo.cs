using System.Collections.Generic;
using T4Documentation.Generator.Abstract;

namespace T4Documentation.Generator.Domain
{
    public class MethodInfo : CommentItem
    {
        public List<ParameterInfo> ParameterInfos { get; set; } = new List<ParameterInfo>();
        public string Access { get; set; } = "";
        public string Name { get; set; } = "";
        public string ReturnType { get; set; } = "";

    }
}