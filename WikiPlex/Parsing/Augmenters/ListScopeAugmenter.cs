using System;
using System.Collections.Generic;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    /// <summary>
    /// Handles augmenting the scopes for the <see cref="ListMacro"/>.
    /// </summary>
    public class ListScopeAugmenter : IScopeAugmenter
    {
        /// <summary>
        /// This will insert new, remove, or re-order scopes.
        /// </summary>
        /// <param name="macro">The current macro.</param>
        /// <param name="capturedScopes">The list of captured scopes.</param>
        /// <param name="content">The wiki content being parsed.</param>
        /// <returns>A new list of augmented scopes.</returns>
        public IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
        {
            IList<Scope> newScopes = new List<Scope>();

            string firstScopeContent = content.Substring(capturedScopes[0].Index, capturedScopes[0].Length);
            char depthChar = GetDepthChar(firstScopeContent);
            int startLevel = Utility.CountChars(depthChar, firstScopeContent);

            Tuple<int, char> output = AugmentRecursively(content, capturedScopes, newScopes, 0, startLevel, startLevel, depthChar);

            // add the ending block scope as it was intentionally skipped
            Scope lastScope = capturedScopes[capturedScopes.Count - 1];

            // add the last scope as it was explicitly excluded
            newScopes.Add(new Scope(GetEndScope(output.Item2), lastScope.Index, lastScope.Length));

            return newScopes;
        }

        private static Tuple<int, char> AugmentRecursively(string wikiContent, IList<Scope> scopes, IList<Scope> newScopes, int currentIndex, int currentLevel, int startingLevel, char currentDepthChar)
        {
            for (; (currentIndex + 1) < scopes.Count; currentIndex++)
            {
                Scope current = scopes[currentIndex];
                Scope peek = scopes[currentIndex + 1];
                string peekContent = wikiContent.Substring(peek.Index, peek.Length);

                if (currentIndex == 0)
                {
                    newScopes.Add(new Scope(GetStartScope(currentDepthChar), current.Index, current.Length));
                    continue;
                }

                if (current.Name == ScopeName.ListItemEnd && (current.Index + current.Length + 1) < peek.Index)
                {
                    // ending a block and starting a new block
                    if (currentLevel > startingLevel)
                        return Tuple.Create(currentIndex - 1, currentDepthChar);

                    newScopes.Add(new Scope(GetEndScope(currentDepthChar), current.Index, current.Length));

                    currentDepthChar = GetDepthChar(peekContent);
                    newScopes.Add(new Scope(GetStartScope(currentDepthChar), peek.Index, peek.Length));
                    currentLevel = startingLevel = Utility.CountChars(currentDepthChar, peekContent);
                    currentIndex++;
                    continue;
                }

                if (current.Name == ScopeName.ListItemEnd)
                {
                    currentDepthChar = GetDepthChar(peekContent);
                    int peekLevel = Utility.CountChars(currentDepthChar, peekContent);

                    if (currentLevel > peekLevel && currentLevel != startingLevel)
                        return Tuple.Create(currentIndex - 1, currentDepthChar);

                    if (currentLevel > peekLevel)
                    {
                        // ending the blocks since the current level is
                        // the same level as the starting level
                        Scope lastNewScope = scopes[currentIndex + 1];
                        newScopes.Add(new Scope(GetEndScope(currentDepthChar), lastNewScope.Index, lastNewScope.Length));

                        // starting a new nested block
                        newScopes.Add(new Scope(GetStartScope(currentDepthChar), peek.Index, peek.Length));

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
                        newScopes.Add(new Scope(GetStartScope(currentDepthChar), peek.Index, peek.Length));

                        Tuple<int, char> output = AugmentRecursively(wikiContent, scopes, newScopes, currentIndex + 2, peekLevel, startingLevel, currentDepthChar);
                        currentIndex = output.Item1;
                        Scope lastNewScope = scopes[currentIndex + 1];

                        // ending the nested block
                        for (int j = peekLevel - currentLevel; j > 0; j--)
                            newScopes.Add(new Scope(GetEndScope(currentDepthChar), lastNewScope.Index, lastNewScope.Length));

                        continue;
                    }
                }
                else
                {
                    // this is a new starting item, if the level 
                    // does not match back out to continue processing
                    // with the current item
                    string currentContent = wikiContent.Substring(current.Index, current.Length);
                    if (currentLevel != Utility.CountChars(currentDepthChar, currentContent))
                        return Tuple.Create(currentIndex - 1, currentDepthChar);
                }

                newScopes.Add(current);
            }

            return Tuple.Create(currentIndex - 1, currentDepthChar);
        }

        private static char GetDepthChar(string content)
        {
            return content.Trim()[0];
        }

        private static string GetStartScope(char depthChar)
        {
            return depthChar == '#' 
                ? ScopeName.OrderedListBeginTag 
                : ScopeName.UnorderedListBeginTag;
        }

        private static string GetEndScope(char depthChar)
        {
            return depthChar == '#'
                ? ScopeName.OrderedListEndTag
                : ScopeName.UnorderedListEndTag;
        }
    }
}