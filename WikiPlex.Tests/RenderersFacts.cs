using System;
using System.Linq;
using Moq;
using WikiPlex.Formatting;
using Xunit;

namespace WikiPlex.Tests
{
    public class RenderersFacts
    {
        private class EmptyIdRenderer : IRenderer
        {
            public string Id
            {
                get { return string.Empty; }
            }

            public bool CanExpand(string scopeName)
            {
                throw new NotImplementedException();
            }

            public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
            {
                throw new NotImplementedException();
            }
        }

        private class NullIdRenderer : IRenderer
        {
            public string Id
            {
                get { return null; }
            }

            public bool CanExpand(string scopeName)
            {
                throw new NotImplementedException();
            }

            public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
            {
                throw new NotImplementedException();
            }
        }

        public class Register
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_is_null()
            {
                Exception ex = Record.Exception(() => Renderers.Register(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_id_is_null()
            {
                var macro = new Mock<IRenderer>();

                Exception ex = Record.Exception(() => Renderers.Register(macro.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_id_is_null_by_type()
            {
                Exception ex = Record.Exception(() => Renderers.Register<NullIdRenderer>());

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_renderer_id_is_empty()
            {
                var macro = new Mock<IRenderer>();
                macro.Setup(x => x.Id).Returns(string.Empty);

                Exception ex = Record.Exception(() => Renderers.Register(macro.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("renderer", ((ArgumentException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_renderer_id_is_empty_by_type()
            {
                Exception ex = Record.Exception(() => Renderers.Register<EmptyIdRenderer>());

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("renderer", ((ArgumentException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_correctly_load_the_macro()
            {
                var macro = new Mock<IRenderer>();
                macro.Setup(x => x.Id).Returns("foo");

                Renderers.Register(macro.Object);

                Assert.Contains(macro.Object, Renderers.All);
            }

            [Fact]
            public void Will_correctly_load_the_macro_by_type()
            {
                Renderers.Register<ValidRenderer>();

                Assert.Contains(typeof(ValidRenderer), Renderers.All.Select(r => r.GetType()));
            }

            [Fact]
            public void Will_replace_an_existing_renderer_with_the_same_identifier()
            {
                var macro1 = new Mock<IRenderer>();
                macro1.Setup(x => x.Id).Returns("foo");
                var macro2 = new Mock<IRenderer>();
                macro2.Setup(x => x.Id).Returns("foo");

                Renderers.Register(macro1.Object);
                Renderers.Register(macro2.Object);

                Assert.DoesNotContain(macro1.Object, Renderers.All);
                Assert.Contains(macro2.Object, Renderers.All);
            }

            [Fact]
            public void Will_add_multiple_renderers_with_the_a_different_identifier()
            {
                var macro1 = new Mock<IRenderer>();
                macro1.Setup(x => x.Id).Returns("foo");
                var macro2 = new Mock<IRenderer>();
                macro2.Setup(x => x.Id).Returns("bar");

                Renderers.Register(macro1.Object);
                Renderers.Register(macro2.Object);

                Assert.Contains(macro1.Object, Renderers.All);
                Assert.Contains(macro2.Object, Renderers.All);
            }
        }

        public class Unregister
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_is_null()
            {
                Exception ex = Record.Exception(() => Renderers.Unregister(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_id_is_null()
            {
                var renderer = new Mock<IRenderer>();

                Exception ex = Record.Exception(() => Renderers.Unregister(renderer.Object));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentNullException_when_renderer_id_is_null_by_type()
            {
                Exception ex = Record.Exception(() => Renderers.Unregister<NullIdRenderer>());

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("renderer", ((ArgumentNullException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_renderer_id_is_empty()
            {
                var renderer = new Mock<IRenderer>();
                renderer.Setup(x => x.Id).Returns(string.Empty);

                Exception ex = Record.Exception(() => Renderers.Unregister(renderer.Object));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("renderer", ((ArgumentException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_throw_ArgumentException_when_renderer_id_is_empty_by_type()
            {
                Exception ex = Record.Exception(() => Renderers.Unregister<EmptyIdRenderer>());

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("renderer", ((ArgumentException)ex).ParamName);
                Assert.True(ex.Message.StartsWith("The renderer identifier must not be null or empty."));
            }

            [Fact]
            public void Will_correctly_unregister_the_macro()
            {
                var renderer = new Mock<IRenderer>();
                renderer.Setup(x => x.Id).Returns("foo");
                Renderers.Register(renderer.Object);

                Renderers.Unregister(renderer.Object);

                Assert.DoesNotContain(renderer.Object, Renderers.All);
            }

            [Fact]
            public void Will_correctly_unregister_the_macro_by_type()
            {
                Renderers.Register<ValidRenderer>();

                Renderers.Unregister<ValidRenderer>();

                Assert.DoesNotContain(typeof(ValidRenderer), Renderers.All.Select(m => m.GetType()));
            }
        }

        private class ValidRenderer : IRenderer
        {
            public string Id
            {
                get { return "foo"; }
            }

            public bool CanExpand(string scopeName)
            {
                throw new NotImplementedException();
            }

            public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
            {
                throw new NotImplementedException();
            }
        }
    }
}