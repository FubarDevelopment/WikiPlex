namespace WikiPlex.Compilation.Macros
{
    public interface IBlockMacro : IMacro
    {
        string BlockStartScope { get; }
        string BlockEndScope { get; }
        string ItemEndScope { get; }
    }
}