using WikiPlex.Common;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class ImageRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.ImageWithLinkNoAltLeftAlign)]
            [InlineData(ScopeName.ImageWithLinkNoAltRightAlign)]
            [InlineData(ScopeName.ImageWithLinkNoAlt)]
            [InlineData(ScopeName.ImageWithLinkWithAltLeftAlign)]
            [InlineData(ScopeName.ImageWithLinkWithAltRightAlign)]
            [InlineData(ScopeName.ImageWithLinkWithAlt)]
            [InlineData(ScopeName.ImageLeftAlign)]
            [InlineData(ScopeName.ImageRightAlign)]
            [InlineData(ScopeName.ImageNoAlign)]
            [InlineData(ScopeName.ImageLeftAlignWithAlt)]
            [InlineData(ScopeName.ImageRightAlignWithAlt)]
            [InlineData(ScopeName.ImageNoAlignWithAlt)]
            public void Should_be_able_to_resolve_scope_name(string scopeName)
            {
                var renderer = new ImageRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_render_the_image_tag_with_left_alignment_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageLeftAlign, "http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:left;padding-right:.5em;\" src=\"http://localhost/image.gif\" />", result);
            }

            [Fact]
            public void Should_render_the_image_tag_with_right_alignment_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageRightAlign, "http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:right;padding-left:.5em;\" src=\"http://localhost/image.gif\" />", result);
            }

            [Fact]
            public void Should_render_the_image_tag_with_no_alignment_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageNoAlign, "http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<img src=\"http://localhost/image.gif\" />", result);
            }

            [Fact]
            public void Should_render_the_image_tag_with_left_alignment_and_alt_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageLeftAlignWithAlt, "Friendly|http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:left;padding-right:.5em;\" src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" />", result);
            }

            [Fact]
            public void Should_render_the_image_tag_with_right_alignment_and_alt_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageRightAlignWithAlt, "Friendly|http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:right;padding-left:.5em;\" src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" />", result);
            }

            [Fact]
            public void Should_render_the_image_tag_with_no_alignment_and_alt_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageNoAlignWithAlt, "Friendly|http://localhost/image.gif", x => x, x => x);

                Assert.Equal("<img src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" />", result);
            }

            [Fact]
            public void Should_render_the_image_right_align_with_alt_scope_and_attribute_encode_the_source_and_alt()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageRightAlignWithAlt, "MyImage|http://localhost/image.gif", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:right;padding-left:.5em;\" src=\"safe!\" alt=\"safe!\" title=\"safe!\" />", result);
            }

            [Fact]
            public void Should_render_the_image_left_align_with_alt_scope_and_attribute_encode_the_source_and_alt()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageLeftAlignWithAlt, "MyImage|http://localhost/image.gif", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><img style=\"float:left;padding-right:.5em;\" src=\"safe!\" alt=\"safe!\" title=\"safe!\" />", result);
            }

            [Fact]
            public void Should_render_the_image_with_alt_scope_and_attribute_encode_the_source_and_alt()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageNoAlignWithAlt, "MyImage|http://localhost/image.gif", x => x, x => "safe!");

                Assert.Equal("<img src=\"safe!\" alt=\"safe!\" title=\"safe!\" />", result);
            }

            [Fact]
            public void Should_render_an_unresolved_macro_with_more_than_two_parts_without_link_indication_right_align()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageRightAlignWithAlt, "MyImage|http://localhost/image.gif|foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve image macro, invalid number of parameters.</span>", result);
            }

            [Fact]
            public void Should_render_an_unresolved_macro_with_more_than_two_parts_without_link_indication_left_align()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageLeftAlignWithAlt, "MyImage|http://localhost/image.gif|foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve image macro, invalid number of parameters.</span>", result);
            }

            [Fact]
            public void Should_render_an_unresolved_macro_with_more_than_two_parts_without_link_indication()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageNoAlignWithAlt, "MyImage|http://localhost/image.gif|foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve image macro, invalid number of parameters.</span>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_scope_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAlt, "http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<a href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_scope_and_attribute_encode_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAlt, "http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<a href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_left_aligned_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAltLeftAlign, "http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:left;padding-right:.5em;\" href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_left_aligned_and_attribute_encode_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAltLeftAlign, "http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:left;padding-right:.5em;\" href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_right_aligned_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAltRightAlign, "http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:right;padding-left:.5em;\" href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_no_alt_right_aligned_and_attribute_encode_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkNoAltRightAlign, "http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:right;padding-left:.5em;\" href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_scope_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAlt, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<a href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_scope_and_attribute_encode_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAlt, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<a href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" alt=\"safe!\" title=\"safe!\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_left_aligned_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAltLeftAlign, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:left;padding-right:.5em;\" href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_left_aligned_and_attribute_encode_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAltLeftAlign, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:left;padding-right:.5em;\" href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" alt=\"safe!\" title=\"safe!\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_right_aligned_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAltRightAlign, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => x);

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:right;padding-left:.5em;\" href=\"http://localhost\"><img style=\"border:none;\" src=\"http://localhost/image.gif\" alt=\"Friendly\" title=\"Friendly\" /></a>", result);
            }

            [Fact]
            public void Should_render_the_image_with_link_with_alt_right_aligned_and_attribute_encoded_correctly()
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(ScopeName.ImageWithLinkWithAltRightAlign, "Friendly|http://localhost/image.gif|http://localhost", x => x, x => "safe!");

                Assert.Equal("<div style=\"clear:both;\"></div><a style=\"float:right;padding-left:.5em;\" href=\"safe!\"><img style=\"border:none;\" src=\"safe!\" alt=\"safe!\" title=\"safe!\" /></a>", result);
            }

            [Theory]
            [InlineData(ScopeName.ImageWithLinkWithAlt)]
            [InlineData(ScopeName.ImageWithLinkWithAltLeftAlign)]
            [InlineData(ScopeName.ImageWithLinkWithAltRightAlign)]
            public void Should_render_an_unresolved_macro_when_more_than_three_parts_exist(string scopeName)
            {
                var renderer = new ImageRenderer();

                string result = renderer.Expand(scopeName, "a|b|c|d", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve image macro, invalid number of parameters.</span>", result);
            }
        }
    }
}