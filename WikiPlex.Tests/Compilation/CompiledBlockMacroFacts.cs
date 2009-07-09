using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using WikiPlex.Compilation;

namespace WikiPlex.Tests.Compilation
{
    public class CompiledBlockMacroFacts
    {
        public class Constructor
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_identifier_is_null()
            {
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(null, regex, captures, "foo", "foo", "foo"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("id", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_identifier_is_empty()
            {
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(string.Empty, regex, captures, "foo", "foo", "foo"));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("id", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_regex_is_null()
            {
                const string id = "foo";
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, null, captures, "foo", "foo", "foo"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_captures_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, null, "foo", "foo", "foo"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("captures", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_captures_is_empty()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string>();

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, "foo", "foo", "foo"));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("captures", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_block_start_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, null, "foo", "foo"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("blockStartScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_block_start_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, string.Empty, "foo", "foo"));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("blockStartScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_block_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, "foo", null, "foo"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("blockEndScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_block_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, "foo", string.Empty, "foo"));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("blockEndScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_item_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, "foo", "foo", null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("itemEndScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_item_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledBlockMacro(id, regex, captures, "foo", "foo", string.Empty));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("itemEndScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_properly_set_all_properties()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                var compiledMacro = new CompiledBlockMacro(id, regex, captures, "a", "b", "c");

                Assert.Equal(id, compiledMacro.Id);
                Assert.Equal(regex, compiledMacro.Regex);
                Assert.Equal(captures, compiledMacro.Captures);
                Assert.Equal("a", compiledMacro.BlockStartScope);
                Assert.Equal("b", compiledMacro.BlockEndScope);
                Assert.Equal("c", compiledMacro.ItemEndScope);
            }
        }
    }
}