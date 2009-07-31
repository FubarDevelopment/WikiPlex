using System.Collections.Generic;
using System.Threading;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;

namespace WikiPlex
{
    public static class Macros
    {
        private static readonly IDictionary<string, IMacro> loadedMacros;
        private static readonly ReaderWriterLockSlim macroLock;

        static Macros()
        {
            loadedMacros = new Dictionary<string, IMacro>();
            macroLock = new ReaderWriterLockSlim();

            // load the default macros
            Register<BoldMacro>();
            Register<ItalicsMacro>();
            Register<UnderlineMacro>();
            Register<HeadingsMacro>();
            Register<StrikethroughMacro>();
            Register<SubscriptMacro>();
            Register<SuperscriptMacro>();
            Register<HorizontalLineMacro>();
            Register<LinkMacro>();
            Register<ImageMacro>();
            Register<SourceCodeMacro>();
            Register<OrderedListMacro>();
            Register<UnorderedListMacro>();
            Register<EscapedMarkupMacro>();
            Register<RssFeedMacro>();
            Register<SilverlightMacro>();
            Register<VideoMacro>();
            Register<TableMacro>();
            Register<ContentLeftAlignmentMacro>();
            Register<ContentRightAlignmentMacro>();
        }

        public static IEnumerable<IMacro> All
        {
            get { return loadedMacros.Values; }
        }

        public static void Register<TMacro>()
            where TMacro : class, IMacro, new()
        {
            Register(new TMacro());
        }

        public static void Register(IMacro macro)
        {
            Guard.NotNull(macro, "macro");
            Guard.NotNullOrEmpty(macro.Id, "macro", "The macro identifier must not be null or empty.");

            macroLock.EnterWriteLock();
            try
            {
                loadedMacros[macro.Id] = macro;
            }
            finally
            {
                macroLock.ExitWriteLock();
            }
        }

        public static void Unregister<TMacro>()
            where TMacro : class, IMacro, new()
        {
            Unregister(new TMacro());
        }

        public static void Unregister(IMacro macro)
        {
            Guard.NotNull(macro, "macro");
            Guard.NotNullOrEmpty(macro.Id, "macro", "The macro identifier must not be null or empty.");

            macroLock.EnterWriteLock();
            try
            {
                if (loadedMacros.ContainsKey(macro.Id))
                    loadedMacros.Remove(macro.Id);
            }
            finally
            {
                macroLock.ExitWriteLock();
            }
        }
    }
}