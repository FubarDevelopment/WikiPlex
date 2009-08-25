using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class UnorderedListMacro : IListMacro
    {
        public string Id
        {
            get { return "UnorderedList"; }
        }

        public string ListStartScopeName
        {
            get { return ScopeName.UnorderedListBeginTag; }
        }

        public string ListEndScopeName
        {
            get { return ScopeName.UnorderedListEndTag; }
        }

        public char DepthChar
        {
            get { return '*'; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(^\*+\s)[^\r\n]+((?:\r\n)?)$",
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