using System;
using System.Web.UI.WebControls;
using WikiPlex.Common;
using Xunit;
using Xunit.Extensions;

namespace WikiPlex.Tests
{
    public class ParametersFacts
    {
        public class ExtractUrl
        {
            [Fact]
            public void Should_throw_ArgumentException_if_url_is_not_found()
            {
                var ex = Record.Exception(() => Parameters.ExtractUrl(new[] { "foo=bar" })) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("url", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_url_is_empty()
            {
                var ex = Record.Exception(() => Parameters.ExtractUrl(new[] { "url=" })) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("url", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_url_is_not_valid()
            {
                var ex = Record.Exception(() => Parameters.ExtractUrl(new[] { "url=blah" })) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("url", ex.ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_url_contains_codeplex()
            {
                var ex = Record.Exception(() => Parameters.ExtractUrl(new[] { "url=http://www.codeplex.com" })) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("url", ex.ParamName);
            }

            [Fact]
            public void Should_return_url_if_valid()
            {
                string url = Parameters.ExtractUrl(new[] {"url=http://www.foo.com"});

                Assert.Equal("http://www.foo.com/", url);
            }
        }

        public class ExtractAlign
        {
            [Fact]
            public void Should_return_default_if_align_is_not_found()
            {
                HorizontalAlign align = Parameters.ExtractAlign(new string[0], HorizontalAlign.Center);

                Assert.Equal(HorizontalAlign.Center, align);
            }

            [Fact]
            public void Should_return_default_if_align_is_empty()
            {
                HorizontalAlign align = Parameters.ExtractAlign(new[] {"align="}, HorizontalAlign.Center);

                Assert.Equal(HorizontalAlign.Center, align);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_align_is_not_defined_on_enum()
            {
                var ex = Record.Exception(() => Parameters.ExtractAlign(new[] {"align=foo"}, HorizontalAlign.Center)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("align", ex.ParamName);
            }

            [Fact]
            public void Should_return_align_if_valid()
            {
                HorizontalAlign align = Parameters.ExtractAlign(new[] {"align=right"}, HorizontalAlign.Center);

                Assert.Equal(HorizontalAlign.Right, align);
            }

            [Theory]
            [InlineData("justify")]
            [InlineData("notset")]
            public void Should_throw_ArgumentException_if_align_is_invalid_value(string align)
            {
                var ex = Record.Exception(() => Parameters.ExtractAlign(new[] { "align=" + align }, HorizontalAlign.Center)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal("align", ex.ParamName);
            }
        }
    }
}