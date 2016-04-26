using System;
using System.Collections.Generic;
using System.Linq;

namespace Ccmi.OntoUml.Utilities.Collections
{
    /// <summary>
    /// List with upper and lower bounds on item count.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class BoundedList<T> : List<T>, IBoundedList<T>
    {
        public BoundedList(int? maxItems = null, int? minItems = null)
        {
            this.maxItems = maxItems;
            this.minItems = minItems;
        }

        private int? maxItems;
        public int? MaxItems
        {
            get
            {
                return maxItems;
            }
            set
            {
                if (value.HasValue && value < Count)
                    throw new InvalidOperationException("Cannot set MaxItems to number lower than the current number of items in the collection.");
                maxItems = value;
            }
        }

        private int? minItems;
        public int? MinItems {
            get
            {
                return minItems;
            }
            set
            {
                if (value.HasValue && value > Count)
                    throw new InvalidOperationException("Cannot set MinItems to number higher than the current number of items in the collection.");
                minItems = value;
            }
        }

        public new void Add(T item)
        {
            if (Count >= MaxItems)
                throw new InvalidOperationException("Max items limit exceeded.");
            base.Add(item);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            var valueList = collection.ToList();
            if (Count + valueList.Count > MaxItems)
                throw new InvalidOperationException("Max items limit exceeded.");
            base.AddRange(valueList);
        }

        public new void Insert(int index, T item)
        {
            if (Count >= MaxItems)
                throw new InvalidOperationException("Max items limit exceeded.");
            base.Insert(index, item);
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            var valueList = collection.ToList();
            if (Count + valueList.Count > MaxItems)
                throw new InvalidOperationException("Max items limit exceeded.");
            base.InsertRange(index, valueList);
        }

        public new void Remove(T item)
        {
            if (Count <= MinItems && Contains(item))
                throw new InvalidOperationException("Min items limit exceeded.");
            base.Remove(item);
        }

        public new int RemoveAll(Predicate<T> match)
        {
            var toRemove = FindAll(match);
            if (Count - toRemove.Count < MinItems)
                throw new InvalidOperationException("Min items limit exceeded.");
            return base.RemoveAll(match);
        }

        public new void RemoveAt(int index)
        {
            if (Count <= MinItems)
                throw new InvalidOperationException("Min items limit exceeded.");
            base.RemoveAt(index);
        }

        public new void RemoveRange(int index, int count)
        {
            if (Count - count < MinItems)
                throw new InvalidOperationException("Min items limit exceeded.");
            base.RemoveRange(index, count);
        }
    }
}