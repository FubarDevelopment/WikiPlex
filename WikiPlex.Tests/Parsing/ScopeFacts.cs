using System;
using Xunit;
using WikiPlex.Parsing;

namespace WikiPlex.Tests.Parsing
{
    public class ScopeFacts
    {
        public class Constructor_Facts
        {
            [Fact]
            public void It_Should_set_the_name_and_index_and_length()
            {
                const string name = "The Scope Name";
                const int index = 435;
                const int length = 34;

                var scope = new Scope(name, index, length);

                Assert.Equal("The Scope Name", scope.Name);
                Assert.Equal(435, scope.Index);
                Assert.Equal(34, scope.Length);
            }

            [Fact]
            public void It_Should_throw_when_name_is_null()
            {
                const string name = null;
                const int index = 435;
                const int length = 34;

                Exception ex = Record.Exception(() => new Scope(name, index, length));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("name", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void It_Should_throw_when_name_is_empty()
            {
                string name = string.Empty;
                const int index = 435;
                const int length = 34;

                Exception ex = Record.Exception(() => new Scope(name, index, length));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("name", ((ArgumentException) ex).ParamName);
            }

            [Fact]
            public void It_Should_throw_when_name_is_null_with_no_length()
            {
                const string name = null;
                const int index = 435;

                Exception ex = Record.Exception(() => new Scope(name, index));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("name", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void It_Should_throw_when_name_is_empty_with_no_length()
            {
                string name = string.Empty;
                const int index = 435;

                Exception ex = Record.Exception(() => new Scope(name, index));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("name", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void It_Should_correctly_set_the_name_index()
            {
                const string name = "The Scope Name";
                const int index = 435;
                
                var scope = new Scope(name, index);

                Assert.Equal(name, scope.Name);
                Assert.Equal(index, scope.Index);
                Assert.Equal(0, scope.Length);
            }
        }
    }
}