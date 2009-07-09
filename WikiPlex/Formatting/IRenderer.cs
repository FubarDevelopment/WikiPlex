using System;

namespace WikiPlex.Formatting
{
    public interface IRenderer
    {
        string Id { get; }
        bool CanExpand(string scopeName);
        string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode);
    }
}