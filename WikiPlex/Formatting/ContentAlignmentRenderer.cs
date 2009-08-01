using System;

namespace WikiPlex.Formatting
{
    public class ContentAlignmentRenderer : IRenderer
    {
        public string Id
        {
            get { return "ContentAlignment"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.AlignEnd
                    || scopeName == ScopeName.LeftAlign
                    || scopeName == ScopeName.RightAlign);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            switch (scopeName)
            {
                case ScopeName.AlignEnd:
                    return "</div><div style=\"clear:both;\"></div>";
                case ScopeName.LeftAlign:
                    return "<div style=\"text-align:left;float:left;\">";
                case ScopeName.RightAlign:
                    return "<div style=\"text-align:right;float:right;\">";
                default:
                    return input;
            }
        }
    }
}