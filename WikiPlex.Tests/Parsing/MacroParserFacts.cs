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
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("Macro");
                var macros = new List<IMacro> {macro.Object};
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("this is abc content", macros, scopeStack.Push);

                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(8, popped[0].Index);
                Assert.Equal(3, popped[0].Length);
                Assert.Equal("All", popped[0].Name);
            }

            [Fact]
            public void Will_yield_the_scopes_from_the_augmenter()
            {
                var compiler = new Mock<IMacroCompiler>();
                var macro = new FakeMacro();
                var augmenter = new Mock<IScopeAugmenter>();
                compiler.Setup(x => x.Compile(macro)).Returns(new CompiledMacro("foo", new Regex("abc"), new List<string> {"All"}));
                ScopeAugmenters.Register<FakeMacro>(augmenter.Object);
                var scope = new Scope("Scope", 1);
                augmenter.Setup(x => x.Augment(macro, It.IsAny<IList<Scope>>(), It.IsAny<string>())).Returns(new List<Scope> { scope });
                var parser = new MacroParser(compiler.Object);
                var macros = new List<IMacro> {macro};
                var scopeStack = new Stack<IList<Scope>>();

                parser.Parse("abc", macros, scopeStack.Push);

                ScopeAugmenters.Unregister<FakeMacro>();
                Assert.Equal(1, scopeStack.Count);
                var popped = scopeStack.Pop();
                Assert.Equal(1, popped.Count);
                Assert.Equal("Scope", popped[0].Name);
                Assert.Equal(1, popped[0].Index);
            }
        }

        class FakeMacro : IMacro
        {
            public string Id
            {
                get { return "Fake"; }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}