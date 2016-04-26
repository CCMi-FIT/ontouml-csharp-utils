using System;
using System.Collections.Generic;
using System.Linq;

namespace Ccmi.OntoUml.Utilities.AssociationClasses
{
    /// <summary>
    /// Class representing 1:N associations.
    /// </summary>
    /// <typeparam name="TOne">Type of the "1" side of the association.</typeparam>
    /// <typeparam name="TMany">Type of the "N" side of the association.</typeparam>
    public class OneToManyAssociation<TOne, TMany>
        where TOne : class
        where TMany : class
    {
        private static OneToManyAssociation<TOne, TMany> instance;
        private static readonly object locker = new object();
        private readonly bool allowDuplicates;
        private OneToManyAssociation(bool allowDuplicates = false)
        {
            this.allowDuplicates = allowDuplicates;
        }

        public static OneToManyAssociation<TOne, TMany> Instance(bool allowDuplicates = false)
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new OneToManyAssociation<TOne, TMany>(allowDuplicates);
                    }
                }
            }

            if (allowDuplicates != instance.allowDuplicates)
                throw new InvalidOperationException("Requested different behaviour than the first time around.");

            return instance;
        }

        private readonly Dictionary<TOne, List<TMany>> tuples = new Dictionary<TOne, List<TMany>>();

        /// <summary>
        /// Creates a link between the specified TOne and all of the specified TManys.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="many"></param>
        public void CreateLink(TOne one, params TMany[] many)
        {
            if (many == null) return;
            foreach (var item in many)
                CreateLink(one, item);
        }

        /// <summary>
        /// Creates a link between the specified TOne and TMany.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="many"></param>
        private void CreateLink(TOne one, TMany many)
        {
            if (one == null || many == null) return;
            var containing = tuples.FirstOrDefault(i => i.Value.Contains(many));
            if (containing.Key != null)
            {
                if (containing.Key.Equals(one))
                    throw new InvalidOperationException("Cannot add value to more than one item.");
                if (!allowDuplicates)
                    throw new InvalidOperationException("Cannot add the same value more than once.");
            }
            List<TMany> current;
            if (!tuples.TryGetValue(one, out current))
                tuples[one] = new List<TMany>();
            tuples[one].Add(many);
        }

        /// <summary>
        /// Removes the first link between the specified TOne and TMany.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="many"></param>
        public void DestroyLink(TOne one, TMany many)
        {
            if (one == null) return;
            List<TMany> current;
            if (tuples.TryGetValue(one, out current))
            {
                current.Remove(many);
                if (!current.Any()) // if there are no TManys for the item, remove it as well
                    tuples.Remove(one);
            }
        }

        /// <summary>
        /// Gets the TOne linked to the specified TMany.
        /// </summary>
        /// <param name="many"></param>
        /// <returns>The linked TOne.</returns>
        public TOne GetOne(TMany many) => tuples.SingleOrDefault(i => i.Value.Contains(many)).Key;

        /// <summary>
        /// Gets all the TManys linked to the specified TOne.
        /// </summary>
        /// <param name="one"></param>
        /// <returns>Enumerable of linked TManys.</returns>
        public IEnumerable<TMany> GetMany(TOne one)
        {
            List<TMany> result;
            return !tuples.TryGetValue(one, out result) ? Enumerable.Empty<TMany>() : result;
        }
    }
}