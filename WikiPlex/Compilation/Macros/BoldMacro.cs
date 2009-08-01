using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class BoldMacro : IMacro
    {
        public string Id
        {
            get { return "Bold"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?-s)(?:((?!\*+\s)\*)(?>[^{[\*\n]*)(?>(?:{{?|\[)(?>[^}\]\n]*)(?>(?:}}?|\])*)|.)*?(?>[^{[\*\n]*(\*)))",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.BoldBegin},
                                           {2, ScopeName.BoldEnd}
                                       })
                           };
            }
        }
    }
}