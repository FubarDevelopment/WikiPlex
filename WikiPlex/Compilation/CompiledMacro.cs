using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;

namespace WikiPlex.Compilation
{
    public class CompiledMacro
    {
        public CompiledMacro(string id, Regex regex, IList<string> captures)
        {
            Guard.NotNullOrEmpty(id, "id");
            Guard.NotNull(regex, "regex");
            Guard.NotNullOrEmpty(captures, "captures");

            Id = id;
            Regex = regex;
            Captures = captures;
        }

        public string Id { get; private set; }       
        public Regex Regex { get; private set; }
        public IList<string> Captures { get; private set; }
    }
}