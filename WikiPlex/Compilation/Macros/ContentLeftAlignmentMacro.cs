using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class ContentLeftAlignmentMacro : IMacro
    {
        public string Id
        {
            get { return "ContentLeftAlignment"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?<!.+)(?s)(<{(?:\r?\n)?).*?(?=}<)(}<(?:\r?\n)?)(?-s)(?![<>])",
                                   new Dictionary<int, string>
                                       {
                                           { 1, ScopeName.LeftAlign },
                                           { 2, ScopeName.AlignEnd }
                                       })
                           };
            }
        }
    }
}