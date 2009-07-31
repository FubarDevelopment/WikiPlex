using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public class ListScopeAugmenter : IScopeAugmenter<INestedBlockMacro>
    {
        public IList<Scope> Augment(INestedBlockMacro macro, IList<Scope> capturedScopes, string content)
        {
            IList<Scope> newScopes = new List<Scope>();

            string firstScopeContent = content.Substring(capturedScopes[0].Index, capturedScopes[0].Length);
            int startLevel = macro.DetermineLevel(firstScopeContent);

            AugmentRecursively(content, macro, capturedScopes, newScopes, 0, startLevel, startLevel);

            // add the ending block scope as it was intentionally skipped
            Scope lastScope = capturedScopes[capturedScopes.Count - 1];

            // add the last scope as it was explicitly excluded
            newScopes.Add(new Scope(macro.BlockEndScope, lastScope.Index, lastScope.Length));

            return newScopes;
        }

        private static int AugmentRecursively(string wikiContent, INestedBlockMacro macro, IList<Scope> scopes,
                                              IList<Scope> newScopes, int currentIndex, int currentLevel,
                                              int startingLevel)
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

                        currentIndex = AugmentRecursively(wikiContent, macro, scopes, newScopes, currentIndex + 2,
                                                          peekLevel, startingLevel);
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
    }
}