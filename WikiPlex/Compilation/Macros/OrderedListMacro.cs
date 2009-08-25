using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class OrderedListMacro : IListMacro
    {
        public string Id
        {
            get { return "OrderedList"; }
        }

        public string ListStartScopeName
        {
            get { return ScopeName.OrderedListBeginTag; }
        }

        public string ListEndScopeName
        {
            get { return ScopeName.OrderedListEndTag; }
        }

        public char DepthChar
        {
            get { return '#'; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(^\#+\s)[^\r\n]+((?:\r\n)?)$",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.ListItemBegin},
                                           {2, ScopeName.ListItemEnd}
                                       })
                           };
            }
        }
    }
}