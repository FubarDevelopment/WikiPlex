namespace WikiPlex.Compilation.Macros
{
    /// <summary>
    /// Defines the fields necessary for a list-based macro.
    /// </summary>
    public interface IListMacro : IMacro
    {
        /// <summary>
        /// Gets the list start scope name.
        /// </summary>
        string ListStartScopeName { get; }

        /// <summary>
        /// Gets the list end scope name.
        /// </summary>
        string ListEndScopeName { get; }

        /// <summary>
        /// Gets the char used for depth (level) inspection.
        /// </summary>
        char DepthChar { get; }
    }
}