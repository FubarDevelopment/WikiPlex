using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public interface IScopeAugmenter
    {
        IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content);
    }
}