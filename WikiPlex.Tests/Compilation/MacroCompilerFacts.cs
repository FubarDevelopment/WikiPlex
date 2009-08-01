using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using Xunit;

namespace WikiPlex.Tests.Compilation
{
    public class MacroCompilerFacts
    {
        public class Compile
        {
            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_is_null()
            {
                var compiler = new MacroCompiler();

                Exception ex = Record.Exception(() => compiler.Compile(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_identifier_is_null()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();

                Exception ex = Record.Exception(() => compiler.Compile(macro.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_if_macro_identifier_is_empty()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns(string.Empty);

                Exception ex = Record.Exception(() => compiler.Compile(macro.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_rules_is_null()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");

                Exception ex = Record.Exception(() => compiler.Compile(macro.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro rules must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_if_macro_rules_is_empty()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>());

                Exception ex = Record.Exception(() => compiler.Compile(macro.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro rules must not be null or empty."));
            }

            [Fact]
            public void Will_correctly_compile_a_macro_using_the_identifier_from_the_macro()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {new MacroRule("abc", new Dictionary<int, string> {{0, "All"}})});

                CompiledMacro compiledMacro = compiler.Compile(macro.Object);

                Assert.Equal("foo", compiledMacro.Id);
            }

            [Fact]
            public void Will_correctly_compile_a_macro_with_a_single_rule_and_whole_capture()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {new MacroRule("abc", new Dictionary<int, string> {{0, "All"}})});

                CompiledMacro compiledMacro = compiler.Compile(macro.Object);

                Assert.Equal(@"(?x)
(?-xis)(?m)(abc)(?x)", compiledMacro.Regex.ToString());
                Assert.Null(compiledMacro.Captures[0]);
                Assert.Equal("All", compiledMacro.Captures[1]);
            }

            [Fact]
            public void Will_correctly_compile_a_macro_with_a_single_rule_and_partial_capture()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {
                                                          new MacroRule("(a) (b) (c)",
                                                                        new Dictionary<int, string>
                                                                            {{1, "a"}, {2, "b"}, {3, "c"}})
                                                      });

                CompiledMacro compiledMacro = compiler.Compile(macro.Object);

                Assert.Equal(@"(?x)
(?-xis)(?m)((a) (b) (c))(?x)", compiledMacro.Regex.ToString());
                Assert.Null(compiledMacro.Captures[0]);
                Assert.Null(compiledMacro.Captures[1]);
                Assert.Equal("a", compiledMacro.Captures[2]);
                Assert.Equal("b", compiledMacro.Captures[3]);
                Assert.Equal("c", compiledMacro.Captures[4]);
            }

            [Fact]
            public void Will_correctly_compile_a_macro_with_a_single_rule_and_partial_capture_with_whole_capture()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {
                                                          new MacroRule("(a) (b) (c)",
                                                                        new Dictionary<int, string>
                                                                            {{0, "whole"}, {1, "a"}, {2, "b"}, {3, "c"}})
                                                      });

                CompiledMacro compiledMacro = compiler.Compile(macro.Object);

                Assert.Equal(@"(?x)
(?-xis)(?m)((a) (b) (c))(?x)", compiledMacro.Regex.ToString());
                Assert.Null(compiledMacro.Captures[0]);
                Assert.Equal("whole", compiledMacro.Captures[1]);
                Assert.Equal("a", compiledMacro.Captures[2]);
                Assert.Equal("b", compiledMacro.Captures[3]);
                Assert.Equal("c", compiledMacro.Captures[4]);
            }

            [Fact]
            public void Will_correctly_compile_a_macro_with_multiple_rules()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {
                                                          new MacroRule("(a) (b) (c)",
                                                                        new Dictionary<int, string>
                                                                            {{0, "whole"}, {1, "a"}, {2, "b"}, {3, "c"}}),
                                                          new MacroRule("a (second) rule",
                                                                        new Dictionary<int, string> {{1, "second"}})
                                                      });

                CompiledMacro compiledMacro = compiler.Compile(macro.Object);

                Assert.Equal(@"(?x)
(?-xis)(?m)((a) (b) (c))(?x)

|

(?-xis)(?m)(a (second) rule)(?x)",
                             compiledMacro.Regex.ToString());
                Assert.Null(compiledMacro.Captures[0]);
                Assert.Equal("whole", compiledMacro.Captures[1]);
                Assert.Equal("a", compiledMacro.Captures[2]);
                Assert.Equal("b", compiledMacro.Captures[3]);
                Assert.Equal("c", compiledMacro.Captures[4]);
                Assert.Null(compiledMacro.Captures[5]);
                Assert.Equal("second", compiledMacro.Captures[6]);
            }

            [Fact]
            public void Will_use_previously_compiled_macro()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {
                                                          new MacroRule("(a) (b) (c)",
                                                                        new Dictionary<int, string>
                                                                            {{0, "whole"}, {1, "a"}, {2, "b"}, {3, "c"}})
                                                      });

                CompiledMacro compiledMacro1 = compiler.Compile(macro.Object);
                CompiledMacro compiledMacro2 = compiler.Compile(macro.Object);

                Assert.Equal(compiledMacro1, compiledMacro2);
            }
        }

        public class Flush
        {
            [Fact]
            public void Will_flush_the_compiled_macros()
            {
                var compiler = new MacroCompiler();
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                macro.Setup(x => x.Rules).Returns(new List<MacroRule>
                                                      {
                                                          new MacroRule("(a) (b) (c)",
                                                                        new Dictionary<int, string>
                                                                            {{0, "whole"}, {1, "a"}, {2, "b"}, {3, "c"}})
                                                      });
                compiler.Compile(macro.Object);

                compiler.Flush();

                FieldInfo macrosField = typeof (MacroCompiler).GetField("compiledMacros",
                                                                        BindingFlags.Instance | BindingFlags.NonPublic);
                var compiledMacros = (Dictionary<string, CompiledMacro>) macrosField.GetValue(compiler);
                Assert.Equal(0, compiledMacros.Count);
            }
        }
    }
}