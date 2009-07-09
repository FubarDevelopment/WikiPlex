using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using WikiPlex.Formatting;
using WikiPlex.Parsing;

namespace WikiPlex.Tests.Formatting
{
    public class MacroFormatterFacts
    {
        public class Format
        {
            [Fact]
            public void Will_format_a_list_of_scopes_in_order()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, "ab", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>ab</b>");
                renderer.Setup(x => x.Expand(scope, "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>abc</b>");
                var formatter = new MacroFormatter(new[] {renderer.Object});
                var scopes = new List<Scope> {new Scope(scope, 7, 3), new Scope(scope, 4, 2)};
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the ab abc code", writer);

                Assert.Equal("the <b>ab</b> <b>abc</b> code", writer.ToString());
            }

            [Fact]
            public void Will_format_the_string_when_it_cannot_resolve_a_macro()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(false);
                var formatter = new MacroFormatter(new[] {renderer.Object});
                var scopes = new List<Scope> {new Scope(scope, 4, 2)};
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the ab abc code", writer);

                Assert.Equal("the <span class=\"unresolved\">Cannot resolve macro, as no renderers were found.</span>[ab] abc code", writer.ToString());
            }

            [Fact]
            public void Will_html_encode_the_content_when_it_cannot_resolve_a_macro()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(false);
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 4, 3) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the &ab abc code", writer);

                Assert.Equal("the <span class=\"unresolved\">Cannot resolve macro, as no renderers were found.</span>[&amp;ab] abc code", writer.ToString());
            }

            [Fact]
            public void Will_remove_the_line_break_at_the_end_of_the_formatted_text()
            {
                var renderer = new Mock<IRenderer>();
                renderer.Setup(x => x.CanExpand(It.IsAny<string>())).Returns(true);
                renderer.Setup(x => x.Expand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<h1>end</h1>\r");
                var formatter = new MacroFormatter(new[] {renderer.Object});
                var scopes = new List<Scope> {new Scope("foo", 0, 1)};
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("f", writer);

                Assert.Equal("<h1>end</h1>", writer.ToString());
            }

            [Fact]
            public void Will_skip_nested_scopes()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Verify(x => x.Expand(scope, "bc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>()), Times.Never());
                renderer.Setup(x => x.Expand(scope, "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("notskipped");
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 4, 3), new Scope(scope, 5, 2) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the abc code", writer);

                Assert.Equal("the notskipped code", writer.ToString());
                renderer.Verify();
            }

            [Fact]
            public void Will_not_skip_scopes_with_same_index_and_length()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Verify(x => x.Expand(scope, "bc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>()), Times.Never());
                renderer.Setup(x => x.Expand(scope, "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("notskipped");
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 4, 3), new Scope(scope, 4, 3) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the abc code", writer);

                Assert.Equal("the notskippednotskipped code", writer.ToString());
                renderer.Verify();
            }

            [Fact]
            public void Will_html_encode_the_text_between_scopes()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, "ab", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>ab</b>");
                renderer.Setup(x => x.Expand(scope, "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>abc</b>");
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 8, 3), new Scope(scope, 5, 2) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the& ab abc code", writer);

                Assert.Equal("the&amp; <b>ab</b> <b>abc</b> code", writer.ToString());
            }

            [Fact]
            public void Will_html_encode_the_text_when_writing_to_the_end()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, "ab", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>ab</b>");
                renderer.Setup(x => x.Expand(scope, "abc", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns("<b>abc</b>");
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 7, 3), new Scope(scope, 4, 2) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("the ab abc &code", writer);

                Assert.Equal("the <b>ab</b> <b>abc</b> &amp;code", writer.ToString());
            }

            [Fact]
            public void Will_capture_any_exception_when_resolving_a_scope_and_render_it_as_an_unresolved_macro()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, "ab", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Throws<Exception>();
                var formatter = new MacroFormatter(new[] {renderer.Object});
                var scopes = new List<Scope> {new Scope(scope, 0, 2)};
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("ab", writer);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve macro, as an unhandled exception occurred.</span>[ab]", writer.ToString());
            }

            [Fact]
            public void Will_html_encode_content_while_capturing_any_exception_when_resolving_a_scope_and_render_it_as_an_unresolved_macro()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, "&ab", It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Throws<Exception>();
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var scopes = new List<Scope> { new Scope(scope, 0, 3) };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();

                formatter.Format("&ab", writer);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve macro, as an unhandled exception occurred.</span>[&amp;ab]", writer.ToString());
            }

            [Fact]
            public void Will_raise_ScopeRendered_event_correctly()
            {
                var renderer = new Mock<IRenderer>();
                const string scope = "scope";
                const string expectedContent = "ab";
                renderer.Setup(x => x.CanExpand(scope)).Returns(true);
                renderer.Setup(x => x.Expand(scope, expectedContent, It.IsAny<Func<string, string>>(), It.IsAny<Func<string, string>>())).Returns(expectedContent);
                var formatter = new MacroFormatter(new[] { renderer.Object });
                var expectedScope = new Scope(scope, 0, 2);
                var scopes = new List<Scope> { expectedScope };
                formatter.RecordParse(scopes);
                var writer = new StringBuilder();
                Scope actualScope = null;
                string actualContent = null;

                formatter.ScopeRendered += delegate(object obj, RenderedScopeEventArgs e)
                                               {
                                                   actualContent = e.Content;
                                                   actualScope = e.Scope;
                                               };
                formatter.Format(expectedContent, writer);

                Assert.Equal(expectedScope, actualScope);
                Assert.Equal(expectedContent, actualContent);
            }
        }
    }
}