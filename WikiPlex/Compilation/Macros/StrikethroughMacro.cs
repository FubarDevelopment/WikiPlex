using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class StrikethroughMacro : IMacro
    {
        public string Id
        {
            get { return "Strikethrough"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?-s)((?<!-)--(?!-))(?>[^{[\n]*?)(?>(?:{{?|\[)(?>[^}\]\n]*)(?>(?:}}?|\])*)|.)*?(?>[^{[\n]*?)((?<!-)--(?!-))",
                                   new Dictionary<int, string>
                                       {
                                           { 1, ScopeName.StrikethroughBegin },
                                           { 2, ScopeName.StrikethroughEnd }
                                       })
                           };
            }
        }
    }
}