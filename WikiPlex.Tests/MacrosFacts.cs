using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using Xunit;

namespace WikiPlex.Tests
{
    public class MacrosFacts
    {
        private class EmptyIdMacro : IMacro
        {
            public string Id
            {
                get { return string.Empty; }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }

        private class NullIdMacro : IMacro
        {
            public string Id
            {
                get { return null; }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class Register
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_is_null()
            {
                Exception ex = Record.Exception(() => Macros.Register(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_id_is_null()
            {
                var macro = new Mock<IMacro>();

                Exception ex = Record.Exception(() => Macros.Register(macro.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_id_is_null_by_type()
            {
                Exception ex = Record.Exception(() => Macros.Register<NullIdMacro>());

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_macro_id_is_empty()
            {
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns(string.Empty);

                Exception ex = Record.Exception(() => Macros.Register(macro.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_macro_id_is_empty_by_type()
            {
                Exception ex = Record.Exception(() => Macros.Register<EmptyIdMacro>());

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_correctly_load_the_macro()
            {
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");

                Macros.Register(macro.Object);

                Assert.Contains(macro.Object, Macros.All);
                Macros.Unregister(macro.Object);
            }

            [Fact]
            public void Will_correctly_load_the_macro_by_type()
            {
                Macros.Register<ValidMacro>();

                Assert.Contains(typeof (ValidMacro), Macros.All.Select(m => m.GetType()));
                Macros.Unregister(new ValidMacro());
            }

            [Fact]
            public void Will_replace_an_existing_macro_with_the_same_identifier()
            {
                var macro1 = new Mock<IMacro>();
                macro1.Setup(x => x.Id).Returns("foo");
                var macro2 = new Mock<IMacro>();
                macro2.Setup(x => x.Id).Returns("foo");

                Macros.Register(macro1.Object);
                Macros.Register(macro2.Object);

                Assert.DoesNotContain(macro1.Object, Macros.All);
                Assert.Contains(macro2.Object, Macros.All);
                Macros.Unregister(macro2.Object);
            }

            [Fact]
            public void Will_add_multiple_macros_with_the_a_different_identifier()
            {
                var macro1 = new Mock<IMacro>();
                macro1.Setup(x => x.Id).Returns("foo");
                var macro2 = new Mock<IMacro>();
                macro2.Setup(x => x.Id).Returns("bar");

                Macros.Register(macro1.Object);
                Macros.Register(macro2.Object);

                Assert.Contains(macro1.Object, Macros.All);
                Assert.Contains(macro2.Object, Macros.All);
                Macros.Unregister(macro1.Object);
                Macros.Unregister(macro2.Object);
            }
        }

        public class Unregister
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_is_null()
            {
                Exception ex = Record.Exception(() => Macros.Unregister(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_id_is_null()
            {
                var macro = new Mock<IMacro>();

                Exception ex = Record.Exception(() => Macros.Unregister(macro.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_macro_id_is_null_by_type()
            {
                Exception ex = Record.Exception(() => Macros.Unregister<NullIdMacro>());

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("macro", ((ArgumentNullException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_macro_id_is_empty()
            {
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns(string.Empty);

                Exception ex = Record.Exception(() => Macros.Unregister(macro.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_macro_id_is_empty_by_type()
            {
                Exception ex = Record.Exception(() => Macros.Unregister<EmptyIdMacro>());

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("macro", ((ArgumentException) ex).ParamName);
                Assert.True(ex.Message.StartsWith("The macro identifier must not be null or empty."));
            }

            [Fact]
            public void Will_correctly_unregister_the_macro()
            {
                var macro = new Mock<IMacro>();
                macro.Setup(x => x.Id).Returns("foo");
                Macros.Register(macro.Object);

                Macros.Unregister(macro.Object);

                Assert.DoesNotContain(macro.Object, Macros.All);
            }

            [Fact]
            public void Will_correctly_unregister_the_macro_by_type()
            {
                Macros.Register<ValidMacro>();

                Macros.Unregister<ValidMacro>();

                Assert.DoesNotContain(typeof (ValidMacro), Macros.All.Select(m => m.GetType()));
            }
        }

        private class ValidMacro : IMacro
        {
            public string Id
            {
                get { return "foo"; }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}