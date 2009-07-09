using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation
{
    public class MacroRule
    {
        public MacroRule(string regex)
            : this(regex, new Dictionary<int, string>())
        {
        }

        public MacroRule(string regex, string firstScopeCapture)
        {
            Guard.NotNullOrEmpty(regex, "regex");
            Guard.NotNullOrEmpty(firstScopeCapture, "firstScopeCapture");

            Regex = regex;
            Captures = new Dictionary<int, string>
                           {
                               { 1, firstScopeCapture }
                           };
        }

        public MacroRule(string regex, IDictionary<int, string> captures)
        {
            Guard.NotNullOrEmpty(regex, "regex");
            Guard.NotNull(captures, "captures");

            Regex = regex;
            Captures = captures;
        }

        public string Regex { get; set; }
        public IDictionary<int, string> Captures { get; private set; }
    }
}