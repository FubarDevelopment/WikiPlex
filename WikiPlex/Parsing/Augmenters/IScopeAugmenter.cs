using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public interface IScopeAugmenter<T>
        where T : IMacro
    {
        IList<Scope> Augment(T macro, IList<Scope> capturedScopes, string content);
    }
}