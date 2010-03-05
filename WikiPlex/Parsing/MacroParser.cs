using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    /// <summary>
    /// Handles parsing the wiki content.
    /// </summary>
    public class MacroParser : IMacroParser
    {
        private readonly IMacroCompiler compiler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroParser"/> class.
        /// </summary>
        /// <param name="compiler">The <see cref="IMacroCompiler"/> to use for compiling macros.</param>
        public MacroParser(IMacroCompiler compiler)
        {
            this.compiler = compiler;
        }

        /// <summary>
        /// Will parse the wiki content pushing scopes into the parse handler.
        /// </summary>
        /// <param name="wikiContent">The wiki content.</param>
        /// <param name="macros">The macros to use for parsing.</param>
        /// <param name="scopeAugmenters">The scope augmenters to use for parsing.</param>
        /// <param name="parseHandler">The action method that is used for pushing parsed scopes.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when macros is null.
        /// 
        /// -- or --
        /// 
        /// Thrown when scope augmenters is null.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when macros is empty.</exception>
        public void Parse(string wikiContent, IEnumerable<IMacro> macros, IDictionary<string, IScopeAugmenter> scopeAugmenters, Action<IList<Scope>> parseHandler)
        {
            if (string.IsNullOrEmpty(wikiContent))
                return;

            Guard.NotNullOrEmpty(macros, "macros");
            Guard.NotNull(scopeAugmenters, "scopeAugmenters");

            foreach (IMacro macro in macros)
                Parse(wikiContent, macro, compiler.Compile(macro), scopeAugmenters.FindByMacro(macro), parseHandler);
        }

        private static void Parse(string wikiContent, IMacro macro, CompiledMacro compiledMacro,
                                  IScopeAugmenter augmenter, Action<IList<Scope>> parseHandler)
        {
            Match regexMatch = compiledMacro.Regex.Match(wikiContent);
            if (!regexMatch.Success)
                return;

            IList<Scope> capturedScopes = new List<Scope>();

            while (regexMatch.Success)
            {
                string matchedContent = wikiContent.Substring(regexMatch.Index, regexMatch.Length);
                if (!string.IsNullOrEmpty(matchedContent))
                    capturedScopes = GetCapturedMatches(regexMatch, compiledMacro, capturedScopes);

                regexMatch = regexMatch.NextMatch();
            }

            if (augmenter != null && capturedScopes.Count > 0)
                capturedScopes = augmenter.Augment(macro, capturedScopes, wikiContent);

            if (capturedScopes.Count > 0)
                parseHandler(capturedScopes);
        }

        private static IList<Scope> GetCapturedMatches(Match regexMatch, CompiledMacro macro,
                                                       IList<Scope> capturedMatches)
        {
            for (int i = 0; i < regexMatch.Groups.Count; i++)
            {
                Group regexGroup = regexMatch.Groups[i];
                string capture = macro.Captures[i];

                if (regexGroup.Captures.Count == 0 || String.IsNullOrEmpty(capture))
                    continue;

                foreach (Capture regexCapture in regexGroup.Captures)
                    AppendCapturedMatchesForRegexCapture(regexCapture, capture, capturedMatches);
            }

            return capturedMatches;
        }


        private static void AppendCapturedMatchesForRegexCapture(Capture regexCapture, string capture,
                                                                 ICollection<Scope> capturedScopes)
        {
            capturedScopes.Add(new Scope(capture, regexCapture.Index, regexCapture.Length));
        }
    }
}