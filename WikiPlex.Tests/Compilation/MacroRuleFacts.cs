using System;
using System.Collections.Generic;
using WikiPlex.Compilation;
using Xunit;

namespace WikiPlex.Tests.Compilation
{
    public class MacroRuleFacts
    {
        public class Constructor
        {
            [Fact]
            public void Should_throw_ArgumentNullException_when_regex_is_null()
            {
                var captures = new Dictionary<int, string> {{0, "foo"}};

                Exception ex = Record.Exception(() => new MacroRule(null, captures));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_regex_is_empty()
            {
                var captures = new Dictionary<int, string> {{0, "foo"}};

                Exception ex = Record.Exception(() => new MacroRule(string.Empty, captures));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("regex", ((ArgumentException) ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_captures_is_null()
            {
                const string regex = "foo";

                Exception ex = Record.Exception(() => new MacroRule(regex, (IDictionary<int, string>) null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("captures", ((ArgumentNullException) ex).ParamName);
            }

            [Fact]
            public void Should_properly_set_regex_and_captures()
            {
                const string regex = "foo";
                var captures = new Dictionary<int, string> {{0, "foo"}};

                var rule = new MacroRule(regex, captures);

                Assert.Equal(regex, rule.Regex);
                Assert.Equal(captures, rule.Captures);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_only_setting_regex_and_is_null()
            {
                Exception ex = Record.Exception(() => new MacroRule(null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_only_setting_regex_and_is_empty()
            {
                Exception ex = Record.Exception(() => new MacroRule(string.Empty));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("regex", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_correctly_set_regex_when_only_setting_regex()
            {
                const string regex = "foo";

                var rule = new MacroRule(regex);

                Assert.Equal(regex, rule.Regex);
                Assert.Empty(rule.Captures);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_setting_first_scope_and_regex_is_null()
            {
                Exception ex = Record.Exception(() => new MacroRule(null, "capture"));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("regex", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_setting_first_scope_and_regex_is_empty()
            {
                Exception ex = Record.Exception(() => new MacroRule(string.Empty, "capture"));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("regex", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_setting_first_scope_and_is_null()
            {
                const string regex = "foo";

                Exception ex = Record.Exception(() => new MacroRule(regex, (string) null));

                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("firstScopeCapture", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_setting_first_scope_and_is_empty()
            {
                const string regex = "foo";

                Exception ex = Record.Exception(() => new MacroRule(regex, string.Empty));

                Assert.IsType<ArgumentException>(ex);
                Assert.Equal("firstScopeCapture", ((ArgumentException)ex).ParamName);
            }

            [Fact]
            public void Should_correctly_set_regex_and_first_scope_capture()
            {
                const string regex = "foo";

                var rule = new MacroRule(regex, "scope");

                Assert.Equal(regex, rule.Regex);
                Assert.Equal(1, rule.Captures.Count);
                Assert.Equal("scope", rule.Captures[1]);
            }
        }
    }
}