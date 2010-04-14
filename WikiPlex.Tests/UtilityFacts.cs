using System;
using Xunit;
using WikiPlex.Common;

namespace WikiPlex.Tests
{
    public class UtilityFacts
    {
        public class The_ExtractTextParts_Method
        {
            [Fact]
            public void Should_throw_ArgumentNullException_if_the_input_is_null()
            {
                var ex = Record.Exception(() => Utility.ExtractTextParts(null));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("input", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_the_input_is_empty()
            {
                var ex = Record.Exception(() => Utility.ExtractTextParts(string.Empty));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("input", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_if_the_input_contains_more_than_two_parts()
            {
                var ex = Record.Exception(() => Utility.ExtractTextParts("a|b|c"));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("input", ((ArgumentException)ex).ParamName);
                Assert.Contains("Invalid number of parts.", ex.Message);
            }

            [Fact]
            public void Should_return_the_text_part_with_only_text_when_it_has_no_friendly_text()
            {
                TextPart part = Utility.ExtractTextParts("a");

                Assert.NotNull(part);
                Assert.Equal("a", part.Text);
                Assert.Null(part.FriendlyText);
            }

            [Fact]
            public void Should_return_the_fully_loaded_text_part()
            {
                TextPart part = Utility.ExtractTextParts("a|b");

                Assert.NotNull(part);
                Assert.Equal("a", part.FriendlyText);
                Assert.Equal("b", part.Text);
            }

            [Fact]
            public void Should_trim_the_content_with_one_part()
            {
                TextPart part = Utility.ExtractTextParts(" a ");

                Assert.NotNull(part);
                Assert.Equal("a", part.Text);
            }

            [Fact]
            public void Should_trim_the_content_with_two_parts()
            {
                TextPart part = Utility.ExtractTextParts(" a | b ");

                Assert.NotNull(part);
                Assert.Equal("a", part.FriendlyText);
                Assert.Equal("b", part.Text);
            }
        }

        public class CountChars
        {
            [Fact]
            public void Should_return_0_for_null_input()
            {
                int result = Utility.CountChars('*', null);

                Assert.Equal(0, result);
            }

            [Fact]
            public void Should_return_correct_count_for_input()
            {
                int result = Utility.CountChars('*', "** a");

                Assert.Equal(2, result);
            }
        }

        private enum TestEnum
        {
            One,
            Two,
            Three
        }
    }
}