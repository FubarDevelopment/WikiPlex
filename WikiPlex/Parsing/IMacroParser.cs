using System;
using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public interface IMacroParser
    {
        void Parse(string wikiContent, IEnumerable<IMacro> macros, Action<IList<Scope>> parseHandler);
    }
}