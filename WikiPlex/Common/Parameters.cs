﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WikiPlex.Common
{
    /// <summary>
    /// The static class to handle parameter extraction that is used across many renderers.
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// This will extract a url.
        /// </summary>
        /// <param name="parameters">The collection of parameters.</param>
        /// <returns>The extracted url.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the url cannot be validated.
        /// 
        /// -- or --
        /// 
        /// Thrown if the url contains codeplex.com
        /// </exception>
        public static string ExtractUrl(ICollection<string> parameters)
        {
            return ExtractUrl(parameters, true);
        }

        /// <summary>
        /// This will extract a url.
        /// </summary>
        /// <param name="parameters">The collection of parameters.</param>
        /// <param name="validateDomain">Will validate the domain not allowing codeplex.com</param>
        /// <returns>The extracted url.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the url cannot be validated.
        /// 
        /// -- or --
        /// 
        /// Thrown if the url contains codeplex.com and validateDomain is true.
        /// </exception>
        public static string ExtractUrl(ICollection<string> parameters, bool validateDomain)
        {
            string url = GetValue(parameters, "url");

            Uri parsedUrl;
            if (!Uri.TryCreate(url, UriKind.Absolute, out parsedUrl))
                throw new ArgumentException("Invalid parameter.", "url");
            url = parsedUrl.AbsoluteUri;

            if (validateDomain && url.ToLower().Contains("codeplex.com"))
                throw new ArgumentException("Invalid parameter.", "url");

            return url;
        }

        /// <summary>
        /// This will extract the horizontal alignment parameter.
        /// </summary>
        /// <param name="parameters">The collection of parameters.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The <see cref="HorizontalAlign"/> value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the alignment value is not a valid enum value.
        /// 
        /// -- or --
        /// 
        /// Thrown if the alignment is not Center, Left, or Right
        /// </exception>
        public static HorizontalAlign ExtractAlign(ICollection<string> parameters, HorizontalAlign defaultValue)
        {
            string align;
            if (!TryGetValue(parameters, "align", out align))
                return defaultValue;

            HorizontalAlign alignment;
            if (!Enum.TryParse(align, true, out alignment))
                throw new ArgumentException("Invalid parameter.", "align");

            if (alignment != HorizontalAlign.Center && alignment != HorizontalAlign.Left && alignment != HorizontalAlign.Right)
                throw new ArgumentException("Invalid parameter.", "align");

            return alignment;
        }

        /// <summary>
        /// This will extract the height and width dimensions parameters.
        /// </summary>
        /// <param name="parameters">The colleciton of parameters.</param>
        /// <param name="defaultHeight">The default height.</param>
        /// <param name="defaultWidth">The default width.</param>
        /// <returns>The <see cref="Dimensions"/> contained in the parameters.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the height/width is not a valid unit.
        /// 
        /// -- or --
        /// 
        /// Thrown if the height/width is less than or equal to zero.
        /// </exception>
        public static Dimensions ExtractDimensions(ICollection<string> parameters, Unit defaultHeight, Unit defaultWidth)
        {
            Unit height = ParseUnit(parameters, "height", defaultHeight);
            Unit width = ParseUnit(parameters, "width", defaultWidth);

            return new Dimensions {Height = height, Width = width};
        }

        private static Unit ParseUnit(IEnumerable<string> parameters, string paramName, Unit defaultValue)
        {
            string value;
            if (TryGetValue(parameters, paramName, out value))
            {
                try
                {
                    Unit unit = Unit.Parse(value);
                    if (unit.Value <= 0)
                        throw new ArgumentException("Invalid parameter.", paramName);

                    return unit;
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Invalid parameter.", paramName);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Will get the parameter value.
        /// </summary>
        /// <param name="parameters">The collection of parameters.</param>
        /// <param name="paramName">The parameter name to extract.</param>
        /// <returns>The parameter value.</returns>
        /// <exception cref="ArgumentException">Thrown if the paramName is not present in the collection of parameters.</exception>
        public static string GetValue(IEnumerable<string> parameters, string paramName)
        {
            string value;
            if (!TryGetValue(parameters, paramName, out value))
                throw new ArgumentException("Missing parameter.", paramName);

            return value;
        }

        /// <summary>
        /// Will get the parameter value.
        /// </summary>
        /// <param name="parameters">The collection of parameters.</param>
        /// <param name="paramName">The parameter name to extract.</param>
        /// <param name="value">The output value of the parameter name.</param>
        /// <returns>A boolean value indicating if the value was found or not.</returns>
        public static bool TryGetValue(IEnumerable<string> parameters, string paramName, out string value)
        {
            value = null;
            string paramValue = parameters.FirstOrDefault(s => s.StartsWith(paramName + "=", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(paramValue))
                return false;

            paramValue = paramValue.Substring(paramName.Length + 1);
            if (string.IsNullOrEmpty(paramValue))
                return false;

            value = paramValue;
            return true;
        }
    }
}