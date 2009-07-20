using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class SourceCodeMacro : IMacro
    {
        public string Id
        {
            get { return "SourceCode"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(@"(?s)(?:(?:{"").*?(?:""}))"),
                               new MacroRule(
                                   @"(?m)({{)(.*?)(}})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.SingleLineCode},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?s)({{(?:\s+\r?\n)?)((?>(?:(?!}}|{{).)*)(?>(?:{{(?>(?:(?!}}|{{).)*)}}(?>(?:(?!}}|{{).)*))*).*?)((?:\r?\n)?}})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.MultiLineCode},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*aspx c#\}\r?\n)(.*?)(\r?\n\{code:\s*aspx c#\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeAspxCs},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*aspx vb.net\}\r?\n)(.*?)(\r?\n\{code:\s*aspx vb.net\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeAspxVb},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*ashx\}\r?\n)(.*?)(\r?\n\{code:\s*ashx\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeAshx},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*c#\}\r?\n)(.*?)(\r?\n\{code:\s*c#\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeCSharp},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*vb.net\}\r?\n)(.*?)(\r?\n\{code:\s*vb.net\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeVbDotNet},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*html\}\r?\n)(.*?)(\r?\n\{code:\s*html\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeHtml},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*sql\}\r?\n)(.*?)(\r?\n\{code:\s*sql\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeSql},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*javascript\}\r?\n)(.*?)(\r?\n\{code:\s*javascript\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeJavaScript},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*xml\}\r?\n)(.*?)(\r?\n\{code:\s*xml\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeXml},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*php\}\r?\n)(.*?)(\r?\n\{code:\s*php\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodePhp},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:\s*css\}\r?\n)(.*?)(\r?\n\{code:\s*css\}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ColorCodeCss},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?si)(\{code:[^\}]+\}\r?\n)(.*?)(\r?\n\{code:[^\}]+}(?:\r?\n)?)",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.MultiLineCode},
                                           {3, ScopeName.Remove}
                                       })
                           };
            }
        }
    }
}