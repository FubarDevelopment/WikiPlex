using System.Collections.Generic;
using System.Threading;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;

namespace WikiPlex
{
    public static class ScopeAugmenters
    {
        private static readonly ReaderWriterLockSlim augmenterLock;
        private static readonly IDictionary<string, IScopeAugmenter> loadedAugmenters;

        static ScopeAugmenters()
        {
            loadedAugmenters = new Dictionary<string, IScopeAugmenter>();
            augmenterLock = new ReaderWriterLockSlim();

            // register the default scope augmenters
            Register<TableMacro, TableScopeAugmenter>();
            Register<OrderedListMacro, ListScopeAugmenter<OrderedListMacro>>();
            Register<UnorderedListMacro, ListScopeAugmenter<UnorderedListMacro>>();
        }

        public static void Register<TMacro, TAugmenter>()
            where TMacro : class, IMacro, new()
            where TAugmenter : class, IScopeAugmenter, new()
        {
            Register<TMacro>(new TAugmenter());
        }

        public static void Register<TMacro>(IScopeAugmenter augmenter)
            where TMacro : class, IMacro, new()
        {
            Guard.NotNull(augmenter, "augmenter");

            augmenterLock.EnterWriteLock();
            try
            {
                loadedAugmenters[(new TMacro()).Id] = augmenter;
            }
            finally
            {
                augmenterLock.ExitWriteLock();
            }
        }

        public static void Unregister<TMacro>()
            where TMacro : class, IMacro, new()
        {
            augmenterLock.EnterWriteLock();
            try
            {
                var macro = new TMacro();
                if (loadedAugmenters.ContainsKey(macro.Id))
                    loadedAugmenters.Remove(macro.Id);
            }
            finally
            {
                augmenterLock.ExitWriteLock();
            }
        }

        public static IScopeAugmenter FindByMacro<TMacro>()
            where TMacro : class, IMacro, new()
        {
            return FindByMacro(new TMacro());
        }

        public static IScopeAugmenter FindByMacro<TMacro>(TMacro macro)
            where TMacro : class, IMacro
        {
            Guard.NotNull(macro, "macro");

            augmenterLock.EnterReadLock();
            try
            {
                if (loadedAugmenters.ContainsKey(macro.Id))
                    return loadedAugmenters[macro.Id];
            }
            finally
            {
                augmenterLock.ExitReadLock();
            }

            return null;
        }
    }
}