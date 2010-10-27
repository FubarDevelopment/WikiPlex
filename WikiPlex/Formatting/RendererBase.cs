using System;
using System.Linq;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// The base class for a <see cref="IRenderer"/>.
    /// </summary>
    public abstract class RendererBase : IRenderer
    {
        private const string UnresolvedError = @"<span class=""unresolved"">{0}</span>";
        private readonly string[] scopeNames;
        private readonly string rendererId;

        ///<summary>
        /// Creates a new instance of the <see cref="RendererBase"/>.
        ///</summary>
        /// <param name="scopeNames">The list of scope names the renderer can use.</param>
        protected RendererBase(params string[] scopeNames)
        {
            this.scopeNames = scopeNames;
            rendererId = GetType().Name.Replace("Renderer", string.Empty);
        }

        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return rendererId; }
        }

        /// <summary>
        /// Gets the invalid macro error text.
        /// </summary>
        public virtual string InvalidMacroError
        {
            get { return "Cannot resolve macro."; }
        }

        /// <summary>
        /// Gets the invalid argument error text.
        /// </summary>
        public virtual string InvalidArgumentError
        {
            get { return "Cannot resolve macro, invalid parameter '{0}'."; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return scopeNames.Any(s => s == scopeName);
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            if (!CanExpand(scopeName))
                throw new ArgumentException("Invalid scope name for this renderer.", "scopeName");

            try
            {
                return ExpandImpl(scopeName, input, htmlEncode, attributeEncode);
            }
            catch (ArgumentException ex)
            {
                return string.Format(UnresolvedError, string.Format(InvalidArgumentError, ex.ParamName));
            }
            catch (RenderException ex)
            {
                return string.Format(UnresolvedError, string.IsNullOrEmpty(ex.Message) ? InvalidMacroError : ex.Message);
            }
            catch
            {
                return string.Format(UnresolvedError, InvalidMacroError);
            }
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope for the derived types.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
        protected abstract string ExpandImpl(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode);
    }
}
