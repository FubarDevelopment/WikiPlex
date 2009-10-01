﻿using System.Web;
using ColorCode;
using ColorCode.Common;
using Moq;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class SourceCodeRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.SingleLineCode)]
            [InlineData(ScopeName.MultiLineCode)]
            [InlineData(ScopeName.ColorCodeAshx)]
            [InlineData(ScopeName.ColorCodeAspxCs)]
            [InlineData(ScopeName.ColorCodeAspxVb)]
            [InlineData(ScopeName.ColorCodeCpp)]
            [InlineData(ScopeName.ColorCodeCSharp)]
            [InlineData(ScopeName.ColorCodeHtml)]
            [InlineData(ScopeName.ColorCodeJavaScript)]
            [InlineData(ScopeName.ColorCodeSql)]
            [InlineData(ScopeName.ColorCodeVbDotNet)]
            [InlineData(ScopeName.ColorCodeXml)]
            [InlineData(ScopeName.ColorCodePhp)]
            [InlineData(ScopeName.ColorCodeCss)]
            [InlineData(ScopeName.ColorCodeJava)]
            public void Should_be_able_to_resolve_scope_name(string scopeName)
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Should_resolve_the_single_line_code_scope_correctly()
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                string result = renderer.Expand(ScopeName.SingleLineCode, "public class { }", x => x, x => x);

                Assert.Equal("<span class=\"codeInline\">public class { }</span>", result);
            }

            [Fact]
            public void Should_resolve_the_single_line_code_scope_correctly_and_html_encode_the_content()
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                string result = renderer.Expand(ScopeName.SingleLineCode, "&public class { }", HttpUtility.HtmlEncode, x => x);

                Assert.Equal("<span class=\"codeInline\">&amp;public class { }</span>", result);
            }

            [Fact]
            public void Should_resolve_the_multi_line_code_scope_correctly()
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                string result = renderer.Expand(ScopeName.MultiLineCode, "public class { }", x => x, x => x);

                Assert.Equal("<pre>public class { }</pre>", result);
            }

            [Fact]
            public void Should_resolve_the_multi_line_code_scope_correctly_and_html_encode_the_content()
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                string result = renderer.Expand(ScopeName.MultiLineCode, "public &class { }", HttpUtility.HtmlEncode, x => x);

                Assert.Equal("<pre>public &amp;class { }</pre>", result);
            }

            [Fact]
            public void Should_remove_the_new_line_at_the_end_of_a_multi_line_code()
            {
                var renderer = new SourceCodeRenderer(new Mock<ICodeColorizer>().Object);

                string result = renderer.Expand(ScopeName.MultiLineCode, "public class { }\r\n", x => x, x => x);

                Assert.Equal("<pre>public class { }</pre>", result);
            }

            [Theory]
            [InlineData(LanguageId.Ashx, ScopeName.ColorCodeAshx)]
            [InlineData(LanguageId.AspxCs, ScopeName.ColorCodeAspxCs)]
            [InlineData(LanguageId.AspxVb, ScopeName.ColorCodeAspxVb)]
            [InlineData(LanguageId.Cpp, ScopeName.ColorCodeCpp)]
            [InlineData(LanguageId.CSharp, ScopeName.ColorCodeCSharp)]
            [InlineData(LanguageId.Html, ScopeName.ColorCodeHtml)]
            [InlineData(LanguageId.JavaScript, ScopeName.ColorCodeJavaScript)]
            [InlineData(LanguageId.Sql, ScopeName.ColorCodeSql)]
            [InlineData(LanguageId.VbDotNet, ScopeName.ColorCodeVbDotNet)]
            [InlineData(LanguageId.Xml, ScopeName.ColorCodeXml)]
            [InlineData(LanguageId.Php, ScopeName.ColorCodePhp)]
            [InlineData(LanguageId.Css, ScopeName.ColorCodeCss)]
            [InlineData(LanguageId.Java, ScopeName.ColorCodeJava)]
            public void Should_resolve_the_color_code_scope_correctly(string languageId, string scopeName)
            {
                var colorizer = new Mock<ICodeColorizer>();
                var renderer = new SourceCodeRenderer(colorizer.Object);
                colorizer.Setup(x => x.Colorize("I am not colorized.", Languages.FindById(languageId))).Returns("I am colorized!");

                string result = renderer.Expand(scopeName, "I am not colorized.", x => x, x => x);

                Assert.Equal("I am colorized!", result);
            }

            [Fact]
            public void Should_return_content_for_invalid_scope_name()
            {
                var renderer = new SourceCodeRenderer();

                string result = renderer.Expand("foo", "in", x => x, x => x);

                Assert.Equal("in", result);
            }
        }
    }
}