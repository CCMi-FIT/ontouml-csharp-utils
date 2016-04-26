using System;
using System.Collections.Generic;
using System.Linq;

namespace Ccmi.OntoUml.Utilities.AssociationClasses
{
    /// <summary>
    /// Class representing M:N associations.
    /// </summary>
    /// <typeparam name="TLeft">Type of the left-hand side of the association.</typeparam>
    /// <typeparam name="TRight">Type of the right-hand side of the association.</typeparam>
    public class OneToOneAssociation<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private static readonly Lazy<OneToOneAssociation<TLeft, TRight>> instance = new Lazy<OneToOneAssociation<TLeft, TRight>>(() => new OneToOneAssociation<TLeft, TRight>());

        private OneToOneAssociation()
        {
        }

        public static OneToOneAssociation<TLeft, TRight> Instance()
        {
            return instance.Value;
        }

        private readonly List<Tuple<TLeft, TRight>> tuples = new List<Tuple<TLeft, TRight>>();

        /// <summary>
        /// Creates a link between the specified Left and Right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void CreateLink(TLeft left, TRight right)
        {
            if (left == null || right == null) return;
            if (tuples.Any(i => i.Item1 == left))
                throw new InvalidOperationException("Cannot add value to more than one item.");
            if (tuples.Any(i => i.Item2 == right))
                throw new InvalidOperationException("Cannot add value to more than one item.");
            tuples.Add(new Tuple<TLeft, TRight>(left, right));
        }

        /// <summary>
        /// Removes the link containing the specified Left and Right.
        /// </summary>
        /// <param name="left"></param>
        public void DestroyLink(TLeft left, TRight right)
        {
            var toRemove = tuples.FirstOrDefault(l => l.Item1 == left && l.Item2 == right);
            if (toRemove != null)
                tuples.Remove(toRemove);
        }

        /// <summary>
        /// Gets the Right linked to the specified Left.
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public TRight GetRight(TLeft left) => tuples.FirstOrDefault(l => l.Item1 == left)?.Item2;

        /// <summary>
        /// Gets the Left linked to the specified Right.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public TLeft GetLeft(TRight right) => tuples.FirstOrDefault(l => l.Item2 == right)?.Item1;
    }
}