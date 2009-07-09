using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class SuperscriptMacro : IMacro
    {
        public string Id
        {
            get { return "Superscript"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?-s)((?<!\^)\^\^(?!\^))(?>[^{\[\n]*?)(?>(?:{{?|\[)(?>[^}\]\n]*)(?>(?:}}?|\])*)|.)*?(?>[^{\[\n]*?)((?<!\^)\^\^(?!\^))",
                                   new Dictionary<int, string>
                                       {
                                           { 1, ScopeName.SuperscriptBegin },
                                           { 2, ScopeName.SuperscriptEnd }
                                       })
                           };
            }
        }
    }
}