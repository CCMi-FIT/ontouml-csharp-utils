using Ccmi.OntoUml.Utilities.Collections;
using System;
using Xunit;

namespace Ccmi.OntoUml.Utilities.Tests.Collections
{
    public class BoundedListTests
    {
        [Fact]
        public void AddMaxLimitWorks()
        {
            var list = new BoundedList<string>(maxItems: 2);
            list.Add("x");
            list.Add("x");
            Assert.Throws<InvalidOperationException>(() => list.Add("x"));
        }

        [Fact]
        public void AddRangeMaxLimitWorks()
        {
            var list = new BoundedList<string>(maxItems: 2);
            Assert.Throws<InvalidOperationException>(() => list.AddRange(new[] { "a", "b", "c" }));
        }

        [Fact]
        public void InsertMaxLimitWorks()
        {
            var list = new BoundedList<string>(maxItems: 2);
            list.Insert(0, "c");
            list.Insert(0, "b");
            Assert.Throws<InvalidOperationException>(() => list.Insert(0, "a"));
        }

        [Fact]
        public void InsertRangeMaxLimitWorks()
        {
            var list = new BoundedList<string>(maxItems: 2);
            list.Add("a");
            Assert.Throws<InvalidOperationException>(() => list.InsertRange(0, new[] { "a", "b", "c" }));
        }

        [Fact]
        public void RemoveMinLimitWorks()
        {
            var list = new BoundedList<string>(minItems: 1)
            {
                "a", "b"
            };
            list.Remove("a");
            Assert.Throws<InvalidOperationException>(() => list.Remove("b"));
        }

        [Fact]
        public void RemoveAllLimitWorks()
        {
            var list = new BoundedList<string>(minItems: 2)
            {
                "a", "b", "b"
            };
            Assert.Throws<InvalidOperationException>(() => list.RemoveAll(i => i == "b"));
        }

        [Fact]
        public void RemoveAtLimitWorks()
        {
            var list = new BoundedList<string>(minItems: 1)
            {
                "a", "b"
            };
            list.RemoveAt(1);
            Assert.Throws<InvalidOperationException>(() => list.RemoveAt(0));
        }

        [Fact]
        public void RemoveRangeMinLimitWorks()
        {
            var list = new BoundedList<string>(minItems: 2)
            {
                "a", "b", "c"
            };
            Assert.Throws<InvalidOperationException>(() => list.RemoveRange(0, 2));
        }

        [Fact]
        public void SettingMaxItemsWorks()
        {
            var list = new BoundedList<string>(maxItems: 4)
            {
                "a", "b", "c"
            };
            Assert.Equal(4, list.MaxItems);
            list.MaxItems = 3;
            Assert.Equal(3, list.MaxItems);
            Assert.Throws<InvalidOperationException>(() => list.MaxItems = 2);
        }

        [Fact]
        public void SettingMinItemsWorks()
        {
            var list = new BoundedList<string>(minItems: 2)
            {
                "a", "b", "c"
            };
            Assert.Equal(2, list.MinItems);
            list.MinItems = 3;
            Assert.Equal(3, list.MinItems);
            Assert.Throws<InvalidOperationException>(() => list.MinItems = 4);
        }
    }
}
