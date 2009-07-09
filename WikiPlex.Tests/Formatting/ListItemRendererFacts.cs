﻿using WikiPlex.Common;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class ListItemRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.OrderedListBeginTag)]
            [InlineData(ScopeName.OrderedListEndTag)]
            [InlineData(ScopeName.UnorderedListBeginTag)]
            [InlineData(ScopeName.UnorderedListEndTag)]
            [InlineData(ScopeName.ListItemBegin)]
            [InlineData(ScopeName.ListItemEnd)]
            public void Should_be_able_to_resolve_scope_name(string scopeName)
            {
                var renderer = new ListItemRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_render_the_ordered_list_begin_tag_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.OrderedListBeginTag, string.Empty, x => x, x => x);

                Assert.Equal("<ol><li>", result);
            }

            [Fact]
            public void Should_render_the_ordered_list_end_tag_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.OrderedListEndTag, string.Empty, x => x, x => x);

                Assert.Equal("</li></ol>", result);
            }

            [Fact]
            public void Should_render_the_unordered_list_begin_tag_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.UnorderedListBeginTag, string.Empty, x => x, x => x);

                Assert.Equal("<ul><li>", result);
            }

            [Fact]
            public void Should_render_the_unordered_list_end_tag_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.UnorderedListEndTag, string.Empty, x => x, x => x);

                Assert.Equal("</li></ul>", result);
            }

            [Fact]
            public void Should_render_the_list_item_begin_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.ListItemBegin, string.Empty, x => x, x => x);

                Assert.Equal("<li>", result);
            }

            [Fact]
            public void Should_render_the_list_item_end_scope_correctly()
            {
                var renderer = new ListItemRenderer();

                string result = renderer.Expand(ScopeName.ListItemEnd, string.Empty, x => x, x => x);

                Assert.Equal("</li>", result);
            }
        }
    }
}