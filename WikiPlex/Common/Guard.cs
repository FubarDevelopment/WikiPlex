using System;
using System.Collections.Generic;
using System.Linq;

namespace WikiPlex.Common
{
    public static class Guard
    {
        public static void NotNull<T>(T @object, string paramName)
            where T : class
        {
            if (@object == null)
                throw new ArgumentNullException(paramName);
        }

        public static void NotNull<T>(T @object, string paramName, string message)
            where T : class
        {
            if (@object == null)
                throw new ArgumentNullException(paramName, message);
        }

        public static void NotNullOrEmpty(string @object, string paramName)
        {
            NotNull(@object, paramName);

            if (string.IsNullOrEmpty(@object))
                throw new ArgumentException("Parameter cannot be empty.", paramName);
        }

        public static void NotNullOrEmpty(string @object, string paramName, string message)
        {
            NotNull(@object, paramName, message);

            if (string.IsNullOrEmpty(@object))
                throw new ArgumentException(message, paramName);
        }

        public static void NotNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue> @object, string paramName)
        {
            NotNull(@object, paramName);

            if (@object.Count == 0)
                throw new ArgumentException("Parameter cannot be empty.", paramName);
        }

        public static void NotNullOrEmpty<T>(IEnumerable<T> @object, string paramName)
        {
            NotNull(@object, paramName);

            if (@object.Count() == 0)
                throw new ArgumentException("Parameter cannot be empty.", paramName);
        }

        public static void NotNullOrEmpty<T>(IList<T> @object, string paramName, string message)
        {
            NotNull(@object, paramName, message);

            if (@object.Count == 0)
                throw new ArgumentException(message, paramName);
        }

        public static void NotEqual<T>(T compareTo, T @object, string paramName)
            where T : IComparable
        {
            if (@object.CompareTo(compareTo) == 0)
                throw new ArgumentException("Parameter cannot equal '" + compareTo + "'.", paramName);
        }
    }
}