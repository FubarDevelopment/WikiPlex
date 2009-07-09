using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using WikiPlex.Compilation;

namespace WikiPlex.Tests.Compilation
{
    public class CompiledNestedBlockMacroFacts
    {
        public class Constructor
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_identifier_is_null()
            {
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(null, regex, captures, "foo", "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("id", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_identifier_is_empty()
            {
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(string.Empty, regex, captures, "foo", "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("id", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_regex_is_null()
            {
                const string id = "foo";
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, null, captures, "foo", "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_captures_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, null, "foo", "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("captures", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_captures_is_empty()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string>();

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("captures", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_block_start_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, null, "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("blockStartScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_block_start_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, string.Empty, "foo", "foo", "foo", x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("blockStartScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_block_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", null, "foo", "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("blockEndScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_block_end_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", string.Empty, "foo", "foo", x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("blockEndScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_start_item_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", null, "foo", x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("itemStartScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_start_item_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", string.Empty, "foo", x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("itemStartScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_end_item_scope_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", "foo", null, x => 1));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("itemEndScope", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_end_item_scope_is_empty()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", "foo", string.Empty, x => 1));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("itemEndScope", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_determine_level_is_null()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };

                Exception ex = Record.Exception(() => new CompiledNestedBlockMacro(id, regex, captures, "foo", "foo", "foo", "foo", null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("determineLevel", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_properly_set_all_properties()
            {
                const string id = "foo";
                var regex = new Regex("foo");
                var captures = new List<string> { "foo" };
                Func<string, int> determineLevel = (x => 1);

                var compiledMacro = new CompiledNestedBlockMacro(id, regex, captures, "a", "b", "c", "d", determineLevel);

                Assert.Equal(id, compiledMacro.Id);
                Assert.Equal(regex, compiledMacro.Regex);
                Assert.Equal(captures, compiledMacro.Captures);
                Assert.Equal("a", compiledMacro.BlockStartScope);
                Assert.Equal("b", compiledMacro.BlockEndScope);
                Assert.Equal("c", compiledMacro.ItemStartScope);
                Assert.Equal("d", compiledMacro.ItemEndScope);
                Assert.Equal(determineLevel, compiledMacro.DetermineLevel);
            }
        }
    }
}