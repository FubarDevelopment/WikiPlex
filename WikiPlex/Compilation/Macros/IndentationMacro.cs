using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class IndentationMacro : IMacro
    {
        public string Id
        {
            get { return "Indentation"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(^:\s)[^\r\n]+((?:\r\n)?)$",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.IndentationBegin},
                                           {2, ScopeName.IndentationEnd}
                                       })
                           };
            }
        }
    }
}