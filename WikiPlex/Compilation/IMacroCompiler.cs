using WikiPlex.Compilation.Macros;

namespace WikiPlex.Compilation
{
    /// <summary>
    /// Defines the <see cref="MacroCompiler"/> contract.
    /// </summary>
    public interface IMacroCompiler
    {
        /// <summary>
        /// Will compile a new <see cref="IMacro"/> or return a previously cached <see cref="CompiledMacro"/>.
        /// </summary>
        /// <param name="macro">The macro to compile.</param>
        /// <returns>The compiled macro.</returns>
        CompiledMacro Compile(IMacro macro);
    }
}