using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class LinkMacro : IMacro
    {
        public string Id
        {
            get { return "Link"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?i)(\[url:mailto:)((?>[^]]+))(])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkAsMailto},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\[url:)((?>[^]|]+))(])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkNoText},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\[url:)((?>[^]]+))(])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkWithText},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\[file:)((?>(?:https?)://[^]]+))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkNoText},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\[file:)((?>[^]\|]+\|(?:https?)://[^]]+))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkWithText},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)({anchor:)((?>[^}]+))(})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.Anchor},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(\[#)((?>[^]]+))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.LinkToAnchor},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}