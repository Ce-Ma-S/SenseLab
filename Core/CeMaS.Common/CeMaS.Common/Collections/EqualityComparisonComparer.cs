using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Equality comparer using a comparison.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class EqualityComparisonComparer<T> :
        IEqualityComparer<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparisonMethod"><see cref="ComparisonMethod"/></param>
        /// <param name="getHashCodeMethod"><see cref="GetHashCodeMethod"/>. If null, <see cref="Object.GetHashCode"/> is used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparisonMethod"/> is null.</exception>
        public EqualityComparisonComparer(
            Func<T, T, bool> comparisonMethod,
            Func<T, int> getHashCodeMethod = null
            )
        {
            comparisonMethod.ValidateNonNull("comparisonMethod");
            ComparisonMethod = comparisonMethod;
            if (getHashCodeMethod == null)
                getHashCodeMethod = obj => obj.GetHashCode();
            GetHashCodeMethod = getHashCodeMethod;
        }

        /// <summary>
        /// Compares items equality.
        /// </summary>
        /// <value>non-null</value>
        public Func<T, T, bool> ComparisonMethod { get; private set; }
        /// <summary>
        /// Gets item hash code.
        /// </summary>
        /// <value>non-null</value>
        public Func<T, int> GetHashCodeMethod { get; private set; }

        /// <summary>
        /// <see cref="IEqualityComparer{T}.Equals"/>
        /// </summary>
        public bool Equals(T x, T y)
        {
            return ComparisonMethod(x, y);
        }
        /// <summary>
        /// <see cref="IEqualityComparer{T}.GetHashCode"/>
        /// </summary>
        public int GetHashCode(T obj)
        {
            return GetHashCodeMethod(obj);
        }
    }
}
