using System;
using System.Collections.Generic;
using Moq;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting;
using WikiPlex.Parsing;
using Xunit;
using Xunit.Extensions;

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
            public void Should_return_immediately_if_the_wiki_content_is_empty()
            {
                var engine = new WikiEngine();

                string result = engine.Render(string.Empty);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null()
            {
                var engine = new WikiEngine();

                var ex = Record.Exception(() => engine.Render("foo", (IMacro[]) null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty()
            {
                var engine = new WikiEngine();

                var ex = Record.Exception(() => engine.Render("foo", new IMacro[0])) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("*_wiki_*")).Returns("<b><i>wiki</i></b>");

                string result = engine.Render("*_wiki_*", formatter.Object);

                Assert.Equal("<b><i>wiki</i></b>", result);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully_using_only_the_specified_macro()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                var scopes = new List<Scope>();
                formatter.Setup(x => x.Format("*_wiki_*")).Returns("<b>_wiki_</b>");
                formatter.Setup(x => x.RecordParse(It.IsAny<IList<Scope>>())).Callback<IList<Scope>>(scopes.AddRange);

                engine.Render("*_wiki_*", new[] { new BoldMacro() }, formatter.Object);

                Assert.Equal(2, scopes.Count);
                Assert.Equal(ScopeName.BoldBegin, scopes[0].Name);
                Assert.Equal(ScopeName.BoldEnd, scopes[1].Name);
            }

            [Fact]
            public void Should_convert_line_breaks_to_html_break_tags()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("text\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("text<br />text", result);
            }

            [Fact]
            public void Should_convert_encoded_line_breaks_to_html_break_tags()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("text&#10;text");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("text<br />text", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_unordered_list()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ul><li>text</li></ul>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ul><li>text</li></ul>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_ordered_list()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ol><li>text</li></ol>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ol><li>text</li></ol>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_blockquote()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<blockquote>text</blockquote>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<blockquote>text</blockquote>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_breaks_prior_to_ending_a_blockquote()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<blockquote>text\n</blockquote>");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<blockquote>text\n</blockquote>", result);
            }

            [Fact]
            public void Should_not_convert_line_breaks_into_html_break_tags_within_pre_tags()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<pre>code\ncode</pre>");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<pre>code\ncode</pre>", result);
            }

            [Theory]
            [InlineData("<h1>text</h1>")]
            [InlineData("<h2>text</h2>")]
            [InlineData("<h3>text</h3>")]
            [InlineData("<h4>text</h4>")]
            [InlineData("<h5>text</h5>")]
            [InlineData("<h6>text</h6>")]
            [InlineData("<hr />")]
            [InlineData("<ul><li>one</li></ul>")]
            [InlineData("<ol><li>one</li></ol>")]
            public void Should_not_add_a_html_break_tag_right_before_a_tag(string html)
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                string expectation = "text\n" + html;
                formatter.Setup(x => x.Format("input")).Returns(expectation);

                string result = engine.Render("input", formatter.Object);

                Assert.Equal(expectation, result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_after_a_horizontal_rule_tag()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                string expectation = "<hr />\ntext";
                formatter.Setup(x => x.Format("input")).Returns(expectation);

                string result = engine.Render("input", formatter.Object);

                Assert.Equal(expectation, result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_list_item_tag()
            {
                var engine = new WikiEngine();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ol><li>one\n</li></ol><ol><li>two</li></ol>");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ol><li>one\n</li></ol><ol><li>two</li></ol>", result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null_when_specifying_a_formatter()
            {
                var engine = new WikiEngine();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", null, formatter)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty_when_specifying_a_formatter()
            {
                var engine = new WikiEngine();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", new IMacro[0], formatter)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_formatter_is_null()
            {
                var engine = new WikiEngine();
                
                var ex = Record.Exception(() => engine.Render("*abc*", new[] { new TestMacro() }, null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("formatter", ex.ParamName);
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
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("before\nafter")).Returns("before\nafter");

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