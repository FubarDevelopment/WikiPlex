using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    /// <summary>
    /// This macro will indent text.
    /// </summary>
    /// <example><code language="none">
    /// : First Level Indentation
    /// :: Second Level Indentation
    /// </code></example>
    public class IndentationMacro : IMacro
    {
        /// <summary>
        /// Gets the id of the macro.
        /// </summary>
        public string Id
        {
            get { return "Indentation"; }
        }

        /// <summary>
        /// Gets the list of rules.
        /// </summary>
        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(?s)(^:{\s*(?:\r?\n|$)).*?(^:}\s*(?:\r?\n|$))",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.IndentationBegin},
                                           {2, ScopeName.IndentationEnd}
                                       }),
                               new MacroRule(
                                   @"(^:+\s?)[^\r\n]+(\r?\n|$)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.IndentationBegin},
                                           {2, ScopeName.IndentationEnd}
                                       })
                           };
            }
        }
    }
}