using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using WikiPlex.Compilation;

namespace WikiPlex.Tests.Compilation
{
    public class CompiledMacroFacts
    {
        public class Constructor
        {
            [Fact]
            public void Should_throw_ArgumentNullException_when_identifier_is_null()
            {
                var regex = new Regex("foo");
                var captures = new List<string> {"foo"};

                Exception ex = Record.Exception(() => new CompiledMacro(null, regex, captures));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("id", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_identifier_is_empty()
            {
                var regex = new Regex("foo");
                var captures = new List<string> {"foo"};

                Exception ex = Record.Exception(() => new CompiledMacro(string.Empty, regex, captures));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("id", ((ArgumentException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_regex_is_null()
            {
                const string id = "foo";
                var captures = new List<string> {"foo"};

                Exception ex = Record.Exception(() => new CompiledMacro(id, null, captures));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_captures_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");

                Exception ex = Record.Exception(() => new CompiledMacro(id, regex, null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("captures", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_captures_is_empty()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string>();

                Exception ex = Record.Exception(() => new CompiledMacro(id, regex, captures));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("captures", ((ArgumentException) ex).ParamName);
            }

            [Fact]
            public void Should_properly_set_all_properties()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> {"foo"};

                var compiledMacro = new CompiledMacro(id, regex, captures);

                Assert.Equal(id, compiledMacro.Id);
                Assert.Equal(regex, compiledMacro.Regex);
                Assert.Equal(captures, compiledMacro.Captures);
            }
        }
    }
}