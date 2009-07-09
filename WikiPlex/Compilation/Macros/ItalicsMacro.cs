using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class ItalicsMacro : IMacro
    {
        public string Id
        {
            get { return "Italics"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?-s)(?:(_)(?>[^{\[_\n]*)(?>(?:{{?|\[)(?>[^}\]\n]*)(?>(?:}}?|\])*)|.)*?(?>[^{\[_\n]*)(_))",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.ItalicsBegin},
                                           {2, ScopeName.ItalicsEnd}
                                       })
                           };
            }
        }
    }
}