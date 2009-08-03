using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public class MacroParser : IMacroParser
    {
        private readonly IMacroCompiler compiler;

        public MacroParser(IMacroCompiler compiler)
        {
            this.compiler = compiler;
        }

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