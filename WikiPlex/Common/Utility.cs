using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WikiPlex.Common
{
    /// <summary>
    /// The static utility class containing useful common methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Will extract the single text or pair text parts of a string, separated by a |.
        /// </summary>
        /// <param name="input">The input to inspect.</param>
        /// <returns>A new <see cref="TextPart"/>.</returns>
        /// <remarks>If there are 2 parts, the first is the text, and the second is the friendly text. Otherwise, only the text is set.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the input is empty.
        /// 
        /// -- or --
        /// 
        /// Thrown when there are more than 2 parts found.
        /// </exception>
        public static TextPart ExtractTextParts(string input)
        {
            Guard.NotNullOrEmpty(input, "input");
            string[] parts = input.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 2)
                throw new ArgumentException("Invalid number of parts.", "input");

            if (parts.Length == 1)
                return new TextPart(parts[0].Trim(), null);

            return new TextPart(parts[1].Trim(), parts[0].Trim());
        }

        /// <summary>
        /// Will extract the single text or pair text parts of a string, separated by a |, including the dimensions for the text part.
        /// </summary>
        /// <param name="input">The input to inspect.</param>
        /// <returns>A new <see cref="ImagePart"/>.</returns>
        /// <remarks>If there are 2 parts, the first is the text, and the second is the friendly text. Otherwise, only the text is set. Also, should the text contain dimensions, they will be set.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the input is empty.
        /// 
        /// -- or --
        /// 
        /// Thrown when there are more than 2 parts found.
        /// 
        /// -- or --
        /// 
        /// Thrown if the height/width is not a valid unit.
        /// 
        /// -- or --
        /// 
        /// Thrown when the height/width is less than or equal to zero.
        /// </exception>
        public static ImagePart ExtractImageParts(string input)
        {
            Guard.NotNullOrEmpty(input, "input");
            string[] parts = input.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 2)
                throw new ArgumentException("Invalid number of parts.", "input");

            string toParse = parts.Length == 1 ? parts[0].Trim() : parts[1].Trim();
            string friendly = parts.Length == 1 ? null : parts[0].Trim();
            string[] parameters = toParse.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            return new ImagePart(parameters[0], friendly, Parameters.ExtractDimensions(parameters));
        }

        /// <summary>
        /// Will count the number of occurences a character is in an string.
        /// </summary>
        /// <param name="toFind">The character to find.</param>
        /// <param name="input">The input string.</param>
        /// <returns>The number of occurrances.</returns>
        public static int CountChars(char toFind, string input)
        {
            return string.IsNullOrEmpty(input) 
                        ? 0 
                        : input.Count(c => c == toFind);
        }

        /// <summary>
        /// Will extract the first group fragment from an input based on a regular expression.
        /// </summary>
        /// <param name="regex">The regex to use.</param>
        /// <param name="input">The input to inspect.</param>
        /// <returns>The extracted fragment, empty string if not found.</returns>
        public static string ExtractFragment(Regex regex, string input)
        {
            Match match = regex.Match(input);
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;

            return string.Empty;
        }
    }
}