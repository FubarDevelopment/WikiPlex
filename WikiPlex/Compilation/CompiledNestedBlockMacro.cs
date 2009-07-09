using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;

namespace WikiPlex.Compilation
{
    public class CompiledNestedBlockMacro : CompiledBlockMacro
    {
        public CompiledNestedBlockMacro(string id, Regex regex, IList<string> captures, string blockStartScope, string blockEndScope, string itemStartScope, string itemEndScope, Func<string, int> determineLevel) 
            : base(id, regex, captures, blockStartScope, blockEndScope, itemEndScope)
        {
            Guard.NotNullOrEmpty(itemStartScope, "itemStartScope");
            Guard.NotNull(determineLevel, "determineLevel");

            ItemStartScope = itemStartScope;
            DetermineLevel = determineLevel;
        }

        public string ItemStartScope { get; private set; }
        public Func<string, int> DetermineLevel { get; private set; }
    }
}