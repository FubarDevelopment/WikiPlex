using System.Web;
using WikiPlex.Common;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class TextFormattingRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.BoldBegin)]
            [InlineData(ScopeName.BoldEnd)]
            [InlineData(ScopeName.ItalicsBegin)]
            [InlineData(ScopeName.ItalicsEnd)]
            [InlineData(ScopeName.UnderlineBegin)]
            [InlineData(ScopeName.UnderlineEnd)]
            [InlineData(ScopeName.HeadingOneBegin)]
            [InlineData(ScopeName.HeadingOneEnd)]
            [InlineData(ScopeName.HeadingTwoBegin)]
            [InlineData(ScopeName.HeadingTwoEnd)]
            [InlineData(ScopeName.HeadingThreeBegin)]
            [InlineData(ScopeName.HeadingThreeEnd)]
            [InlineData(ScopeName.HeadingFourBegin)]
            [InlineData(ScopeName.HeadingFourEnd)]
            [InlineData(ScopeName.HeadingFiveBegin)]
            [InlineData(ScopeName.HeadingFiveEnd)]
            [InlineData(ScopeName.HeadingSixBegin)]
            [InlineData(ScopeName.HeadingSixEnd)]
            [InlineData(ScopeName.Remove)]
            [InlineData(ScopeName.StrikethroughBegin)]
            [InlineData(ScopeName.StrikethroughEnd)]
            [InlineData(ScopeName.SubscriptBegin)]
            [InlineData(ScopeName.SubscriptEnd)]
            [InlineData(ScopeName.SuperscriptBegin)]
            [InlineData(ScopeName.SuperscriptEnd)]
            [InlineData(ScopeName.HorizontalRule)]
            [InlineData(ScopeName.EscapedMarkup)]
            public void Should_be_able_to_resolve_scope_name(string scopeName)
            {
                var renderer = new TextFormattingRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Theory]
            [InlineData(ScopeName.BoldBegin, "b")]
            [InlineData(ScopeName.BoldEnd, "/b")]
            [InlineData(ScopeName.ItalicsBegin, "i")]
            [InlineData(ScopeName.ItalicsEnd, "/i")]
            [InlineData(ScopeName.UnderlineBegin, "u")]
            [InlineData(ScopeName.UnderlineEnd, "/u")]
            [InlineData(ScopeName.HeadingOneBegin, "h1")]
            [InlineData(ScopeName.HeadingTwoBegin, "h2")]
            [InlineData(ScopeName.HeadingThreeBegin, "h3")]
            [InlineData(ScopeName.HeadingFourBegin, "h4")]
            [InlineData(ScopeName.HeadingFiveBegin, "h5")]
            [InlineData(ScopeName.HeadingSixBegin, "h6")]
            [InlineData(ScopeName.StrikethroughBegin, "del")]
            [InlineData(ScopeName.StrikethroughEnd, "/del")]
            [InlineData(ScopeName.SubscriptBegin, "sub")]
            [InlineData(ScopeName.SubscriptEnd, "/sub")]
            [InlineData(ScopeName.SuperscriptBegin, "sup")]
            [InlineData(ScopeName.SuperscriptEnd, "/sup")]
            [InlineData(ScopeName.HorizontalRule, "hr /")]
            public void Should_resolve_the_scope_correctly_for_tags(string scopeName, string tagName)
            {
                var renderer = new TextFormattingRenderer();

                string result = renderer.Expand(scopeName, "in", x => x, x => x);

                Assert.Equal(string.Format("<{0}>", tagName), result);
            }

            [Theory]
            [InlineData(ScopeName.HeadingOneEnd, "h1")]
            [InlineData(ScopeName.HeadingTwoEnd, "h2")]
            [InlineData(ScopeName.HeadingThreeEnd, "h3")]
            [InlineData(ScopeName.HeadingFourEnd, "h4")]
            [InlineData(ScopeName.HeadingFiveEnd, "h5")]
            [InlineData(ScopeName.HeadingSixEnd, "h6")]
            public void Should_resolve_heading_scopes_correctly(string scopeName, string endTagName)
            {
                var renderer = new TextFormattingRenderer();

                string result = renderer.Expand(scopeName, "in", x => x, x => x);

                Assert.Equal(string.Format("</{0}>\r", endTagName), result);
            }

            [Fact]
            public void Should_resolve_the_remove_scope_correctly()
            {
                var renderer = new TextFormattingRenderer();

                string result = renderer.Expand(ScopeName.Remove, "in", x => x, x => x);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_resolve_the_escaped_markup_scope_correctly()
            {
                var renderer = new TextFormattingRenderer();

                string result = renderer.Expand(ScopeName.EscapedMarkup, "this is content", x => x, x => x);

                Assert.Equal("this is content", result);
            }

            [Fact]
            public void Should_resolve_the_escaped_markup_scope_correctly_and_html_encode_it()
            {
                var renderer = new TextFormattingRenderer();

                string result = renderer.Expand(ScopeName.EscapedMarkup, "this is &content", HttpUtility.HtmlEncode, x => x);

                Assert.Equal("this is &amp;content", result);
            }
        }
    }
}