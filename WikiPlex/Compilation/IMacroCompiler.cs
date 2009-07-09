using WikiPlex.Compilation.Macros;

namespace WikiPlex.Compilation
{
    public interface IMacroCompiler
    {
        CompiledMacro Compile(IMacro macro);
    }
}