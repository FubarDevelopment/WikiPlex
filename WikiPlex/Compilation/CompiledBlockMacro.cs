using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;

namespace WikiPlex.Compilation
{
    public class CompiledBlockMacro : CompiledMacro
    {
        public CompiledBlockMacro(string id, Regex regex, IList<string> captures, string blockStartScope,
                                  string blockEndScope, string itemEndScope)
            : base(id, regex, captures)
        {
            Guard.NotNullOrEmpty(blockStartScope, "blockStartScope");
            Guard.NotNullOrEmpty(blockEndScope, "blockEndScope");
            Guard.NotNullOrEmpty(itemEndScope, "itemEndScope");

            BlockStartScope = blockStartScope;
            BlockEndScope = blockEndScope;
            ItemEndScope = itemEndScope;
        }

        public string BlockStartScope { get; private set; }
        public string BlockEndScope { get; private set; }
        public string ItemEndScope { get; private set; }
    }
}