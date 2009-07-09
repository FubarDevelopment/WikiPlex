using System;
using WikiPlex.Parsing;

namespace WikiPlex.Formatting
{
    public class RenderedScopeEventArgs : EventArgs
    {
        private readonly Scope scope;
        private readonly string content;

        public RenderedScopeEventArgs(Scope scope, string content)
        {
            this.scope = scope;
            this.content = content;
        }

        public string Content
        {
            get { return content; }
        }

        public Scope Scope
        {
            get { return scope; }
        }
    }
}