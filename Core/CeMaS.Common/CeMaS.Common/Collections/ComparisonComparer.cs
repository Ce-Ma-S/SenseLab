using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// (Equality) comparer using a comparison.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class ComparisonComparer<T> :
        IComparer<T>,
        IEqualityComparer<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparison"><see cref="Comparison"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="comparison"/> is null.</exception>
        public ComparisonComparer(Comparison<T> comparison)
        {
            comparison.ValidateNonNull("comparison");
            Comparison = comparison;
        }

        /// <summary>
        /// Compares items.
        /// </summary>
        /// <value>non-null</value>
        public Comparison<T> Comparison { get; private set; }

        /// <summary>
        /// <see cref="Comparer{T}.Compare"/>
        /// </summary>
        public int Compare(T x, T y)
        {
            return Comparison(x, y);
        }
        /// <summary>
        /// <see cref="IEqualityComparer{T}.Equals"/>
        /// </summary>
        public bool Equals(T x, T y)
        {
            return Comparison(x, y) == 0;
        }
        /// <summary>
        /// <see cref="IEqualityComparer{T}.GetHashCode"/>
        /// </summary>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
