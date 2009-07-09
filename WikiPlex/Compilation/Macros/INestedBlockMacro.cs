using System;

namespace WikiPlex.Compilation.Macros
{
    public interface INestedBlockMacro : IBlockMacro
    {
        string ItemStartScope { get; }
        Func<string, int> DetermineLevel { get; }
    }
}