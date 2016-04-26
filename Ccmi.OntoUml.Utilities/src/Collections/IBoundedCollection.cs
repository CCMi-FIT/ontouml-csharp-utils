using System.Collections.Generic;

namespace Ccmi.OntoUml.Utilities.Collections
{
    /// <summary>
    /// Collection with upper and lower bounds on item count.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public interface IBoundedCollection<T> : ICollection<T>
    {
        int? MaxItems { get; set; }
        int? MinItems { get; set; }
    }
}