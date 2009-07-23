using System;

namespace WikiPlex.Syndication
{
    public class SyndicationDate
    {
        public string Value { get; private set; }

        public SyndicationDate(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            try
            {
                return DateTime.Parse(Value).ToLongDateString();
            }
            catch
            {
                return Value;
            }
        }
    }
}