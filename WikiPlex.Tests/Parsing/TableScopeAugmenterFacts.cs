﻿using System.Collections.Generic;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;
using WikiPlex.Parsing;
using Xunit;

namespace WikiPlex.Tests.Parsing
{
    public class TableScopeAugmenterFacts
    {
        public class Augment
        {
            [Fact]
            public void Should_add_table_begin_and_end_scopes()
            {
                var origScopes = new List<Scope>(new[]
                        {
                            new Scope(ScopeName.TableRowBegin, 0, 1),
                            new Scope(ScopeName.TableCell, 1, 1),
                            new Scope(ScopeName.TableRowEnd, 2, 1)
                        });

                var augmenter = new TableScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new TableMacro(), origScopes, null);

                Assert.Equal(5, actualScopes.Count);
                Assert.Equal(ScopeName.TableBegin, actualScopes[0].Name);
                Assert.Equal(0, actualScopes[0].Index);
                Assert.Equal(1, actualScopes[0].Length);
                Assert.Equal(ScopeName.TableEnd, actualScopes[4].Name);
                Assert.Equal(3, actualScopes[4].Index);
                Assert.Equal(0, actualScopes[4].Length);
            }

            [Fact]
            public void Should_contain_the_correct_captured_scopes()
            {
                var origScopes = new List<Scope>(new[]
                        {
                            new Scope(ScopeName.TableRowBegin, 0, 1),
                            new Scope(ScopeName.TableCell, 1, 1),
                            new Scope(ScopeName.TableRowEnd, 2, 1)
                        });

                var augmenter = new TableScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new TableMacro(), origScopes, null);

                Assert.Equal(5, actualScopes.Count);
                Assert.Equal(origScopes[0], actualScopes[1]);
                Assert.Equal(origScopes[1], actualScopes[2]);
                Assert.Equal(origScopes[2], actualScopes[3]);
            }

            [Fact]
            public void Should_end_and_start_a_new_block()
            {
                var origScopes = new List<Scope>(new[]
                        {
                            new Scope(ScopeName.TableRowBegin, 0, 1),
                            new Scope(ScopeName.TableCell, 1, 1),
                            new Scope(ScopeName.TableRowEnd, 2, 1),
                            new Scope(ScopeName.TableRowBegin, 10, 1),
                            new Scope(ScopeName.TableCell, 11, 1),
                            new Scope(ScopeName.TableRowEnd, 12, 1)
                        });

                var augmenter = new TableScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new TableMacro(), origScopes, null);

                Assert.Equal(10, actualScopes.Count);
                Assert.Equal(ScopeName.TableBegin, actualScopes[0].Name);
                Assert.Equal(0, actualScopes[0].Index);
                Assert.Equal(1, actualScopes[0].Length);
                Assert.Equal(ScopeName.TableEnd, actualScopes[4].Name);
                Assert.Equal(3, actualScopes[4].Index);
                Assert.Equal(0, actualScopes[4].Length);
                Assert.Equal(ScopeName.TableBegin, actualScopes[5].Name);
                Assert.Equal(10, actualScopes[5].Index);
                Assert.Equal(1, actualScopes[5].Length);
                Assert.Equal(ScopeName.TableEnd, actualScopes[9].Name);
                Assert.Equal(13, actualScopes[9].Index);
                Assert.Equal(0, actualScopes[9].Length);
            }

            [Fact]
            public void Should_contain_the_correct_captured_scopes_with_end_and_start_new_block()
            {
                var origScopes = new List<Scope>(new[]
                        {
                            new Scope(ScopeName.TableRowBegin, 0, 1),
                            new Scope(ScopeName.TableCell, 1, 1),
                            new Scope(ScopeName.TableRowEnd, 2, 1),
                            new Scope(ScopeName.TableRowBegin, 10, 1),
                            new Scope(ScopeName.TableCell, 11, 1),
                            new Scope(ScopeName.TableRowEnd, 12, 1)
                        });

                var augmenter = new TableScopeAugmenter();

                IList<Scope> actualScopes = augmenter.Augment(new TableMacro(), origScopes, null);

                Assert.Equal(10, actualScopes.Count);
                Assert.Equal(origScopes[0], actualScopes[1]);
                Assert.Equal(origScopes[1], actualScopes[2]);
                Assert.Equal(origScopes[2], actualScopes[3]);
                Assert.Equal(origScopes[3], actualScopes[6]);
                Assert.Equal(origScopes[4], actualScopes[7]);
                Assert.Equal(origScopes[5], actualScopes[8]);
            }
        }
    }
}