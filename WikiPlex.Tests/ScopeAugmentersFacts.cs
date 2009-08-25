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
        public class Constructor
        {
            [Fact]
            public void Should_contain_the_correct_scope_augmenters()
            {
                Assert.Equal(4, ScopeAugmenters.All.Count);
                Assert.IsType<TableScopeAugmenter>(ScopeAugmenters.All.FindByMacro<TableMacro>());
                Assert.IsType<ListScopeAugmenter<OrderedListMacro>>(ScopeAugmenters.All.FindByMacro<OrderedListMacro>());
                Assert.IsType<ListScopeAugmenter<UnorderedListMacro>>(ScopeAugmenters.All.FindByMacro<UnorderedListMacro>());
                Assert.IsType<IndentationScopeAugmenter>(ScopeAugmenters.All.FindByMacro<IndentationMacro>());
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
                IDictionary<string, IScopeAugmenter> augmenters = new Dictionary<string, IScopeAugmenter>();
                IScopeAugmenter result = augmenters.FindByMacro<FakeMacro>();

                Assert.Null(result);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro()
            {
                var augmenter = new FakeAugmenter();
                var augmenters = new Dictionary<string, IScopeAugmenter> {{"Fake", augmenter}};

                IScopeAugmenter result = augmenters.FindByMacro<FakeMacro>();

                Assert.Equal(augmenter, result);
                ScopeAugmenters.Unregister<FakeMacro>();
            }

            [Fact]
            public void Will_throw_ArgumentNullException_if_macro_object_is_null()
            {
                var augmenters = new Dictionary<string, IScopeAugmenter>();
                Exception ex = Record.Exception(() => augmenters.FindByMacro<FakeMacro>(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_return_the_augmenter_for_a_macro_object()
            {
                var augmenter = new FakeAugmenter();
                var augmenters = new Dictionary<string, IScopeAugmenter> {{"Fake", augmenter}};

                IScopeAugmenter result = augmenters.FindByMacro(new FakeMacro());

                Assert.Equal(augmenter, result);
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
                try
                {
                    var augmenter = new FakeAugmenter();

                    ScopeAugmenters.Register<FakeMacro>(augmenter);

                    Assert.Contains("Fake", ScopeAugmenters.All.Keys);
                    Assert.Contains(augmenter, ScopeAugmenters.All.Values);
                }
                finally
                {
                    ScopeAugmenters.Unregister<FakeMacro>();
                }
            }

            [Fact]
            public void Will_correctly_load_the_augmenter_by_type()
            {
                try
                {
                    ScopeAugmenters.Register<FakeMacro, FakeAugmenter>();

                    Assert.Contains("Fake", ScopeAugmenters.All.Keys);
                    Assert.IsType<FakeAugmenter>(ScopeAugmenters.All["Fake"]);
                }
                finally
                {
                    ScopeAugmenters.Unregister<FakeMacro>();
                }
            }

            [Fact]
            public void Will_correctly_replace_augmenter_with_same_macro_type()
            {
                try
                {
                    var augmenter = new FakeAugmenter();
                    ScopeAugmenters.Register<FakeMacro, FakeAugmenter>();

                    ScopeAugmenters.Register<FakeMacro>(augmenter);

                    Assert.Equal(augmenter, ScopeAugmenters.All["Fake"]);
                }
                finally
                {
                    ScopeAugmenters.Unregister<FakeMacro>();
                }
            }

            [Fact]
            public void Will_correctly_load_multiple_augmenters()
            {
                try
                {
                    var augmenter1 = new FakeAugmenter();
                    var augmenter2 = new SecondFakeAugmenter();

                    ScopeAugmenters.Register<FakeMacro>(augmenter1);
                    ScopeAugmenters.Register<SecondFakeMacro>(augmenter2);

                    Assert.Equal(augmenter1, ScopeAugmenters.All["Fake"]);
                    Assert.Equal(augmenter2, ScopeAugmenters.All["SecondFake"]);
                }
                finally
                {
                    ScopeAugmenters.Unregister<FakeMacro>();
                    ScopeAugmenters.Unregister<SecondFakeMacro>();
                }
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
                try
                {
                    var augmenter1 = new FakeAugmenter();
                    var augmenter2 = new SecondFakeAugmenter();
                    ScopeAugmenters.Register<FakeMacro>(augmenter1);
                    ScopeAugmenters.Register<SecondFakeMacro>(augmenter2);

                    ScopeAugmenters.Unregister<FakeMacro>();

                    Assert.False(ScopeAugmenters.All.ContainsKey("Fake"));
                    Assert.Equal(augmenter2, ScopeAugmenters.All["SecondFake"]);
                }
                finally
                {
                    ScopeAugmenters.Unregister<SecondFakeMacro>();
                }
            }
        }
    }
}