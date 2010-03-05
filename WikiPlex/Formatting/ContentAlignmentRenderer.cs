using System;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// This will render the content alignment scopes.
    /// </summary>
    public class ContentAlignmentRenderer : IRenderer
    {
        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "ContentAlignment"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.AlignEnd
                    || scopeName == ScopeName.LeftAlign
                    || scopeName == ScopeName.RightAlign);
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
            switch (scopeName)
            {
                case ScopeName.AlignEnd:
                    return "</div><div style=\"clear:both;\"></div>";
                case ScopeName.LeftAlign:
                    return "<div style=\"text-align:left;float:left;\">";
                case ScopeName.RightAlign:
                    return "<div style=\"text-align:right;float:right;\">";
                default:
                    return input;
            }
        }
    }
}