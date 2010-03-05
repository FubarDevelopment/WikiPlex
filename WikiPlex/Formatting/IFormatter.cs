using System;
using System.Collections.Generic;
using System.Text;
using WikiPlex.Parsing;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Defines the contract for the <see cref="MacroFormatter"/> class.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Event that is raised when a scope is rendered.
        /// </summary>
        event EventHandler<RenderedScopeEventArgs> ScopeRendered;

        /// <summary>
        /// Will record the parsing of scopes.
        /// </summary>
        /// <param name="scopes">The parsed scopes.</param>
        void RecordParse(IList<Scope> scopes);
        
        /// <summary>
        /// Will format the wiki content based on the recorded scopes and output the content to the writer.
        /// </summary>
        /// <param name="wikiContent">The wiki content to format.</param>
        /// <param name="writer">The writer to write the content to.</param>
        void Format(string wikiContent, StringBuilder writer);
    }
}