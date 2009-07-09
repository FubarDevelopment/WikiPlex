using System;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class TableRenderer : IRenderer
    {
        public string Id
        {
            get { return "Table"; }
        }

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