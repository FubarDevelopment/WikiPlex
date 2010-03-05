using System;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render all table based scopes.
    /// </summary>
    public class TableRenderer : IRenderer
    {
        /// <summary>
        /// Gets the id of the renderer.
        /// </summary>
        public string Id
        {
            get { return "Table"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.TableBegin
                    || scopeName == ScopeName.TableEnd
                    || scopeName == ScopeName.TableCell
                    || scopeName == ScopeName.TableCellHeader
                    || scopeName == ScopeName.TableRowBegin
                    || scopeName == ScopeName.TableRowEnd
                    || scopeName == ScopeName.TableRowHeaderBegin
                    || scopeName == ScopeName.TableRowHeaderEnd);
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
                case ScopeName.TableBegin:
                    return "<table>";
                case ScopeName.TableEnd:
                    return "</table>";
                case ScopeName.TableCell:
                    return "</td><td>";
                case ScopeName.TableCellHeader:
                    return "</th><th>";
                case ScopeName.TableRowBegin:
                    return "<tr><td>";
                case ScopeName.TableRowEnd:
                    return "</td></tr>";
                case ScopeName.TableRowHeaderBegin:
                    return "<tr><th>";
                case ScopeName.TableRowHeaderEnd:
                    return "</th></tr>";
                default:
                    return input;
            }
        }
    }
}