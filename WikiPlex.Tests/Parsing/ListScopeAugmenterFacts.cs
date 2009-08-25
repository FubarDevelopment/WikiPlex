using System.Collections.Generic;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;
using Xunit;

namespace WikiPlex.Tests.Parsing
{
    public class ListScopeAugmenterFacts
    {
        public class Augment
        {
            [Fact]
            public void Should_add_block_start_and_end_scopes_for_single_list_item()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0)
                                                     });
                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a");

                Assert.Equal(2, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(0, actualScopes[0].Index);
                Assert.Equal(2, actualScopes[0].Length);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[1].Name);
                Assert.Equal(3, actualScopes[1].Index);
                Assert.Equal(0, actualScopes[1].Length);
            }

            [Fact]
            public void Will_yield_the_correct_scopes()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 2),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0)
                                                     });
                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n* b");

                Assert.Equal(4, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[3].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_with_nested_item_at_end()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 3),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0)
                                                     });
                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n** b");

                Assert.Equal(4, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[1].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[3].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_with_nested_item_in_middle()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 3),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0),
                                                         new Scope(ScopeName.ListItemBegin, 8, 2),
                                                         new Scope(ScopeName.ListItemEnd, 10, 0)
                                                     });
                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n** b\n* a");

                Assert.Equal(6, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[1].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[2].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
            }

            [Fact]
            public void Should_have_correct_scopes_for_multiple_item_nested_levels()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 2),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0),
                                                         new Scope(ScopeName.ListItemBegin, 8, 3),
                                                         new Scope(ScopeName.ListItemEnd, 12, 0),
                                                         new Scope(ScopeName.ListItemBegin, 13, 3),
                                                         new Scope(ScopeName.ListItemEnd, 17, 0)
                                                     });
                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n* a\n** b\n** b");

                Assert.Equal(8, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[4].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[5].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[6].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[7].Name);
            }

            [Fact]
            public void Should_have_correct_scopes_when_walking_up_and_down_nested_levels()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 2),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0),
                                                         new Scope(ScopeName.ListItemBegin, 8, 3),
                                                         new Scope(ScopeName.ListItemEnd, 12, 0),
                                                         new Scope(ScopeName.ListItemBegin, 13, 3),
                                                         new Scope(ScopeName.ListItemEnd, 17, 0),
                                                         new Scope(ScopeName.ListItemBegin, 18, 2),
                                                         new Scope(ScopeName.ListItemEnd, 20, 0)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n* a\n** b\n** b\n* a");

                Assert.Equal(10, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[4].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[5].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[6].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[7].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[8].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[9].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_with_nested_item_in_middle_three_deep()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 2),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0),
                                                         new Scope(ScopeName.ListItemBegin, 8, 3),
                                                         new Scope(ScopeName.ListItemEnd, 12, 0),
                                                         new Scope(ScopeName.ListItemBegin, 13, 4),
                                                         new Scope(ScopeName.ListItemEnd, 18, 0),
                                                         new Scope(ScopeName.ListItemBegin, 19, 2),
                                                         new Scope(ScopeName.ListItemEnd, 20, 0)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n* a\n** b\n*** c\n* a");

                Assert.Equal(10, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[3].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[6].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[7].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[8].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[9].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_with_nested_item_at_end_with_multiple_blocks()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 4, 3),
                                                         new Scope(ScopeName.ListItemEnd, 7, 0),
                                                         new Scope(ScopeName.ListItemBegin, 9, 2),
                                                         new Scope(ScopeName.ListItemEnd, 12, 0)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n** b\n\n* a");

                Assert.Equal(6, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[1].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[3].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_when_first_items_index_is_not_one_in_length()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 3),
                                                         new Scope(ScopeName.ListItemEnd, 4, 0),
                                                         new Scope(ScopeName.ListItemBegin, 5, 3),
                                                         new Scope(ScopeName.ListItemEnd, 9, 0),
                                                         new Scope(ScopeName.ListItemBegin, 10, 3),
                                                         new Scope(ScopeName.ListItemEnd, 14)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "** a\n** a\n** a");

                Assert.Equal(6, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_when_first_items_index_is_two_in_length_and_last_item_index_is_one_in_length()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 3),
                                                         new Scope(ScopeName.ListItemEnd, 4, 0),
                                                         new Scope(ScopeName.ListItemBegin, 5, 3),
                                                         new Scope(ScopeName.ListItemEnd, 9, 0),
                                                         new Scope(ScopeName.ListItemBegin, 10, 2),
                                                         new Scope(ScopeName.ListItemEnd, 13)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "** a\n** a\n* a");

                Assert.Equal(6, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[1].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[2].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[3].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_when_second_list_first_items_index_is_not_one_in_length()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 5, 3),
                                                         new Scope(ScopeName.ListItemEnd, 9, 0),
                                                         new Scope(ScopeName.ListItemBegin, 10, 3),
                                                         new Scope(ScopeName.ListItemEnd, 14)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n\n** a\n** a");

                Assert.Equal(6, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[1].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[2].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
            }

            [Fact]
            public void Will_yield_the_correct_scopes_when_second_list_first_items_index_is_not_one_in_length_in_middle()
            {
                var origScopes = new List<Scope>(new[]
                                                     {
                                                         new Scope(ScopeName.ListItemBegin, 0, 2),
                                                         new Scope(ScopeName.ListItemEnd, 3, 0),
                                                         new Scope(ScopeName.ListItemBegin, 5, 3),
                                                         new Scope(ScopeName.ListItemEnd, 9, 0),
                                                         new Scope(ScopeName.ListItemBegin, 10, 3),
                                                         new Scope(ScopeName.ListItemEnd, 14),
                                                         new Scope(ScopeName.ListItemBegin, 16, 2),
                                                         new Scope(ScopeName.ListItemEnd, 18)
                                                     });

                var augmenter = new ListScopeAugmenter<UnorderedListMacro>();

                IList<Scope> actualScopes = augmenter.Augment(new UnorderedListMacro(), origScopes, "* a\n\n** a\n** a\n\n* a");

                Assert.Equal(8, actualScopes.Count);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[0].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[1].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[2].Name);
                Assert.Equal(ScopeName.ListItemEnd, actualScopes[3].Name);
                Assert.Equal(ScopeName.ListItemBegin, actualScopes[4].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[5].Name);
                Assert.Equal(ScopeName.UnorderedListBeginTag, actualScopes[6].Name);
                Assert.Equal(ScopeName.UnorderedListEndTag, actualScopes[7].Name);
            }
        }
    }
}