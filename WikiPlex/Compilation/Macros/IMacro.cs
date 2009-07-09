using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public interface IMacro
    {
        string Id { get; }
        IList<MacroRule> Rules { get; }
    }
}