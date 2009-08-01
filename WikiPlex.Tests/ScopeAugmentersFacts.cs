using System;
using System.Collections.Generic;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;
using Xunit;

namespace WikiPlex.Tests
{
    public class ScopeAugmentersFacts
    {
        private class Constructor
        {
            [Fact]
            public void Should_contain_the_correct_scope_augmenters()
            {
                Assert.IsType<TableScopeAugmenter>(ScopeAugmenters.FindByMacro<TableMacro>());
                Assert.IsType<ListScopeAugmenter<OrderedListMacro>>(ScopeAugmenters.FindByMacro<OrderedListMacro>());
                Assert.IsType<ListScopeAugmenter<UnorderedListMacro>>(ScopeAugmenters.FindByMacro<UnorderedListMacro>());
            }
        }

        private class FakeAugmenter : IScopeAugmenter
        {
            public IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeMacro : IMacro
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

        public class FindByMacro
        {
            [Fact]
            public void Will_return_null_if_augmenter_not_found()
            {
                IScopeAugmenter result = ScopeAugmenters.FindByMacro<FakeMacro>();

                Assert.Null(result);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro()
            {
                var augmenter = new FakeAugmenter();
                ScopeAugmenters.Register<FakeMacro>(augmenter);

                IScopeAugmenter result = ScopeAugmenters.FindByMacro<FakeMacro>();

                Assert.Equal(augmenter, result);
                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_object_is_null()
            {
                Exception ex = Record.Exception(() => ScopeAugmenters.FindByMacro<FakeMacro>(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro_object()
            {
                var augmenter = new FakeAugmenter();
                ScopeAugmenters.Register<FakeMacro>(augmenter);

                IScopeAugmenter result = ScopeAugmenters.FindByMacro(new FakeMacro());

                Assert.Equal(augmenter, result);
                ScopeAugmenters.Unregister<FakeMacro>();
            }
        }

        public class Register
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_augmenter_is_null()
            {
                Exception ex = Record.Exception(() => ScopeAugmenters.Register<FakeMacro>(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_correctly_load_the_augmenter()
            {
                var augmenter = new FakeAugmenter();

                ScopeAugmenters.Register<FakeMacro>(augmenter);

                Assert.Equal(augmenter, ScopeAugmenters.FindByMacro<FakeMacro>());

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
                var augmenter = new FakeAugmenter();
                ScopeAugmenters.Register<FakeMacro, FakeAugmenter>();

                ScopeAugmenters.Register<FakeMacro>(augmenter);

                Assert.Equal(augmenter, ScopeAugmenters.FindByMacro<FakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_correctly_load_multiple_augmenters()
            {
                var augmenter1 = new FakeAugmenter();
                var augmenter2 = new SecondFakeAugmenter();

                ScopeAugmenters.Register<FakeMacro>(augmenter1);
                ScopeAugmenters.Register<SecondFakeMacro>(augmenter2);

                Assert.Equal(augmenter1, ScopeAugmenters.FindByMacro<FakeMacro>());
                Assert.Equal(augmenter2, (SecondFakeAugmenter) ScopeAugmenters.FindByMacro<SecondFakeMacro>());

                ScopeAugmenters.Unregister<FakeMacro>();
                ScopeAugmenters.Unregister<SecondFakeMacro>();
            }
        }

        private class SecondFakeAugmenter : IScopeAugmenter
        {
            public IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
            {
                throw new NotImplementedException();
            }
        }

        private class SecondFakeMacro : IMacro
        {
            public string Id
            {
                get { return "SecondFake"; }
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
                var augmenter1 = new FakeAugmenter();
                var augmenter2 = new SecondFakeAugmenter();
                ScopeAugmenters.Register<FakeMacro>(augmenter1);
                ScopeAugmenters.Register<SecondFakeMacro>(augmenter2);

                ScopeAugmenters.Unregister<FakeMacro>();

                Assert.Null(ScopeAugmenters.FindByMacro<FakeMacro>());
                Assert.Equal(augmenter2, (SecondFakeAugmenter) ScopeAugmenters.FindByMacro<SecondFakeMacro>());
                ScopeAugmenters.Unregister<SecondFakeMacro>();
            }
        }
    }
}