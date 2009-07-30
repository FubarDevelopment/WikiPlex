using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
    public class TableScopeAugmenter : IScopeAugmenter<TableMacro>
    {
        public IList<Scope> Augment(TableMacro macro, IList<Scope> capturedScopes, string content)
        {
            IList<Scope> augmentedScopes = new List<Scope>();

            for (int i = 0; (i + 1) < capturedScopes.Count; i++)
            {
                Scope current = capturedScopes[i];
                Scope peek = capturedScopes[i + 1];

                if (i == 0)
                {
                    // this is the first item, ensure the block has started
                    augmentedScopes.Add(CreateStartScope(macro, current));
                    augmentedScopes.Add(current);
                    continue;
                }

                if (current.Name == macro.ItemEndScope
                    && (current.Index + current.Length + 1) < peek.Index)
                {
                    // ending a block and starting a new block
                    augmentedScopes.Add(current);
                    augmentedScopes.Add(CreateEndScope(macro, current));
                    augmentedScopes.Add(CreateStartScope(macro, peek));
                    augmentedScopes.Add(peek);
                    i++;
                    continue;
                }

                augmentedScopes.Add(current);
            }

            // explicitly add the last scope as it was intentionally skipped
            Scope last = capturedScopes[capturedScopes.Count - 1];
            augmentedScopes.Add(last);
            augmentedScopes.Add(CreateEndScope(macro, last));

            return augmentedScopes;
        }

        private static Scope CreateStartScope(IBlockMacro macro, Scope scope)
        {
            return new Scope(macro.BlockStartScope, scope.Index, scope.Length);
        }

        private static Scope CreateEndScope(IBlockMacro macro, Scope scope)
        {
            return new Scope(macro.BlockEndScope, scope.Index + scope.Length);
        }
    }
}