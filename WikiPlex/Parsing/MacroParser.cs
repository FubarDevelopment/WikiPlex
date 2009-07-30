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

        public void Parse(string wikiContent, IEnumerable<IMacro> macros, Action<IList<Scope>> parseHandler)
        {
            if (string.IsNullOrEmpty(wikiContent))
                return;

            Guard.NotNullOrEmpty(macros, "macros");

            foreach (IMacro macro in macros)
                Parse(wikiContent, compiler.Compile(macro), parseHandler);
        }

        private static void Parse(string wikiContent, CompiledMacro macro, Action<IList<Scope>> parseHandler)
        {
            Match regexMatch = macro.Regex.Match(wikiContent);
            if (!regexMatch.Success)
                return;

            IList<Scope> capturedScopes = new List<Scope>();

            while (regexMatch.Success)
            {
                string matchedContent = wikiContent.Substring(regexMatch.Index, regexMatch.Length);
                if (!string.IsNullOrEmpty(matchedContent))
                    capturedScopes = GetCapturedMatches(regexMatch, macro, capturedScopes);

                regexMatch = regexMatch.NextMatch();
            }

            capturedScopes = BlockOutCapturedScopes(wikiContent, macro, capturedScopes);

            if (capturedScopes.Count > 0)
                parseHandler(capturedScopes);
        }


        private static IList<Scope> BlockOutCapturedScopes(string wikiContent, CompiledMacro compiledMacro, IList<Scope> scopes)
        {
            if (scopes.Count == 0 || (!(compiledMacro is CompiledNestedBlockMacro) && !(compiledMacro is CompiledBlockMacro)))
                return scopes;

            var nestedMacro = compiledMacro as CompiledNestedBlockMacro;
            if (nestedMacro != null)
                return GetNestedBlockScopes(scopes, nestedMacro, wikiContent);

            var blockMacro = compiledMacro as CompiledBlockMacro;
            return GetBlockScopes(scopes, blockMacro);
        }

        private static IList<Scope> GetBlockScopes(IList<Scope> scopes, CompiledBlockMacro macro)
        {
            IList<Scope> newScopes = new List<Scope>();

            for (int i = 0; (i + 1) < scopes.Count; i++)
            {
                Scope current = scopes[i];
                Scope peek = scopes[i + 1];

                if (i == 0)
                {
                    // this is the first item, ensure the block has started
                    newScopes.Add(new Scope(macro.BlockStartScope, current.Index, current.Length));
                    newScopes.Add(new Scope(current.Name, current.Index, current.Length));
                    continue;
                }

                if (current.Name == macro.ItemEndScope && (current.Index + current.Length + 1) < peek.Index)
                {
                    // ending a block and starting a new block
                    newScopes.Add(current);
                    newScopes.Add(new Scope(macro.BlockEndScope, current.Index + current.Length));
                    newScopes.Add(new Scope(macro.BlockStartScope, peek.Index, peek.Length));
                    newScopes.Add(new Scope(peek.Name, peek.Index, peek.Length));
                    i++;
                    continue;
                }

                newScopes.Add(current);
            }

            // explicitly add the last scope as it was intentionally skipped
            Scope lastScope = scopes[scopes.Count - 1];
            newScopes.Add(lastScope);
            newScopes.Add(new Scope(macro.BlockEndScope, lastScope.Index + lastScope.Length));

            return newScopes;
        }

        private static IList<Scope> GetNestedBlockScopes(IList<Scope> scopes, CompiledNestedBlockMacro macro, string wikiContent)
        {
            IList<Scope> newScopes = new List<Scope>();

            if (scopes.Count == 1)
            {
                scopes.Insert(0, new Scope(macro.BlockStartScope, scopes[0].Index));
                scopes.Add(new Scope(macro.BlockEndScope, scopes[1].Index + scopes[1].Length));
                return scopes;
            }

            string firstScopeContent = wikiContent.Substring(scopes[0].Index, scopes[0].Length);
            int startLevel = macro.DetermineLevel(firstScopeContent);

            GetNestedBlockScopesRecursively(wikiContent, macro, scopes, newScopes, 0, startLevel, startLevel);

            // add the ending block scope as it was intentionally skipped
            Scope lastScope = scopes[scopes.Count - 1];

            // add the last scope as it was explicitly excluded
            newScopes.Add(new Scope(macro.BlockEndScope, lastScope.Index, lastScope.Length));

            return newScopes;
        }

        private static int GetNestedBlockScopesRecursively(string wikiContent, CompiledNestedBlockMacro macro, IList<Scope> scopes, IList<Scope> newScopes, int currentIndex, int currentLevel, int startingLevel)
        {
            for (; (currentIndex + 1) < scopes.Count; currentIndex++)
            {
                Scope current = scopes[currentIndex];
                Scope peek = scopes[currentIndex + 1];

                if (currentIndex == 0)
                {
                    newScopes.Add(new Scope(macro.BlockStartScope, current.Index, current.Length));
                    continue;
                }

                if (current.Name == macro.ItemEndScope && (current.Index + current.Length + 1) < peek.Index)
                {
                    // ending a block and starting a new block
                    if (currentLevel > startingLevel)
                        return currentIndex - 1;

                    newScopes.Add(new Scope(macro.BlockEndScope, current.Index, current.Length));
                    newScopes.Add(new Scope(macro.BlockStartScope, peek.Index, peek.Length));
                    currentLevel = startingLevel = macro.DetermineLevel(wikiContent.Substring(peek.Index, peek.Length));
                    currentIndex++;
                    continue;
                }

                if (current.Name == macro.ItemEndScope)
                {
                    string peekContent = wikiContent.Substring(peek.Index, peek.Length);
                    int peekLevel = macro.DetermineLevel(peekContent);

                    if (currentLevel > peekLevel && currentLevel != startingLevel)
                        return currentIndex - 1;

                    if (currentLevel > peekLevel)
                    {
                        // ending the blocks since the current level is
                        // the same level as the starting level
                        Scope lastNewScope = scopes[currentIndex + 1];
                        newScopes.Add(new Scope(macro.BlockEndScope, lastNewScope.Index, lastNewScope.Length));

                        // starting a new nested block
                        newScopes.Add(new Scope(macro.BlockStartScope, peek.Index, peek.Length));

                        // now skip the current item because the block
                        // start scope contains this start. We also 
                        // need to set the current level to the peek
                        // level so it will continue processing correctly
                        currentIndex++;
                        currentLevel = peekLevel;

                        continue;
                    }

                    if (currentLevel < peekLevel)
                    {
                        // starting a new nested block
                        newScopes.Add(new Scope(macro.BlockStartScope, peek.Index, peek.Length));

                        currentIndex = GetNestedBlockScopesRecursively(wikiContent, macro, scopes, newScopes, currentIndex + 2, peekLevel, startingLevel);
                        Scope lastNewScope = scopes[currentIndex + 1];

                        // ending the nested block
                        for (int j = peekLevel - currentLevel; j > 0; j--)
                            newScopes.Add(new Scope(macro.BlockEndScope, lastNewScope.Index, lastNewScope.Length));

                        continue;
                    }
                }
                else
                {
                    // this is a new starting item, if the level 
                    // does not match back out to continue processing
                    // with the current item
                    string currentContent = wikiContent.Substring(current.Index, current.Length);
                    if (currentLevel != macro.DetermineLevel(currentContent))
                        return currentIndex - 1;
                }

                newScopes.Add(current);
            }

            return currentIndex - 1;
        }

        private static IList<Scope> GetCapturedMatches(Match regexMatch, CompiledMacro macro, IList<Scope> capturedMatches)
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


        private static void AppendCapturedMatchesForRegexCapture(Capture regexCapture, string capture, ICollection<Scope> capturedScopes)
        {
            capturedScopes.Add(new Scope(capture, regexCapture.Index, regexCapture.Length));
        }
    }
}