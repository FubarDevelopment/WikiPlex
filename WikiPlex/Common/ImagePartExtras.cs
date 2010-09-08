using System;

namespace WikiPlex.Common
{
    /// <summary>
    /// Contains the image part extras that should be extracted.
    /// </summary>
    [Flags]
    public enum ImagePartExtras
    {
        /// <summary>
        /// Default. This indicates no extras should be extracted.
        /// </summary>
        None = 0x0,
        
        /// <summary>
        /// Indicates the input contains text.
        /// </summary>
        ContainsText = 0x1,

        /// <summary>
        /// Indicates the input contains a link.
        /// </summary>
        ContainsLink = 0x2,

        /// <summary>
        /// Indicates that the input contains both text and link.
        /// </summary>
        All = 0x3
    }
}