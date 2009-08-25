using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WikiPlex.Common;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting;
using WikiPlex.Parsing;

namespace WikiPlex
{
    public class WikiEngine : IWikiEngine
    {
        private static readonly MacroCompiler compiler = new MacroCompiler();
        private static readonly Regex NewLineRegex = new Regex(@"(?<!\r|</tr>|</li>|</ul>|</ol>|<hr />|</blockquote>)\n(?!<h[1-6]>|<hr />|<ul>|<ol>|</li>)", RegexOptions.Compiled);
        private static readonly Regex PreRegex = new Regex(@"(?s)((?><pre>)(?>.*?</pre>))", RegexOptions.Compiled);

        public string Render(string wikiContent)
        {
            var formatter = new MacroFormatter(Renderers.All);
            return Render(wikiContent, formatter);
        }

        public string Render(string wikiContent, IFormatter formatter)
        {
            return Render(wikiContent, Macros.All, formatter);
        }

        public string Render(string wikiContent, IEnumerable<IMacro> macros)
        {
            var formatter = new MacroFormatter(Renderers.All);
            return Render(wikiContent, macros, formatter);
        }

        public string Render(string wikiContent, IEnumerable<IMacro> macros, IFormatter formatter)
        {
            Guard.NotNullOrEmpty(macros, "macros");
            Guard.NotNull(formatter, "formatter");

            if (string.IsNullOrEmpty(wikiContent))
                return wikiContent;

            wikiContent = wikiContent.Replace("\r\n", "\n");

            var parser = new MacroParser(compiler);
            var buffer = new StringBuilder(wikiContent.Length);

            parser.Parse(wikiContent, macros, ScopeAugmenters.All, formatter.RecordParse);

            formatter.Format(wikiContent, buffer);

            return ReplaceNewLines(buffer.ToString());
        }

        private static string ReplaceNewLines(string input)
        {
            string replacedInput = NewLineRegex.Replace(input, "<br />");

            var match = PreRegex.Match(replacedInput);
            if (!match.Success)
                return replacedInput;

            // now we need to remove any <br /> tags within <pre></pre> tags
            var output = new StringBuilder(input.Length);
            int currentIndex = 0;
            while (match.Success)
            {
                output.Append(replacedInput.Substring(currentIndex, match.Groups[0].Index - currentIndex));
                output.Append(replacedInput.Substring(match.Groups[0].Index, match.Groups[0].Length).Replace("<br />", "\n"));
                currentIndex = match.Groups[0].Index + match.Groups[0].Length;
                match = match.NextMatch();
            }

            output.Append(replacedInput.Substring(currentIndex));

            return output.ToString();
        }
    }
}