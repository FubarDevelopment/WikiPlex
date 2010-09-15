using System;
using System.Collections.Generic;
using System.Linq;
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
                var engine = TestableWikiEngine.Create();

                string result = engine.Render(null);

                Assert.Null(result);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_empty()
            {
                var engine = TestableWikiEngine.Create();

                string result = engine.Render(string.Empty);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null()
            {
                var engine = TestableWikiEngine.Create();

                var ex = Record.Exception(() => engine.Render("foo", (IMacro[]) null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty()
            {
                var engine = TestableWikiEngine.Create();

                var ex = Record.Exception(() => engine.Render("foo", new IMacro[0])) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_renderers_is_null()
            {
                var engine = TestableWikiEngine.Create();

                var ex = Record.Exception(() => engine.Render("foo", new[] { new TestMacro() }, (IEnumerable<IRenderer>)null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("renderers", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_renderers_is_empty()
            {
                var engine = TestableWikiEngine.Create();

                var ex = Record.Exception(() => engine.Render("foo", new[] {new TestMacro()}, new IRenderer[0])) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("renderers", ex.ParamName);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("output");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("output", result);
            }

            [Fact]
            public void Should_render_the_wiki_content_successfully_using_only_the_specified_macro()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("output");

                engine.Render("input", new[] { new BoldMacro() }, formatter.Object);

                engine.Parser.Verify(x => x.Parse("input", It.Is<IEnumerable<IMacro>>(m => (m.Count() == 1 && m.ElementAt(0) is BoldMacro)), ScopeAugmenters.All, It.IsAny<Action<IList<Scope>>>()));
            }

            [Fact]
            public void Should_convert_line_breaks_to_html_break_tags()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("text\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("text<br />text", result);
            }

            [Fact]
            public void Should_convert_encoded_line_breaks_to_html_break_tags()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("text&#10;text");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("text<br />text", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_unordered_list()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ul><li>text</li></ul>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ul><li>text</li></ul>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_ordered_list()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ol><li>text</li></ol>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ol><li>text</li></ol>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_break_into_html_break_tag_after_blockquote()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<blockquote>text</blockquote>\ntext");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<blockquote>text</blockquote>\ntext", result);
            }

            [Fact]
            public void Should_not_convert_line_breaks_prior_to_ending_a_blockquote()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<blockquote>text\n</blockquote>");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<blockquote>text\n</blockquote>", result);
            }

            [Fact]
            public void Should_not_convert_line_breaks_into_html_break_tags_within_pre_tags()
            {
                var engine = TestableWikiEngine.Create();
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
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                string expectation = "text\n" + html;
                formatter.Setup(x => x.Format("input")).Returns(expectation);

                string result = engine.Render("input", formatter.Object);

                Assert.Equal(expectation, result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_after_a_horizontal_rule_tag()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                string expectation = "<hr />\ntext";
                formatter.Setup(x => x.Format("input")).Returns(expectation);

                string result = engine.Render("input", formatter.Object);

                Assert.Equal(expectation, result);
            }

            [Fact]
            public void Should_not_add_a_html_break_tag_right_before_a_list_item_tag()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("input")).Returns("<ol><li>one\n</li></ol><ol><li>two</li></ol>");

                string result = engine.Render("input", formatter.Object);

                Assert.Equal("<ol><li>one\n</li></ol><ol><li>two</li></ol>", result);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_macros_is_null_when_specifying_a_formatter()
            {
                var engine = TestableWikiEngine.Create();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", null, formatter)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_macros_is_empty_when_specifying_a_formatter()
            {
                var engine = TestableWikiEngine.Create();
                var renderer = new Mock<IRenderer>();

                var formatter = new MacroFormatter(new[] { renderer.Object });
                var ex = Record.Exception(() => engine.Render("*abc*", new IMacro[0], formatter)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("macros", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_if_formatter_is_null()
            {
                var engine = TestableWikiEngine.Create();
                
                var ex = Record.Exception(() => engine.Render("*abc*", new[] { new TestMacro() }, (IFormatter) null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("formatter", ex.ParamName);
            }

            [Fact]
            public void Should_return_immediately_if_the_wiki_content_is_empty_passing_a_formatter()
            {
                var engine = TestableWikiEngine.Create();
                var macro = new Mock<IMacro>();
                var formatter = new Mock<IFormatter>();

                string result = engine.Render(string.Empty, new[] { macro.Object }, formatter.Object);

                Assert.Empty(result);
            }

            [Fact]
            public void Should_convert_carriage_return_and_new_line_into_only_new_line_prior_to_parsing()
            {
                var engine = TestableWikiEngine.Create();
                var formatter = new Mock<IFormatter>();
                formatter.Setup(x => x.Format("before\nafter")).Returns("before\nafter");

                string result = engine.Render("before\r\nafter");

                Assert.Equal("before<br />after", result);
            }
        }

        private class TestableWikiEngine : WikiEngine
        {
            public readonly Mock<IMacroParser> Parser;

            private TestableWikiEngine(Mock<IMacroParser> parser)
                : base(parser.Object)
            {
                Parser = parser;
            }

            public static TestableWikiEngine Create()
            {
                return new TestableWikiEngine(new Mock<IMacroParser>());
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