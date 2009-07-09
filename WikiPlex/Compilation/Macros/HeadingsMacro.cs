using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class HeadingsMacro : IMacro
    {
        public string Id
        {
            get { return "Headings"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(^!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingOneBegin},
                                           {2, ScopeName.HeadingOneEnd}
                                       }),
                               new MacroRule(
                                   @"(^!!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingTwoBegin},
                                           {2, ScopeName.HeadingTwoEnd}
                                       }),
                               new MacroRule(
                                   @"(^!!!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingThreeBegin},
                                           {2, ScopeName.HeadingThreeEnd}
                                       }),
                               new MacroRule(
                                   @"(^!!!!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingFourBegin},
                                           {2, ScopeName.HeadingFourEnd}
                                       }),
                               new MacroRule(
                                   @"(^!!!!!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingFiveBegin},
                                           {2, ScopeName.HeadingFiveEnd}
                                       }),
                                new MacroRule(
                                   @"(^!!!!!!\s)[^\r\n]*(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.HeadingSixBegin},
                                           {2, ScopeName.HeadingSixEnd}
                                       })
                           };
            }
        }
    }
}