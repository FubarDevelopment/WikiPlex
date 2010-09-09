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

            [Fact]
            public void Should_not_throw_ArgumentException_if_url_contains_codeplex_but_validateDomain_is_false()
            {
                var ex = Record.Exception(() => Parameters.ExtractUrl(new[] { "url=http://www.codeplex.com" }, false));

                Assert.Null(ex);
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

        public class ExtractDimensions
        {
            [Fact]
            public void Should_return_null_values_if_not_found()
            {
                Dimensions dimensions = Parameters.ExtractDimensions(new[] {"foo=bar"});

                Assert.Null(dimensions.Height);
                Assert.Null(dimensions.Width);
            }

            [Fact]
            public void Should_return_defaults_if_not_found()
            {
                Dimensions dimensions = Parameters.ExtractDimensions(new[] {"foo=bar"}, 200, 300);

                Assert.Equal(200, dimensions.Height);
                Assert.Equal(300, dimensions.Width);
            }

            [Fact]
            public void Should_return_defaults_if_empty()
            {
                Dimensions dimensions = Parameters.ExtractDimensions(new[] { "height=", "width=" }, 200, 300);

                Assert.Equal(200, dimensions.Height);
                Assert.Equal(300, dimensions.Width);
            }

            [Fact]
            public void Should_return_correct_height_and_width_as_pixels()
            {
                Dimensions dimensions = Parameters.ExtractDimensions(new[] {"height=300", "width=400"}, 200, 200);

                Assert.Equal("300px", dimensions.Height.ToString());
                Assert.Equal("400px", dimensions.Width.ToString());
            }

            [Fact]
            public void Should_return_correct_height_and_width_as_percent()
            {
                Dimensions dimensions = Parameters.ExtractDimensions(new[] {"height=50%", "width=75%"}, 200, 200);

                Assert.Equal("50%", dimensions.Height.ToString());
                Assert.Equal("75%", dimensions.Width.ToString());
            }

            [Theory]
            [InlineData("height")]
            [InlineData("width")]
            public void Should_throw_ArgumentException_if_invalid(string paramName)
            {
                var ex = Record.Exception(() => Parameters.ExtractDimensions(new[] {paramName + "=abc"}, 200, 200)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal(paramName, ex.ParamName);
            }

            [Theory]
            [InlineData("height")]
            [InlineData("width")]
            public void Should_throw_ArgumentException_if_negative(string paramName)
            {
                var ex = Record.Exception(() => Parameters.ExtractDimensions(new[] {paramName + "=-10"}, 200, 200)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal(paramName, ex.ParamName);
            }

            [Theory]
            [InlineData("height")]
            [InlineData("width")]
            public void Should_throw_ArgumentException_if_zero(string paramName)
            {
                var ex = Record.Exception(() => Parameters.ExtractDimensions(new[] { paramName + "=0" }, 200, 200)) as ArgumentException;

                Assert.NotNull(ex);
                Assert.Equal(paramName, ex.ParamName);
            }
        }

        public class ExtractBool
        {
            [Fact]
            public void Should_return_default_if_parameter_is_not_found()
            {
                bool value = Parameters.ExtractBool(new[] {"foo=bar"}, "test", true);

                Assert.True(value);
            }

            [Fact]
            public void Should_return_default_if_parameter_is_empty()
            {
                bool value = Parameters.ExtractBool(new[] {"test="}, "test", true);

                Assert.True(value);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_value_is_not_a_boolean()
            {
                var ex = Record.Exception(() => Parameters.ExtractBool(new[] {"test=bar"}, "test", true));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
            }

            [Fact]
            public void Should_return_correct_value()
            {
                bool value = Parameters.ExtractBool(new[] {"test=true"}, "test", false);

                Assert.True(value);
            }
        }
    }
}