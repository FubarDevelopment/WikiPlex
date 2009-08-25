using WikiPlex.Formatting;
using Xunit;
using Xunit.Extensions;

namespace WikiPlex.Tests.Formatting
{
    public class IndentationRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.IndentationBegin)]
            [InlineData(ScopeName.IndentationEnd)]
            public void Should_be_able_to_expand_scope_name(string scopeName)
            {
                var renderer = new IndentationRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_render_the_blockquote_begin_tag_correctly()
            {
                var renderer = new IndentationRenderer();

                string result = renderer.Expand(ScopeName.IndentationBegin, string.Empty, x => x, x => x);

                Assert.Equal("<blockquote>", result);
            }

            [Fact]
            public void Should_render_the_blockquote_end_tag_correctly()
            {
                var renderer = new IndentationRenderer();

                string result = renderer.Expand(ScopeName.IndentationEnd, string.Empty, x => x, x => x);

                Assert.Equal("</blockquote>", result);
            }
        }
    }
}