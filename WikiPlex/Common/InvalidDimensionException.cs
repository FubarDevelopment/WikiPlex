using System;

namespace WikiPlex.Common
{
    /// <summary>
    /// The exception that is thrown when invalid dimensions are used during rendering.
    /// </summary>
    public class InvalidDimensionException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDimensionException"/> class.
        /// </summary>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="reason">The reason of why it is invalid.</param>
        public InvalidDimensionException(string paramName, string reason)
            : base(paramName)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets or sets the reason it is invalid.
        /// </summary>
        public string Reason { get; private set; }
    }
}