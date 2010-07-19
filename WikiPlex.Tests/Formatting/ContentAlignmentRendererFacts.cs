using WikiPlex.Formatting;
using Xunit;
using Xunit.Extensions;

namespace WikiPlex.Tests.Formatting
{
    public class ContentAlignmentRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.AlignEnd)]
            [InlineData(ScopeName.LeftAlign)]
            [InlineData(ScopeName.RightAlign)]
            public void Should_be_able_to_expand_scope_name(string scopeName)
            {
                var renderer = new ContentAlignmentRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_expand_the_alignment_end_tag_correctly()
            {
                var renderer = new ContentAlignmentRenderer();

                string result = renderer.Expand(ScopeName.AlignEnd, "in", x => x, x => x);

                Assert.Equal("</div><div style=\"clear:both;height:0;\">&nbsp;</div>", result);
            }

            [Theory]
            [InlineData(ScopeName.LeftAlign, "left")]
            [InlineData(ScopeName.RightAlign, "right")]
            public void Should_expand_the_text_alignment_correctly(string scopeName, string alignment)
            {
                var renderer = new ContentAlignmentRenderer();

                string result = renderer.Expand(scopeName, "in", x => x, x => x);

                Assert.Equal(string.Format("<div style=\"text-align:{0};float:{0};\">", alignment), result);
            }

            [Fact]
            public void Should_return_content_for_invalid_scope_name()
            {
                var renderer = new ContentAlignmentRenderer();

                string result = renderer.Expand("foo", "in", x => x, x => x);

                Assert.Equal("in", result);
            }
        }
    }
}