using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class SyndicatedFeedMacro : IMacro
    {
        public string Id
        {
            get { return "Syndicated Feed"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?i)(\{(?:rss|atom|feed)\:)([^\}]+)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.SyndicatedFeed},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}