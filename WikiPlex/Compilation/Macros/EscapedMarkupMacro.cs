using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class EscapedMarkupMacro : IMacro
    {
        public string Id
        {
            get { return "Escaped Markup"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?s)({\""\n?)(.*?)(\n?\""})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.EscapedMarkup},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}