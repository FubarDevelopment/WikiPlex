using System;
using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    /// <summary>
    /// Defines the <see cref="MacroParser"/> contract.
    /// </summary>
    public interface IMacroParser
    {
        /// <summary>
        /// Will parse the wiki content pushing scopes into the parse handler.
        /// </summary>
        /// <param name="wikiContent">The wiki content.</param>
        /// <param name="macros">The macros to use for parsing.</param>
        /// <param name="scopeAugmenters">The scope augmenters to use for parsing.</param>
        /// <param name="parseHandler">The action method that is used for pushing parsed scopes.</param>
        void Parse(string wikiContent, IEnumerable<IMacro> macros, IDictionary<string, IScopeAugmenter> scopeAugmenters, Action<IList<Scope>> parseHandler);
    }
}