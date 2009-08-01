using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class SilverlightMacro : IMacro
    {
        public string Id
        {
            get { return "Silverlight"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?i)(\{silverlight\:)([^\}]+)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.Silverlight},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}