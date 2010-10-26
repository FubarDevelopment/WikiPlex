using System;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// This will render the content alignment scopes.
    /// </summary>
    public class ContentAlignmentRenderer : RendererBase
    {
        ///<summary>
        /// Creates a new instance of the <see cref="ContentAlignmentRenderer"/> class.
        ///</summary>
        public ContentAlignmentRenderer() 
            : base(ScopeName.AlignEnd, ScopeName.LeftAlign, ScopeName.RightAlign)
        {}

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
        protected override string ExpandImpl(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            if (scopeName == ScopeName.AlignEnd)
                return "</div><div style=\"clear:both;height:0;\">&nbsp;</div>";
            if (scopeName == ScopeName.LeftAlign)
                return "<div style=\"text-align:left;float:left;\">";
            if (scopeName == ScopeName.RightAlign)
                return "<div style=\"text-align:right;float:right;\">";

            return null;
        }
    }
}