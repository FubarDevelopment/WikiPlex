using Xunit;
using Xunit.Extensions;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Tests.Compilation.Macros
{
    public class UnorderedListMacroFacts
    {
        [Theory]
        [InlineData(1, "* ")]
        [InlineData(2, "** ")]
        [InlineData(3, "*** ")]
        public void Should_correctly_determine_the_level(int expectedLevel, string input)
        {
            var macro = new UnorderedListMacro();

            int result = macro.DetermineLevel(input);

            Assert.Equal(expectedLevel, result);
        }
    }
}