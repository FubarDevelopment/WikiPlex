using System;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// This will render the ordered list and unordered list scopes.
    /// </summary>
    public class ListItemRenderer : IRenderer
    {
        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "ListItemFormatting"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.OrderedListBeginTag
                    || scopeName == ScopeName.OrderedListEndTag
                    || scopeName == ScopeName.UnorderedListBeginTag
                    || scopeName == ScopeName.UnorderedListEndTag
                    || scopeName == ScopeName.ListItemBegin
                    || scopeName == ScopeName.ListItemEnd);
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
                case ScopeName.OrderedListBeginTag:
                    return "<ol><li>";
                case ScopeName.OrderedListEndTag:
                    return "</li></ol>";
                case ScopeName.UnorderedListBeginTag:
                    return "<ul><li>";
                case ScopeName.UnorderedListEndTag:
                    return "</li></ul>";
                case ScopeName.ListItemBegin:
                    return "<li>";
                case ScopeName.ListItemEnd:
                    return "</li>";
                default:
                    return input;
            }
        }
    }
}