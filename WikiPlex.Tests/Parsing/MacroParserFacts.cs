using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Moq;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;

namespace WikiPlex.Tests.Parsing
{
    public class MacroParserFacts
    {
        public class Parse
        {
            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void Will_immediately_return_when_wiki_content_is_not_present(string wikiContent)
            {
                var parser = new MacroParser(new Mock<IMacroCompiler>().Object);
                var macros = new List<IMacro> {new Mock<IMacro>().Object};
                int invocations = 0;

                parser.Parse(wikiContent, macros, s => invocations++);

                Assert.Equal(0, invocations);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_macros_is_null()
            {
                var parser = new MacroParser(new Mock<IMacroCompiler>().Object);
                int invocations = 0;

                Exception ex = Record.Exception(() => parser.Parse("content", null, s => invocations++));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macros", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentException_when_macros_is_empty()
            {
                var parser = new MacroParser(new Mock<IMacroCompiler>().Object);
                var macros = new List<IMacro>();
                int invocations = 0;

                Exception ex = Record.Exception(() => parser.Parse("content", macros, s => invocations++));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macros", ((ArgumentException) ex).ParamName);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_macro()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledMacro("foo", new Regex("abc"), new List<string> {"All"}));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> {new Mock<IMacro>().Object};
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("this is abc content", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped[0].Index);
                Assert.Equal(3, popped[0].Length);
                Assert.Equal("All", popped[0].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_block_macro()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledBlockMacro("foo", new Regex(@"(\n?\*\s).*?(\n?)"), new List<string> { "<tr><td>", "</td></tr>", null }, "<table>", "</table>", "</td></tr>"));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> {new Mock<IMacro>().Object};
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<table>", popped[0].Name);
                Assert.Equal("<tr><td>", popped[1].Name);
                Assert.Equal("</td></tr>", popped[2].Name);
                Assert.Equal("<tr><td>", popped[3].Name);
                Assert.Equal("</td></tr>", popped[4].Name);
                Assert.Equal("</table>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_block_macro_with_two_entries()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledBlockMacro("foo", new Regex(@"(\n?\*\s).*?(\n?)"), new List<string> { "<tr><td>", "</td></tr>", null }, "<table>", "</table>", "</td></tr>"));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n* a\n\n* a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(12, popped.Count);
                Assert.Equal("<table>", popped[0].Name);
                Assert.Equal("<tr><td>", popped[1].Name);
                Assert.Equal("</td></tr>", popped[2].Name);
                Assert.Equal("<tr><td>", popped[3].Name);
                Assert.Equal("</td></tr>", popped[4].Name);
                Assert.Equal("</table>", popped[5].Name);
                Assert.Equal("<table>", popped[6].Name);
                Assert.Equal("<tr><td>", popped[7].Name);
                Assert.Equal("</td></tr>", popped[8].Name);
                Assert.Equal("<tr><td>", popped[9].Name);
                Assert.Equal("</td></tr>", popped[10].Name);
                Assert.Equal("</table>", popped[11].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_and_macro_single_scope_with_supports_nested_block()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex("abc"), new List<string> { "All" }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => 1));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("this is abc content", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(3, popped.Count);
                Assert.Equal(8, popped[0].Index);
                Assert.Equal(0, popped[0].Length);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal(8, popped[1].Index);
                Assert.Equal(3, popped[1].Length);
                Assert.Equal("All", popped[1].Name);
                Assert.Equal(11, popped[2].Index);
                Assert.Equal(0, popped[2].Length);
                Assert.Equal("</li></ul>", popped[2].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => 1));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(4, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li>", popped[1].Name);
                Assert.Equal("<li>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
            }
            
            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_multiple_blocks()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => 1));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n* a\n\n* a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li>", popped[1].Name);
                Assert.Equal("<li>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
                Assert.Equal("<ul><li>", popped[4].Name);
                Assert.Equal("</li>", popped[5].Name);
                Assert.Equal("<li>", popped[6].Name);
                Assert.Equal("</li></ul>", popped[7].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_nested_item_at_end()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n** a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(4, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("<ul><li>", popped[1].Name);
                Assert.Equal("</li></ul>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_nested_item_in_middle()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n** a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("<ul><li>", popped[1].Name);
                Assert.Equal("</li></ul>", popped[2].Name);
                Assert.Equal("</li>", popped[3].Name);
                Assert.Equal("<li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_nested_item_in_middle_three_deep()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n** a\n*** a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("<ul><li>", popped[1].Name);
                Assert.Equal("<ul><li>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
                Assert.Equal("</li></ul>", popped[4].Name);
                Assert.Equal("</li>", popped[5].Name);
                Assert.Equal("<li>", popped[6].Name);
                Assert.Equal("</li></ul>", popped[7].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_nested_item_in_middle_multiple_nested()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n** a\n** a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("<ul><li>", popped[1].Name);
                Assert.Equal("</li>", popped[2].Name);
                Assert.Equal("<li>", popped[3].Name);
                Assert.Equal("</li></ul>", popped[4].Name);
                Assert.Equal("</li>", popped[5].Name);
                Assert.Equal("<li>", popped[6].Name);
                Assert.Equal("</li></ul>", popped[7].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_with_nested_item_at_end_with_multiple_blocks()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n** a\n\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("<ul><li>", popped[1].Name);
                Assert.Equal("</li></ul>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
                Assert.Equal("<ul><li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_when_first_items_index_is_not_one_in_length()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("** a\n** a\n** a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li>", popped[1].Name);
                Assert.Equal("<li>", popped[2].Name);
                Assert.Equal("</li>", popped[3].Name);
                Assert.Equal("<li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_when_first_items_index_is_two_in_length_and_last_item_index_is_one_in_length()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("** a\n** a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li>", popped[1].Name);
                Assert.Equal("<li>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
                Assert.Equal("<ul><li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_when_first_items_index_is_three_in_length_and_last_item_index_is_one_in_length()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("*** a\n*** a\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li>", popped[1].Name);
                Assert.Equal("<li>", popped[2].Name);
                Assert.Equal("</li></ul>", popped[3].Name);
                Assert.Equal("<ul><li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_when_second_list_first_items_index_is_not_one_in_length()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n\n** a\n** a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(6, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li></ul>", popped[1].Name);
                Assert.Equal("<ul><li>", popped[2].Name);
                Assert.Equal("</li>", popped[3].Name);
                Assert.Equal("<li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_matches_from_a_compiled_nested_block_macro_when_second_list_first_items_index_is_not_one_in_length_in_middle()
            {
                var compiler = new Mock<IMacroCompiler>();
                compiler.Setup(x => x.Compile(It.IsAny<IMacro>())).Returns(new CompiledNestedBlockMacro("foo", new Regex(@"(\n?\*+\s).*?(\n?)"), new List<string> { "<li>", "</li>", null }, "<ul><li>", "</li></ul>", "<li>", "</li>", x => x.Trim().Length));
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> { new Mock<IMacro>().Object };
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("* a\n\n** a\n** a\n\n* a", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped.Count);
                Assert.Equal("<ul><li>", popped[0].Name);
                Assert.Equal("</li></ul>", popped[1].Name);
                Assert.Equal("<ul><li>", popped[2].Name);
                Assert.Equal("</li>", popped[3].Name);
                Assert.Equal("<li>", popped[4].Name);
                Assert.Equal("</li></ul>", popped[5].Name);
                Assert.Equal("<ul><li>", popped[6].Name);
                Assert.Equal("</li></ul>", popped[7].Name);
            }
        }
    }
}