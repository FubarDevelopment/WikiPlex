using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class TableMacro : IBlockMacro
    {
        public string Id
        {
            get { return "Table"; }
        }

        public string BlockStartScope
        {
            get { return ScopeName.TableBegin; }
        }

        public string BlockEndScope
        {
            get { return ScopeName.TableEnd; }
        }

        public string ItemEndScope
        {
            get { return ScopeName.TableRowEnd; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),

                               // table headers
                               new MacroRule(@"(^\|\|)", ScopeName.TableRowHeaderBegin),
                               new MacroRule(
                                   @"(?<=^\|\|.*)(?:(\|\|\s*?$)|(?:(\|\|)[^\|\n]*($)))",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.TableRowHeaderEnd },
                                           {2, ScopeName.TableCellHeader },
                                           {3, ScopeName.TableRowHeaderEnd }
                                       }),
                               new MacroRule(@"(?<=^\|\|.*)(\|\|)", ScopeName.TableCellHeader),

                               // table cells
                               new MacroRule(@"(^\|)(?!\|)", ScopeName.TableRowBegin),
                               new MacroRule(
                                   @"(?<=^\|.*)(?:(?<!\|)(\|\s*?$)|(?:(\|)[^\|\n]*($)))",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.TableRowEnd},
                                           {2, ScopeName.TableCell},
                                           {3, ScopeName.TableRowEnd}
                                       }),
                               new MacroRule(@"(?<=^\|.*)(?:(?<!\|)(\|)(?!\|$))", ScopeName.TableCell)
                           };
            }
        }
    }
}