using System;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class TextFormattingRenderer : IRenderer
    {
        public string Id
        {
            get { return "TextFormatting"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.BoldBegin
                    || scopeName == ScopeName.BoldEnd
                    || scopeName == ScopeName.ItalicsBegin
                    || scopeName == ScopeName.ItalicsEnd
                    || scopeName == ScopeName.UnderlineBegin
                    || scopeName == ScopeName.UnderlineEnd
                    || scopeName == ScopeName.HeadingOneBegin
                    || scopeName == ScopeName.HeadingOneEnd
                    || scopeName == ScopeName.HeadingTwoBegin
                    || scopeName == ScopeName.HeadingTwoEnd
                    || scopeName == ScopeName.HeadingThreeBegin
                    || scopeName == ScopeName.HeadingThreeEnd
                    || scopeName == ScopeName.HeadingFourBegin
                    || scopeName == ScopeName.HeadingFourEnd
                    || scopeName == ScopeName.HeadingFiveBegin
                    || scopeName == ScopeName.HeadingFiveEnd
                    || scopeName == ScopeName.HeadingSixBegin
                    || scopeName == ScopeName.HeadingSixEnd
                    || scopeName == ScopeName.StrikethroughBegin
                    || scopeName == ScopeName.StrikethroughEnd
                    || scopeName == ScopeName.SubscriptBegin
                    || scopeName == ScopeName.SubscriptEnd
                    || scopeName == ScopeName.SuperscriptBegin
                    || scopeName == ScopeName.SuperscriptEnd
                    || scopeName == ScopeName.HorizontalRule
                    || scopeName == ScopeName.EscapedMarkup
                    || scopeName == ScopeName.Remove);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            switch (scopeName)
            {
                case ScopeName.BoldBegin:
                    return "<b>";
                case ScopeName.BoldEnd:
                    return "</b>";
                case ScopeName.ItalicsBegin:
                    return "<i>";
                case ScopeName.ItalicsEnd:
                    return "</i>";
                case ScopeName.UnderlineBegin:
                    return "<u>";
                case ScopeName.UnderlineEnd:
                    return "</u>";
                case ScopeName.HeadingOneBegin:
                    return "<h1>";
                case ScopeName.HeadingOneEnd:
                    return "</h1>\r";
                case ScopeName.HeadingTwoBegin:
                    return "<h2>";
                case ScopeName.HeadingTwoEnd:
                    return "</h2>\r";
                case ScopeName.HeadingThreeBegin:
                    return "<h3>";
                case ScopeName.HeadingThreeEnd:
                    return "</h3>\r";
                case ScopeName.HeadingFourBegin:
                    return "<h4>";
                case ScopeName.HeadingFourEnd:
                    return "</h4>\r";
                case ScopeName.HeadingFiveBegin:
                    return "<h5>";
                case ScopeName.HeadingFiveEnd:
                    return "</h5>\r";
                case ScopeName.HeadingSixBegin:
                    return "<h6>";
                case ScopeName.HeadingSixEnd:
                    return "</h6>\r";
                case ScopeName.StrikethroughBegin:
                    return "<del>";
                case ScopeName.StrikethroughEnd:
                    return "</del>";
                case ScopeName.SubscriptBegin:
                    return "<sub>";
                case ScopeName.SubscriptEnd:
                    return "</sub>";
                case ScopeName.SuperscriptBegin:
                    return "<sup>";
                case ScopeName.SuperscriptEnd:
                    return "</sup>";
                case ScopeName.HorizontalRule:
                    return "<hr />";
                case ScopeName.Remove:
                    return string.Empty;
                case ScopeName.EscapedMarkup:
                    return htmlEncode(input);
                default:
                    return input;
            }
        }
    }
}