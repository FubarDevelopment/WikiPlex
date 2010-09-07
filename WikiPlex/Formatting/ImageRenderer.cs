using System;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render the image scopes.
    /// </summary>
    public class ImageRenderer : IRenderer
    {
        private const string ImageAndLink = "<a href=\"{2}\"><img style=\"border:none;\" src=\"{3}\" {4}/></a>";
        private const string ImageAndLinkWithStyle = "<div style=\"clear:both;height:0;\">&nbsp;</div><a style=\"float:{0};{1}\" href=\"{2}\"><img style=\"border:none;\" src=\"{3}\" {4}/></a>";
        private const string ImageLinkAndAlt = "<a href=\"{2}\"><img style=\"border:none;\" src=\"{3}\" alt=\"{4}\" title=\"{4}\" {5}/></a>";
        private const string ImageLinkAndAltWithStyle = "<div style=\"clear:both;height:0;\">&nbsp;</div><a style=\"float:{0};{1}\" href=\"{2}\"><img style=\"border:none;\" src=\"{3}\" alt=\"{4}\" title=\"{4}\" {5}/></a>";
        private const string ImageNoLink = "<img src=\"{2}\" {3}/>";
        private const string ImageNoLinkAndAlt = "<img src=\"{2}\" alt=\"{3}\" title=\"{3}\" {4}/>";
        private const string ImageNoLinkAndAltWithStyle = "<div style=\"clear:both;height:0;\">&nbsp;</div><img style=\"float:{0};{1}\" src=\"{2}\" alt=\"{3}\" title=\"{3}\" {4}/>";
        private const string ImageNoLinkWithStyle = "<div style=\"clear:both;height:0;\">&nbsp;</div><img style=\"float:{0};{1}\" src=\"{2}\" {3}/>";

        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "Image Renderer"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.ImageWithLinkNoAltLeftAlign
                    || scopeName == ScopeName.ImageWithLinkNoAltRightAlign
                    || scopeName == ScopeName.ImageWithLinkNoAlt
                    || scopeName == ScopeName.ImageWithLinkWithAltLeftAlign
                    || scopeName == ScopeName.ImageWithLinkWithAltRightAlign
                    || scopeName == ScopeName.ImageWithLinkWithAlt
                    || scopeName == ScopeName.ImageLeftAlign
                    || scopeName == ScopeName.ImageRightAlign
                    || scopeName == ScopeName.ImageNoAlign
                    || scopeName == ScopeName.ImageLeftAlignWithAlt
                    || scopeName == ScopeName.ImageRightAlignWithAlt
                    || scopeName == ScopeName.ImageNoAlignWithAlt);
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            try
            {
                switch (scopeName)
                {
                    case ScopeName.ImageLeftAlign:
                        return RenderImageNoLinkMacro(input, FloatAlignment.Left, attributeEncode);
                    case ScopeName.ImageRightAlign:
                        return RenderImageNoLinkMacro(input, FloatAlignment.Right, attributeEncode);
                    case ScopeName.ImageNoAlign:
                        return RenderImageNoLinkMacro(input, FloatAlignment.None, attributeEncode);
                    case ScopeName.ImageLeftAlignWithAlt:
                        return RenderImageWithAltMacro(input, FloatAlignment.Left, attributeEncode);
                    case ScopeName.ImageRightAlignWithAlt:
                        return RenderImageWithAltMacro(input, FloatAlignment.Right, attributeEncode);
                    case ScopeName.ImageNoAlignWithAlt:
                        return RenderImageWithAltMacro(input, FloatAlignment.None, attributeEncode);
                    case ScopeName.ImageWithLinkNoAlt:
                        return RenderImageWithLinkMacro(input, FloatAlignment.None, attributeEncode);
                    case ScopeName.ImageWithLinkNoAltLeftAlign:
                        return RenderImageWithLinkMacro(input, FloatAlignment.Left, attributeEncode);
                    case ScopeName.ImageWithLinkNoAltRightAlign:
                        return RenderImageWithLinkMacro(input, FloatAlignment.Right, attributeEncode);
                    case ScopeName.ImageWithLinkWithAlt:
                        return RenderImageWithLinkAndAltMacro(input, FloatAlignment.None, attributeEncode);
                    case ScopeName.ImageWithLinkWithAltLeftAlign:
                        return RenderImageWithLinkAndAltMacro(input, FloatAlignment.Left, attributeEncode);
                    case ScopeName.ImageWithLinkWithAltRightAlign:
                        return RenderImageWithLinkAndAltMacro(input, FloatAlignment.Right, attributeEncode);
                    default:
                        return input;
                }
            }
            catch
            {
                return RenderUnresolvedMacro();
            }
        }

        private static string RenderUnresolvedMacro()
        {
            return "<span class=\"unresolved\">Cannot resolve image macro, invalid number of parameters.</span>";
        }

        private static string RenderImageNoLinkMacro(string input, FloatAlignment alignment, Func<string, string> encode)
        {
            string format = alignment == FloatAlignment.None ? ImageNoLink : ImageNoLinkWithStyle;
            Dimensions dimensions;
            string url = ParseInput(input, out dimensions);

            return string.Format(format, alignment.GetStyle(), alignment.GetPadding(), encode(url), dimensions);
        }

        private static string RenderImageWithAltMacro(string input, FloatAlignment alignment, Func<string, string> encode)
        {
            TextPart part = Utility.ExtractTextParts(input);
            string format = alignment == FloatAlignment.None ? ImageNoLinkAndAlt : ImageNoLinkAndAltWithStyle;
            Dimensions dimensions;
            string url = ParseInput(part.Text, out dimensions);

            return string.Format(format, alignment.GetStyle(), alignment.GetPadding(), encode(url), encode(part.FriendlyText), dimensions);
        }

        private static string RenderImageWithLinkMacro(string input, FloatAlignment alignment, Func<string, string> encode)
        {
            TextPart part = Utility.ExtractTextParts(input);
            string format = alignment == FloatAlignment.None ? ImageAndLink : ImageAndLinkWithStyle;
            Dimensions dimensions;
            string url = ParseInput(part.FriendlyText, out dimensions);

            return string.Format(format, alignment.GetStyle(), alignment.GetPadding(), encode(part.Text), encode(url), dimensions);
        }

        private static string RenderImageWithLinkAndAltMacro(string input, FloatAlignment alignment, Func<string, string> encode)
        {
            string[] parts = input.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new ArgumentException();

            string format = alignment == FloatAlignment.None ? ImageLinkAndAlt : ImageLinkAndAltWithStyle;
            Dimensions dimensions;
            string url = ParseInput(parts[1].Trim(), out dimensions);

            return string.Format(format, alignment.GetStyle(), alignment.GetPadding(), encode(parts[2].Trim()), encode(url), encode(parts[0].Trim()), dimensions);
        }

        private static string ParseInput(string input, out Dimensions dimensions)
        {
            string[] parameters = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            dimensions = Parameters.ExtractDimensions(parameters);
            return parameters[0];
        }
    }
}