﻿using System.Collections.Generic;
using System.Threading;
using WikiPlex.Common;
using WikiPlex.Formatting;

namespace WikiPlex
{
    public class Renderers
    {
        private static readonly IDictionary<string, IRenderer> loadedRenderers;
        private static readonly ReaderWriterLockSlim rendererLock;

        static Renderers()
        {
            loadedRenderers = new Dictionary<string, IRenderer>();
            rendererLock = new ReaderWriterLockSlim();

            // load the default renderers
            Register<TextFormattingRenderer>();
            Register<LinkRenderer>();
            Register<ImageRenderer>();
            Register<SourceCodeRenderer>();
            Register<ListItemRenderer>();
            Register<RssFeedRenderer>();
            Register<SilverlightRenderer>();
            Register<VideoRenderer>();
            Register<TableRenderer>();
            Register<ContentAlignmentRenderer>();
        }

        public static IEnumerable<IRenderer> All
        {
            get { return loadedRenderers.Values; }
        }

        public static void Register<T>()
            where T : IRenderer, new()
        {
            Register(new T());
        }

        public static void Register(IRenderer renderer)
        {
            Guard.NotNull(renderer, "renderer");
            Guard.NotNullOrEmpty(renderer.Id, "renderer", "The renderer identifier must not be null or empty.");

            rendererLock.EnterWriteLock();
            try
            {
                loadedRenderers[renderer.Id] = renderer;
            }
            finally
            {
                rendererLock.ExitWriteLock();
            }
        }

        public static void Unregister<T>()
            where T : IRenderer, new()
        {
            Unregister(new T());
        }

        public static void Unregister(IRenderer renderer)
        {
            Guard.NotNull(renderer, "renderer");
            Guard.NotNullOrEmpty(renderer.Id, "renderer", "The renderer identifier must not be null or empty.");

            rendererLock.EnterWriteLock();
            try
            {
                if (loadedRenderers.ContainsKey(renderer.Id))
                    loadedRenderers.Remove(renderer.Id);
            }
            finally
            {
                rendererLock.ExitWriteLock();
            }
        }
    }
}