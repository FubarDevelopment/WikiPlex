using WikiPlex.Common;

namespace WikiPlex.Parsing
{
    public class Scope
    {
        public Scope(string name, int index, int length)
        {
            Guard.NotNullOrEmpty(name, "name");

            Name = name;
            Index = index;
            Length = length;
        }

        public Scope(string name, int index)
        {
            Guard.NotNullOrEmpty(name, "name");

            Name = name;
            Index = index;
        }

        public int Index { get; private set; }
        public int Length { get; private set; }
        public string Name { get; private set; }
    }
}