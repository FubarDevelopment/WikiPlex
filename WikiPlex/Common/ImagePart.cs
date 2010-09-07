namespace WikiPlex.Common
{
    ///<summary>
    /// Defines a text, friendly text, and dimensions for an image.
    ///</summary>
    public class ImagePart : TextPart
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="ImagePart"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="friendlyText">The friendly text.</param>
        /// <param name="dimensions">The dimensions of the text.</param>
        public ImagePart(string text, string friendlyText, Dimensions dimensions) : base(text, friendlyText)
        {
            Dimensions = dimensions;
        }

        /// <summary>
        /// Gets the dimensions of the text.
        /// </summary>
        public Dimensions Dimensions { get; private set; }
    }
}
