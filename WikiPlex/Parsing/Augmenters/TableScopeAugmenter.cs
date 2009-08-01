﻿using System.Collections.Generic;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public class TableScopeAugmenter : IScopeAugmenter
    {
        public IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
        {
            IList<Scope> augmentedScopes = new List<Scope>();

            for (int i = 0; (i + 1) < capturedScopes.Count; i++)
            {
                Scope current = capturedScopes[i];
                Scope peek = capturedScopes[i + 1];

                if (i == 0)
                {
                    // this is the first item, ensure the block has started
                    augmentedScopes.Add(CreateStartScope(current));
                    augmentedScopes.Add(current);
                    continue;
                }

                if (current.Name == ScopeName.TableRowEnd
                    && (current.Index + current.Length + 1) < peek.Index)
                {
                    // ending a block and starting a new block
                    augmentedScopes.Add(current);
                    augmentedScopes.Add(CreateEndScope(current));
                    augmentedScopes.Add(CreateStartScope(peek));
                    augmentedScopes.Add(peek);
                    i++;
                    continue;
                }

                augmentedScopes.Add(current);
            }

            // explicitly add the last scope as it was intentionally skipped
            Scope last = capturedScopes[capturedScopes.Count - 1];
            augmentedScopes.Add(last);
            augmentedScopes.Add(CreateEndScope(last));

            return augmentedScopes;
        }

        private static Scope CreateStartScope(Scope scope)
        {
            return new Scope(ScopeName.TableBegin, scope.Index, scope.Length);
        }

        private static Scope CreateEndScope(Scope scope)
        {
            return new Scope(ScopeName.TableEnd, scope.Index + scope.Length);
        }
    }
}