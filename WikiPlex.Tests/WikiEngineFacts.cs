using System;
using System.Collections.Generic;
using Moq;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting;
using Xunit;

namespace WikiPlex.Tests
{
    public class WikiEngineFacts
    {
        public class Render
        {
            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_null()
            {
                var engine = new WikiEngine();

                string result = engine.Render(null);

                Assert.Null(result);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_null_passing_macros()
            {
                var engine = new WikiEngine();
                var macro = new Mock<IMacro>();

                string result = engine.Render(null, new[] { macro.Object });

                Assert.Null(result);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_empty()
            {
                var engine = new WikiEngine();

                string result = engine.Render(string.Empty);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_empty_passing_macros()
            {
                var engine = new WikiEngine();
                var macro = new Mock<IMacro>();

                string result = engine.Render(string.Empty, new[] { macro.Object });

                Assert.Empty(result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null()
            {
                var engine = new WikiEngine();

                var ex = Record.Exception(() => engine.Render("foo", (IMacro[]) null));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macros", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty()
            {
                var engine = new WikiEngine();

                var ex = Record.Exception(() => engine.Render("foo", new IMacro[0]));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macros", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*_wiki_*");

                Assert.Equal("<b><i>wiki</i></b>", result);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully_using_only_the_specified_macro()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*_wiki_*", new[] { new BoldMacro() });

                Assert.Equal("<b>_wiki_</b>", result);
            }

            [Fact]
            public void Should_convert_line_breaks_to_html_break_tags()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*_text_\n+rendered+");

                Assert.Equal("<b>wiki</b><i>text</i><br /><u>rendered</u>", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_unordered_list()
            {
                var engine = new WikiEngine();

                string result = engine.Render("* wiki\n_text_");

                Assert.Equal("<ul><li>wiki</li></ul>\n<i>text</i>", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_ordered_list()
            {
                var engine = new WikiEngine();

                string result = engine.Render("# wiki\n_text_");

                Assert.Equal("<ol><li>wiki</li></ol>\n<i>text</i>", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_blockquote()
            {
                var engine = new WikiEngine();

                string result = engine.Render(": wiki\ntext");

                Assert.Equal("<blockquote>wiki</blockquote>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_breaks_into_html_break_tags_within_pre_tags()
            {
                var engine = new WikiEngine();

                string result = engine.Render("{{code\ncode}}");

                Assert.Equal("<pre>code\ncode</pre>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_one_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n! header");

                Assert.Equal("<b>wiki</b>\n<h1>header</h1>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_two_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n!! header");

                Assert.Equal("<b>wiki</b>\n<h2>header</h2>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_three_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n!!! header");

                Assert.Equal("<b>wiki</b>\n<h3>header</h3>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_four_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n!!!! header");

                Assert.Equal("<b>wiki</b>\n<h4>header</h4>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_five_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n!!!!! header");

                Assert.Equal("<b>wiki</b>\n<h5>header</h5>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_heading_six_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n!!!!!! header");

                Assert.Equal("<b>wiki</b>\n<h6>header</h6>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_horizontal_rule_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n----");

                Assert.Equal("<b>wiki</b>\n<hr />", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_after_a_horizontal_rule_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("----\n*wiki*");

                Assert.Equal("<hr />\n<b>wiki</b>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_unordered_list_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n* one");

                Assert.Equal("<b>wiki</b>\n<ul><li>one</li></ul>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_ordered_list_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("*wiki*\n# one");

                Assert.Equal("<b>wiki</b>\n<ol><li>one</li></ol>", result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_list_item_tag()
            {
                var engine = new WikiEngine();

                string result = engine.Render("### one\n# two");

                Assert.Equal("<ol><li>one\n</li></ol><ol><li>two</li></ol>", result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null_when_specifying_a_formatter()
            {
                var engine = new WikiEngine();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", null, formatter));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macros", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty_when_specifying_a_formatter()
            {
                var engine = new WikiEngine();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", new IMacro[0], formatter));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macros", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_formatter_is_null()
            {
                var engine = new WikiEngine();
                
                var ex = Record.Exception(() => engine.Render("*abc*", new[] { new TestMacro() }, (IFormatter) null));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("formatter", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_render_correctly_using_the_specific_formatter()
            {
                var engine = new WikiEngine();
                var renderer = new Mock<IRenderer>();
                renderer.Setup(x => x.CanExpand("Test")).Returns(true);
                renderer.Setup(x => x.Expand("Test", "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("Output");

                var formatter = new MacroFormatter(new[] {renderer.Object});
                string result = engine.Render("*abc*", new[] {new TestMacro()}, formatter);

                Assert.Equal("*Output*", result);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_empty_passing_a_formatter()
            {
                var engine = new WikiEngine();
                var macro = new Mock<IMacro>();
                var formatter = new Mock<IFormatter>();

                string result = engine.Render(string.Empty, new[] { macro.Object }, formatter.Object);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_convert_carriage_return_and_new_line_into_only_new_line_prior_to_parsing()
            {
                var engine = new WikiEngine();

                string result = engine.Render("before\r\nafter");

                Assert.Equal("before<br />after", result);
            }
        }

        private class TestMacro : IMacro
        {
            public string Id
            {
                get { return "Test"; }
            }

            public IList<MacroRule> Rules
            {
                get
                {
                    return new List<MacroRule>
                {
                    new MacroRule(
                        @"abc",
                        new Dictionary<int, string>
                        {
                            { 0, "Test" }
                        })
                };
                }
            }
        }
    }
}