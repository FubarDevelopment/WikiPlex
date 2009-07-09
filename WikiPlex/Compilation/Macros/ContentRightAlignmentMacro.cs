using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class ContentRightAlignmentMacro : IMacro
    {
        public string Id
        {
            get { return "ContentRightAlignment"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?<!.+)(?s)(>{(?:\r?\n)?).*?(?=}>)(}>(?:\r?\n)?)(?-s)(?![<>])",
                                   new Dictionary<int, string>
                                       {
                                           { 1, ScopeName.RightAlign },
                                           { 2, ScopeName.AlignEnd }
                                       })
                           };
            }
        }
    }
}