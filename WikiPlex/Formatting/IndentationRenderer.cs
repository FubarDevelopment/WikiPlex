using System;

namespace WikiPlex.Formatting
{
    public class IndentationRenderer : IRenderer
    {
        public string Id
        {
            get { return "Indentation"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.IndentationBegin
                    || scopeName == ScopeName.IndentationEnd);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            switch (scopeName)
            {
                case ScopeName.IndentationBegin:
                    return "<blockquote>";
                case ScopeName.IndentationEnd:
                    return "</blockquote>";
                default:
                    return input;
            }
        }
    }
}