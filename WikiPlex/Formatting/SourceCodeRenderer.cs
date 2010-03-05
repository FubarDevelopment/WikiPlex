using System;
using ColorCode;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render all source code scopes.
    /// </summary>
    public class SourceCodeRenderer : IRenderer
    {
        private readonly ICodeColorizer codeColorizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceCodeRenderer"/> class.
        /// </summary>
        public SourceCodeRenderer()
            : this(new CodeColorizer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceCodeRenderer"/> class.
        /// </summary>
        /// <param name="codeColorizer">The <see cref="ICodeColorizer"/> to use for syntax highlighting.</param>
        public SourceCodeRenderer(ICodeColorizer codeColorizer)
        {
            this.codeColorizer = codeColorizer;
        }

        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "SourceCode"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.SingleLineCode
                    || scopeName == ScopeName.MultiLineCode
                    || scopeName == ScopeName.ColorCodeAshx
                    || scopeName == ScopeName.ColorCodeAspxCs
                    || scopeName == ScopeName.ColorCodeAspxVb
                    || scopeName == ScopeName.ColorCodeCpp
                    || scopeName == ScopeName.ColorCodeCSharp
                    || scopeName == ScopeName.ColorCodeHtml
                    || scopeName == ScopeName.ColorCodeJava
                    || scopeName == ScopeName.ColorCodeJavaScript
                    || scopeName == ScopeName.ColorCodeSql
                    || scopeName == ScopeName.ColorCodeVbDotNet
                    || scopeName == ScopeName.ColorCodeXml
                    || scopeName == ScopeName.ColorCodePhp
                    || scopeName == ScopeName.ColorCodeCss
                    || scopeName == ScopeName.ColorCodePowerShell);
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
                case ScopeName.SingleLineCode:
                    return string.Format("<span class=\"codeInline\">{0}</span>", htmlEncode(input));
                case ScopeName.MultiLineCode:
                    if (input.EndsWith(Environment.NewLine))
                        input = input.Substring(0, input.Length - Environment.NewLine.Length);
                    return string.Format("<pre>{0}</pre>", htmlEncode(input));
                case ScopeName.ColorCodeAshx:
                    return codeColorizer.Colorize(input, Languages.Ashx);
                case ScopeName.ColorCodeAspxCs:
                    return codeColorizer.Colorize(input, Languages.AspxCs);
                case ScopeName.ColorCodeAspxVb:
                    return codeColorizer.Colorize(input, Languages.AspxVb);
                case ScopeName.ColorCodeCpp:
                    return codeColorizer.Colorize(input, Languages.Cpp);
                case ScopeName.ColorCodeCSharp:
                    return codeColorizer.Colorize(input, Languages.CSharp);
                case ScopeName.ColorCodeHtml:
                    return codeColorizer.Colorize(input, Languages.Html);
                case ScopeName.ColorCodeJava:
                    return codeColorizer.Colorize(input, Languages.Java);
                case ScopeName.ColorCodeJavaScript:
                    return codeColorizer.Colorize(input, Languages.JavaScript);
                case ScopeName.ColorCodeSql:
                    return codeColorizer.Colorize(input, Languages.Sql);
                case ScopeName.ColorCodeVbDotNet:
                    return codeColorizer.Colorize(input, Languages.VbDotNet);
                case ScopeName.ColorCodeXml:
                    return codeColorizer.Colorize(input, Languages.Xml);
                case ScopeName.ColorCodePhp:
                    return codeColorizer.Colorize(input, Languages.Php);
                case ScopeName.ColorCodeCss:
                    return codeColorizer.Colorize(input, Languages.Css);
                case ScopeName.ColorCodePowerShell:
                    return codeColorizer.Colorize(input, Languages.PowerShell);
                default:
                    return input;
            }
        }
    }
}