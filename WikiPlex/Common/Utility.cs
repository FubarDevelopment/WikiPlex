using System;
using System.Linq;

namespace WikiPlex.Common
{
    public static class Utility
    {
        public static bool IsDefinedOnEnum<T>(object input)
        {
            Type type = typeof(T);
            Guard.NotEqual(false, type.IsEnum, "type");

            if (Enum.IsDefined(type, input))
                return true;

            return Enum.GetNames(type)
                       .Where(n => string.Compare(input.ToString(), n, StringComparison.OrdinalIgnoreCase) == 0)
                       .Count() == 1;
        }

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

        public static int CountChars(char toFind, string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            int count = 0;
            foreach (char c in input)
            {
                if (c == toFind)
                    count++;
            }

            return count;
        }
    }
}