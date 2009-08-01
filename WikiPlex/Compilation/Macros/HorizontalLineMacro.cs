using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class HorizontalLineMacro : IMacro
    {
        public string Id
        {
            get { return "Horizontal Line"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(@"^(----)", ScopeName.HorizontalRule)
                           };
            }
        }
    }
}