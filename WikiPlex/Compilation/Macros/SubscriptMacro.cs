using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class SubscriptMacro : IMacro
    {
        public string Id
        {
            get { return "Subscript"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?-s)((?<!,),,(?!,))(?>[^{\[\n]*?)(?>(?:{{?|\[)(?>[^}\]\n]*)(?>(?:}}?|\])*)|.)*?(?>[^{\[\n]*?)((?<!,),,(?!,))",
                                   new Dictionary<int, string>
                                       {
                                           { 1, ScopeName.SubscriptBegin },
                                           { 2, ScopeName.SubscriptEnd }
                                       })
                           };
            }
        }
    }
}