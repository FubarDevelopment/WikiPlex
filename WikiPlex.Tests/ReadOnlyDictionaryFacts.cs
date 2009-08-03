using System;
using System.Collections.Generic;
using WikiPlex.Common;
using Xunit;

namespace WikiPlex.Tests
{
    public class ReadOnlyDictionaryFacts
    {
        [Fact]
        public void Will_yield_correct_enumeration()
        {
            IDictionary<string, string> expected = new Dictionary<string, string> {{"a", "a"}, {"b", "b"}};
            IDictionary<string, string> dictionary = new ReadOnlyDictionary<string, string>(expected);
            IEnumerator<KeyValuePair<string, string>> expectedEnum = expected.GetEnumerator();
            IEnumerator<KeyValuePair<string, string>> actualEnum = dictionary.GetEnumerator();

            while (expectedEnum.MoveNext())
            {
                actualEnum.MoveNext();

                Assert.Equal(expectedEnum.Current, actualEnum.Current);
            }
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_adding()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            Exception ex = Record.Exception(() => dictionary.Add("a", "a"));

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_adding_key_value_pair()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            Exception ex = Record.Exception(() => dictionary.Add(new KeyValuePair<string, string>("a", "a")));

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_clearing()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            Exception ex = Record.Exception(() => dictionary.Clear());

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_removing()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            Exception ex = Record.Exception(() => dictionary.Remove("a"));

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_removing_key_value_pair()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            Exception ex = Record.Exception(() => dictionary.Remove(new KeyValuePair<string, string>()));

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_yield_correct_result_for_contains()
        {
            var pair = new KeyValuePair<string, string>("a", "a");
            IDictionary<string, string> origDictionary = new Dictionary<string, string>();
            origDictionary.Add(pair);
            var dictionary = new ReadOnlyDictionary<string, string>(origDictionary);

            bool result = dictionary.Contains(pair);

            Assert.True(result);
        }

        [Fact]
        public void Will_yield_correct_result_for_copyto()
        {
            var pair = new KeyValuePair<string, string>("a", "a");
            IDictionary<string, string> orig = new Dictionary<string, string>();
            orig.Add(pair);
            var dictionary = new ReadOnlyDictionary<string, string>(orig);
            var output = new KeyValuePair<string, string>[1];

            dictionary.CopyTo(output, 0);

            Assert.Equal(1, output.Length);
            Assert.Equal(pair, output[0]);
        }

        [Fact]
        public void Will_yield_correct_result_for_count()
        {
            var orig = new Dictionary<string, string> {{"a", "a"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            int count = dictionary.Count;

            Assert.Equal(1, count);
        }

        [Fact]
        public void Will_yield_true_for_isreadonly()
        {
            var dictionary = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

            bool result = dictionary.IsReadOnly;

            Assert.True(result);
        }

        [Fact]
        public void Will_yield_correct_result_for_containskey()
        {
            var orig = new Dictionary<string, string> {{"a", "a"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            bool result = dictionary.ContainsKey("a");

            Assert.True(result);
        }

        [Fact]
        public void Will_yield_correct_result_for_trygetvalue()
        {
            var orig = new Dictionary<string, string> {{"a", "b"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);
            string output;

            bool result = dictionary.TryGetValue("a", out output);

            Assert.True(result);
            Assert.Equal("b", output);
        }

        [Fact]
        public void Will_yield_correct_value_for_indexer_get()
        {
            var orig = new Dictionary<string, string> {{"a", "b"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            string result = dictionary["a"];

            Assert.Equal("b", result);
        }

        [Fact]
        public void Will_throw_NotSupportedException_when_setting_indexer()
        {
            var orig = new Dictionary<string, string>();
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            Exception ex = Record.Exception(() => dictionary["a"] = "b");

            Assert.IsType<NotSupportedException>(ex);
        }

        [Fact]
        public void Will_yield_correct_list_of_keys()
        {
            var orig = new Dictionary<string, string> {{"a", "a"}, {"b", "b"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            ICollection<string> result = dictionary.Keys;

            Assert.Equal(2, result.Count);
            Assert.Contains("a", result);
            Assert.Contains("b", result);
        }

        [Fact]
        public void Will_yield_correct_list_of_values()
        {
            var orig = new Dictionary<string, string> {{"a", "a"}, {"b", "b"}};
            var dictionary = new ReadOnlyDictionary<string, string>(orig);

            ICollection<string> result = dictionary.Values;

            Assert.Equal(2, result.Count);
            Assert.Contains("a", result);
            Assert.Contains("b", result);
        }
    }
}