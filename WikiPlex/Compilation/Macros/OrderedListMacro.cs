using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WikiPlex.Compilation.Macros
{
    public class OrderedListMacro : IListMacro
    {
        private static readonly Regex LevelRegex = new Regex(@"\#", RegexOptions.Compiled);

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

        public int DetermineLevel(string content)
        {
            return LevelRegex.Matches(content).Count;
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