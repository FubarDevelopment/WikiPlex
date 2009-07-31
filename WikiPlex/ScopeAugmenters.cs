using System;
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
        private static readonly IDictionary<Type, object> loadedAugmenters;

        static ScopeAugmenters()
        {
            loadedAugmenters = new Dictionary<Type, object>();
            augmenterLock = new ReaderWriterLockSlim();
        }

        public static void Register<TMacro, TAugmenter>()
            where TMacro : class, IMacro
            where TAugmenter : class, IScopeAugmenter<TMacro>, new()
        {
            Register(new TAugmenter());
        }

        public static void Register<TMacro>(IScopeAugmenter<TMacro> augmenter)
            where TMacro : class, IMacro
        {
            Guard.NotNull(augmenter, "augmenter");

            augmenterLock.EnterWriteLock();
            try
            {
                loadedAugmenters[typeof (TMacro)] = augmenter;
            }
            finally
            {
                augmenterLock.ExitWriteLock();
            }
        }

        public static void Unregister<TMacro>()
            where TMacro : class, IMacro
        {
            augmenterLock.EnterWriteLock();
            try
            {
                Type type = typeof (TMacro);
                if (loadedAugmenters.ContainsKey(type))
                    loadedAugmenters.Remove(type);
            }
            finally
            {
                augmenterLock.ExitWriteLock();
            }
        }

        public static IScopeAugmenter<TMacro> FindByMacro<TMacro>()
            where TMacro : class, IMacro, new()
        {
            return FindByMacro(new TMacro());
        }

        public static IScopeAugmenter<TMacro> FindByMacro<TMacro>(TMacro macro)
            where TMacro : class, IMacro
        {
            Guard.NotNull(macro, "macro");

            augmenterLock.EnterReadLock();
            try
            {
                Type type = typeof (TMacro);
                if (loadedAugmenters.ContainsKey(type))
                    return (IScopeAugmenter<TMacro>) loadedAugmenters[type];
            }
            finally
            {
                augmenterLock.ExitReadLock();
            }

            return null;
        }
    }
}