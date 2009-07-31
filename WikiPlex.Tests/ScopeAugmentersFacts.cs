using System;
using System.Collections.Generic;
using Moq;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;
using Xunit;

namespace WikiPlex.Tests
{
    public class ScopeAugmentersFacts
    {
        public class FakeAugmenter : IScopeAugmenter<FakeMacro>
        {
            public IList<Scope> Augment(FakeMacro macro, IList<Scope> capturedScopes, string content)
            {
                throw new NotImplementedException();
            }
        }

        public class FakeMacro : IMacro
        {
            public string Id
            {
                get { throw new NotImplementedException(); }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class FindByMacro
        {
            [Fact]
            public void Will_return_null_if_augmenter_not_found()
            {
                IScopeAugmenter<FakeMacro> result = ScopeAugmenters.FindByMacro<FakeMacro>();

                Assert.Null(result);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro()
            {
                var augmenter = new Mock<IScopeAugmenter<FakeMacro>>();
                ScopeAugmenters.Register(augmenter.Object);

                IScopeAugmenter<FakeMacro> result = ScopeAugmenters.FindByMacro<FakeMacro>();

                Assert.Equal(augmenter.Object, result);
                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_object_is_null()
            {
                var ex = Record.Exception(() => ScopeAugmenters.FindByMacro<FakeMacro>(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro_object()
            {
                var augmenter = new Mock<IScopeAugmenter<FakeMacro>>();
                ScopeAugmenters.Register(augmenter.Object);

                IScopeAugmenter<FakeMacro> result = ScopeAugmenters.FindByMacro(new FakeMacro());

                Assert.Equal(augmenter.Object, result);
                ScopeAugmenters.Unregister<FakeMacro>();
            }
        }

        public class Register
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_augmenter_is_null()
            {
                var ex = Record.Exception(() => ScopeAugmenters.Register<FakeMacro>(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_correctly_load_the_augmenter()
            {
                var augmenter = new Mock<IScopeAugmenter<FakeMacro>>();

                ScopeAugmenters.Register(augmenter.Object);

                Assert.Equal(augmenter.Object, ScopeAugmenters.FindByMacro<FakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_correctly_load_the_augmenter_by_type()
            {
                ScopeAugmenters.Register<FakeMacro, FakeAugmenter>();

                Assert.NotNull(ScopeAugmenters.FindByMacro<FakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_correctly_replace_augmenter_with_same_macro_type()
            {
                var augmenter = new Mock<IScopeAugmenter<FakeMacro>>();
                ScopeAugmenters.Register<FakeMacro, FakeAugmenter>();

                ScopeAugmenters.Register(augmenter.Object);

                Assert.Equal(augmenter.Object, ScopeAugmenters.FindByMacro<FakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_correctly_load_multiple_augmenters()
            {
                var augmenter1 = new Mock<IScopeAugmenter<FakeMacro>>();
                var augmenter2 = new Mock<IScopeAugmenter<SecondFakeMacro>>();

                ScopeAugmenters.Register(augmenter1.Object);
                ScopeAugmenters.Register(augmenter2.Object);

                Assert.Equal(augmenter1.Object, ScopeAugmenters.FindByMacro<FakeMacro>());
                Assert.Equal(augmenter2.Object, ScopeAugmenters.FindByMacro<SecondFakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
                ScopeAugmenters.Unregister<SecondFakeMacro>();
            }
        }

        public class SecondFakeMacro : IMacro
        {
            public string Id
            {
                get { throw new NotImplementedException(); }
            }

            public IList<MacroRule> Rules
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class Unregister
        {
            [Fact]
            public void Will_unregister_augmenter_by_macro_type_correctly()
            {
                var augmenter1 = new Mock<IScopeAugmenter<FakeMacro>>();
                var augmenter2 = new Mock<IScopeAugmenter<SecondFakeMacro>>();
                ScopeAugmenters.Register(augmenter1.Object);
                ScopeAugmenters.Register(augmenter2.Object);

                ScopeAugmenters.Unregister<FakeMacro>();

                Assert.Null(ScopeAugmenters.FindByMacro<FakeMacro>());
                Assert.Equal(augmenter2.Object, ScopeAugmenters.FindByMacro<SecondFakeMacro>());
                ScopeAugmenters.Unregister<SecondFakeMacro>();
            }
        }
    }
}