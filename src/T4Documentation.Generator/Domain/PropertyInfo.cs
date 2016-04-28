using T4Documentation.Generator.Abstract;

namespace T4Documentation.Generator.Domain
{
    public class PropertyInfo : CommentItem
    {
        public string Access { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";

    }
}