using System;

namespace WikiPlex.Common
{
    public class InvalidDimensionException : ArgumentOutOfRangeException
    {
        public InvalidDimensionException(string paramName, string reason)
            : base(paramName)
        {
            Reason = reason;
        }

        public string Reason
        {
            get;
            set;
        }
    }
}