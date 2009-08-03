using System;
using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public interface IMacroParser
    {
        void Parse(string wikiContent, IEnumerable<IMacro> macros, IDictionary<string, IScopeAugmenter> scopeAugmenters, Action<IList<Scope>> parseHandler);
    }
}