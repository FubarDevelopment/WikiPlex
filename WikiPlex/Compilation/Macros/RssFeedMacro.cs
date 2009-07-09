using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class RssFeedMacro : IMacro
    {
        public string Id
        {
            get { return "Rss Feed"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?i)(\{rss\:)([^\}]+)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.RssFeed},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}