using System.Collections.Generic;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;
using Xunit;

namespace WikiPlex.Tests.Parsing
{
    public class IndentationScopeAugmenterFacts
    {
        public class Augment
        {
            [Fact]
            public void Should_return_original_scopes_if_start_is_one_level()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.IndentationBegin, 0, 2),
                                                         new Scope(ScopeName.IndentationEnd, 3, 0)
                                                     });
                var augmenter = new IndentationScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new IndentationMacro(), origScopes, ": a");

                Assert.Equal(2, actualScopes.Count);
            }

            [Fact]
            public void Should_add_begin_and_end_scopes_if_two_level()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.IndentationBegin, 0, 3),
                                                         new Scope(ScopeName.IndentationEnd, 4, 0)
                                                     });
                var augmenter = new IndentationScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new IndentationMacro(), origScopes, ":: a");

                Assert.Equal(4, actualScopes.Count);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[0].Name);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[1].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[2].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[3].Name);
                Assert.Equal(0, actualScopes[0].Index);
                Assert.Equal(0, actualScopes[1].Index);
                Assert.Equal(4, actualScopes[2].Index);
                Assert.Equal(4, actualScopes[3].Index);
                Assert.Equal(3, actualScopes[0].Length);
                Assert.Equal(3, actualScopes[1].Length);
                Assert.Equal(0, actualScopes[2].Length);
                Assert.Equal(0, actualScopes[3].Length);
            }

            [Fact]
            public void Should_add_begin_and_end_scopes_if_more_than_two_level()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.IndentationBegin, 0, 5),
                                                         new Scope(ScopeName.IndentationEnd, 6, 0)
                                                     });
                var augmenter = new IndentationScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new IndentationMacro(), origScopes, ":::: a");

                Assert.Equal(8, actualScopes.Count);
            }

            [Fact]
            public void Should_correctly_add_scopes_with_multiple_matches()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.IndentationBegin, 0, 3),
                                                         new Scope(ScopeName.IndentationEnd, 4, 0),
                                                         new Scope(ScopeName.IndentationBegin, 5, 4),
                                                         new Scope(ScopeName.IndentationEnd, 10, 0)
                                                     });
                var augmenter = new IndentationScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new IndentationMacro(), origScopes, ":: a\n::: b");

                Assert.Equal(10, actualScopes.Count);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[0].Name);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[1].Name);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[4].Name);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[5].Name);
                Assert.Equal(ScopeName.IndentationBegin, actualScopes[6].Name);

                Assert.Equal(ScopeName.IndentationEnd, actualScopes[2].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[3].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[7].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[8].Name);
                Assert.Equal(ScopeName.IndentationEnd, actualScopes[9].Name);

                Assert.Equal(0, actualScopes[0].Index);
                Assert.Equal(0, actualScopes[1].Index);
                Assert.Equal(5, actualScopes[4].Index);
                Assert.Equal(5, actualScopes[5].Index);
                Assert.Equal(5, actualScopes[6].Index);

                Assert.Equal(4, actualScopes[2].Index);
                Assert.Equal(4, actualScopes[3].Index);
                Assert.Equal(10, actualScopes[7].Index);
                Assert.Equal(10, actualScopes[8].Index);
                Assert.Equal(10, actualScopes[9].Index);

                Assert.Equal(3, actualScopes[0].Length);
                Assert.Equal(3, actualScopes[1].Length);
                Assert.Equal(4, actualScopes[4].Length);
                Assert.Equal(4, actualScopes[5].Length);
                Assert.Equal(4, actualScopes[6].Length);
            }
        }
    }
}