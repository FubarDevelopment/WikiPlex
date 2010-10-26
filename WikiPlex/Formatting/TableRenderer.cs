﻿using System;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render all table based scopes.
    /// </summary>
    public class TableRenderer : RendererBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TableRenderer"/> class.
        /// </summary>
        public TableRenderer()
            : base(ScopeName.TableBegin, ScopeName.TableCell, ScopeName.TableCellHeader,
                   ScopeName.TableEnd, ScopeName.TableRowBegin, ScopeName.TableRowEnd,
                   ScopeName.TableRowHeaderBegin, ScopeName.TableRowHeaderEnd)
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
                    return null;
            }
        }
    }
}