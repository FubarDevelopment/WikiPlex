using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WikiPlex.Common
{
    public static class Parameters
    {
        public static string ExtractUrl(ICollection<string> parameters)
        {
            string url = GetValue(parameters, "url");

            try
            {
                var parsedUrl = new Uri(url, UriKind.Absolute);
                url = parsedUrl.AbsoluteUri;
            }
            catch
            {
                throw new ArgumentException("Invalid parameter.", "url");
            }

            if (url.ToLower().Contains("codeplex.com"))
                throw new ArgumentException("Invalid parameter.", "url");

            return url;
        }

        public static HorizontalAlign ExtractAlign(ICollection<string> parameters, HorizontalAlign defaultValue)
        {
            string align;
            if (!TryGetValue(parameters, "align", out align))
                return defaultValue;

            if (!Utility.IsDefinedOnEnum<HorizontalAlign>(align))
                throw new ArgumentException("Invalid parameter.", "align");

            var alignment = (HorizontalAlign) Enum.Parse(typeof (HorizontalAlign), align, true);
            if (alignment != HorizontalAlign.Center && alignment != HorizontalAlign.Left && alignment != HorizontalAlign.Right)
                throw new ArgumentException("Invalid parameter.", "align");

            return alignment;
        }

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

        public static string GetValue(IEnumerable<string> parameters, string paramName)
        {
            string value;
            if (!TryGetValue(parameters, paramName, out value))
                throw new ArgumentException("Missing parameter.", paramName);

            return value;
        }

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