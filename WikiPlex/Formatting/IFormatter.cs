using System;
using System.Collections.Generic;
using System.Text;
using WikiPlex.Parsing;

namespace WikiPlex.Formatting
{
    public interface IFormatter
    {
        event EventHandler<RenderedScopeEventArgs> ScopeRendered;

        void RecordParse(IList<Scope> scopes);
        void Format(string wikiContent, StringBuilder writer);
    }
}