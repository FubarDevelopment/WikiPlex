using System.Collections.Generic;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting;

namespace WikiPlex
{
    public interface IWikiEngine
    {
        string Render(string wikiContent);
        string Render(string wikiContent, IFormatter formatter);
        string Render(string wikiContent, IEnumerable<IMacro> macros);
        string Render(string wikiContent, IEnumerable<IMacro> macros, IFormatter formatter);
    }
}