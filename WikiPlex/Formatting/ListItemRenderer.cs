using System;

namespace WikiPlex.Formatting
{
    public class ListItemRenderer : IRenderer
    {
        public string Id
        {
            get { return "ListItemFormatting"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.OrderedListBeginTag
                    || scopeName == ScopeName.OrderedListEndTag
                    || scopeName == ScopeName.UnorderedListBeginTag
                    || scopeName == ScopeName.UnorderedListEndTag
                    || scopeName == ScopeName.ListItemBegin
                    || scopeName == ScopeName.ListItemEnd);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            switch (scopeName)
            {
                case ScopeName.OrderedListBeginTag:
                    return "<ol><li>";
                case ScopeName.OrderedListEndTag:
                    return "</li></ol>";
                case ScopeName.UnorderedListBeginTag:
                    return "<ul><li>";
                case ScopeName.UnorderedListEndTag:
                    return "</li></ul>";
                case ScopeName.ListItemBegin:
                    return "<li>";
                case ScopeName.ListItemEnd:
                    return "</li>";
                default:
                    return input;
            }
        }
    }
}