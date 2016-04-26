using System;
using System.Collections.Generic;
using System.Linq;

namespace Ccmi.OntoUml.Utilities.AssociationClasses
{
    /// <summary>
    /// Class representing M:N associations.
    /// </summary>
    /// <typeparam name="TM">Type of the "M" side of the association.</typeparam>
    /// <typeparam name="TN">Type of the "N" side of the association.</typeparam>
    public class ManyToManyAssociation<TM, TN>
        where TM : class
        where TN : class
    {
        private static ManyToManyAssociation<TM, TN> instance;
        private static readonly object locker = new object();
        private readonly bool allowDuplicates;

        private ManyToManyAssociation(bool allowDuplicates = false)
        {
            this.allowDuplicates = allowDuplicates;
        }

        public static ManyToManyAssociation<TM, TN> Instance(bool allowDuplicates = false)
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new ManyToManyAssociation<TM, TN>(allowDuplicates);
                    }
                }
            }

            if (allowDuplicates != instance.allowDuplicates)
                throw new InvalidOperationException("Requested different behaviour than the first time around.");

            return instance;
        }

        private readonly List<Tuple<TM, TN>> tuples = new List<Tuple<TM, TN>>();

        /// <summary>
        /// Creates links between the specified M and all of the specified Ns.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        public void CreateLink(TM m, params TN[] n)
        {
            if (n == null) return;
            foreach (var item in n)
                CreateLink(m, item);
        }

        /// <summary>
        /// Creates links between the specified M and N.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        private void CreateLink(TM m, TN n)
        {
            if (m == null || n == null) return;
            if (!allowDuplicates)
            {
                if (tuples.Any(pair => pair.Item1.Equals(m) && pair.Item2.Equals(n)))
                    throw new InvalidOperationException("Cannot add the same value pair more than once.");
            }
            tuples.Add(new Tuple<TM, TN>(m, n));
        }

        /// <summary>
        /// Removes the first link between the specified M and N.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        public void DestroyLink(TM m, TN n)
        {
            var toRemove = tuples.FirstOrDefault(l => l.Item1 == m && l.Item2 == n);
            if (toRemove != null)
                tuples.Remove(toRemove);
        }

        /// <summary>
        /// Gets all the Ns linked with the specified M.
        /// </summary>
        /// <param name="m"></param>
        /// <returns>Enumerable of the linked Ns.</returns>
        public IEnumerable<TN> GetNs(TM m) => tuples.Where(l => l.Item1 == m).Select(l => l.Item2);

        /// <summary>
        /// Gets all the Ms linked with the specified N.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Enumerable of the linked Ms.</returns>
        public IEnumerable<TM> GetMs(TN n) => tuples.Where(l => l.Item2 == n).Select(l => l.Item1);
    }
}