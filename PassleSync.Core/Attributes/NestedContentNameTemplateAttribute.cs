using System;

namespace PassleSync.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NestedContentNameTemplateAttribute : Attribute
    {
        public string Template;

        public NestedContentNameTemplateAttribute(string template)
        {
            Template = template;
        }
    }
}
