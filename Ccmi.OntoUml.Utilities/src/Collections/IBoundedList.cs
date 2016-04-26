using System.Collections.Generic;

namespace Ccmi.OntoUml.Utilities.Collections
{
    /// <summary>
    /// List with upper and lower bounds on item count.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public interface IBoundedList<T> : IList<T>, IBoundedCollection<T>
    {
    }
}