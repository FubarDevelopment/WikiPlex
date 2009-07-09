using System.Web;
using WikiPlex.Common;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class LinkRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.LinkNoText)]
            [InlineData(ScopeName.LinkWithText)]
            [InlineData(ScopeName.LinkAsMailto)]
            [InlineData(ScopeName.Anchor)]
            [InlineData(ScopeName.LinkToAnchor)]
            public void Should_be_able_to_resolve_scope_name(string scopeName)
            {
                var renderer = new LinkRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_render_the_link_with_no_text_scope_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkNoText, "http://localhost", x => x, x => x);

                Assert.Equal("<a href=\"http://localhost\" class=\"externalLink\">http://localhost<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_render_the_link_with_text_scope_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkWithText, "Local|http://localhost", x => x, x => x);

                Assert.Equal("<a href=\"http://localhost\" class=\"externalLink\">Local<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_render_the_link_with_text_scope_and_correctly_encoding_the_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkWithText, "&Local|http://localhost", HttpUtility.HtmlEncode, x => x);

                Assert.Equal("<a href=\"http://localhost\" class=\"externalLink\">&amp;Local<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_render_the_link_as_mailto_scope_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkAsMailto, "someone@local.com", x => x, x => x);

                Assert.Equal("<a href=\"mailto:someone@local.com\" class=\"externalLink\">someone@local.com<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_render_the_anchor_scope_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.Anchor, "something", x => x, x => x);

                Assert.Equal("<a name=\"something\"></a>", result);
            }

            [Fact]
            public void Should_render_the_anchor_scope_correctly_encoding_the_attribute()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.Anchor, "&something", HttpUtility.HtmlEncode, HttpUtility.HtmlAttributeEncode);

                Assert.Equal("<a name=\"&amp;something\"></a>", result);
            }

            [Fact]
            public void Should_render_the_link_to_anchor_scope_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkToAnchor, "something", x => x, x => x);

                Assert.Equal("<a href=\"#something\">something</a>", result);
            }

            [Fact]
            public void Should_render_the_link_to_anchor_scope_correctly_and_encoding_the_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkToAnchor, "&something", HttpUtility.HtmlEncode, HttpUtility.HtmlAttributeEncode);

                Assert.Equal("<a href=\"#&amp;something\">&amp;something</a>", result);
            }

            [Fact]
            public void Should_render_an_unresolved_macro_if_input_has_more_than_two_parts()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkWithText, "a|b|c", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve link macro, invalid number of parameters.</span>", result);
            }

            [Fact]
            public void Should_render_a_correct_link_if_not_prefixed_with_http_and_no_friendly_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkNoText, "www.microsoft.com", x => x, x => x);

                Assert.Equal("<a href=\"http://www.microsoft.com\" class=\"externalLink\">www.microsoft.com<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_render_a_correct_link_if_not_prefixed_with_http_and_with_friendly_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkWithText, "Test|www.microsoft.com", x => x, x => x);

                Assert.Equal("<a href=\"http://www.microsoft.com\" class=\"externalLink\">Test<span class=\"externalLinkIcon\"></span></a>", result);   
            }

            [Fact]
            public void Should_render_a_correct_mailto_link_if_prefixed_with_mailto_and_with_friendly_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkWithText, "Test|mailto:test@user.com", x => x, x => x);

                Assert.Equal("<a href=\"mailto:test@user.com\" class=\"externalLink\">Test<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_trim_url_when_link_has_no_text()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkNoText, " www.microsoft.com", x => x, x => x);

                Assert.Equal("<a href=\"http://www.microsoft.com\" class=\"externalLink\">www.microsoft.com<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_trim_mailto_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkAsMailto, " test@test.com ", x => x, x => x);

                Assert.Equal("<a href=\"mailto:test@test.com\" class=\"externalLink\">test@test.com<span class=\"externalLinkIcon\"></span></a>", result);
            }

            [Fact]
            public void Should_trim_anchor_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.Anchor, " test ", x => x, x => x);

                Assert.Equal("<a name=\"test\"></a>", result);
            }

            [Fact]
            public void Should_trim_link_to_anchor_correctly()
            {
                var renderer = new LinkRenderer();

                string result = renderer.Expand(ScopeName.LinkToAnchor, " test ", x => x, x => x);

                Assert.Equal("<a href=\"#test\">test</a>", result);
            }
        }
    }
}