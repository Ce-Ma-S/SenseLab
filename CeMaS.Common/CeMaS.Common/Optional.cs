﻿using System;
using System.Collections.Generic;

namespace CeMaS.Common
{
    /// <summary>
    /// Optional value.
    /// </summary>
    /// <remarks><typeparamref name="T"/>Nullable (?) alternative for both reference and value types.</remarks>
    /// <typeparam name="T">Value type.</typeparam>
    public struct Optional<T>
    {
        public Optional(T value)
            : this()
        {
            HasValue = true;
            Value = value;
        }

        public static readonly Optional<T> None = new Optional<T>();

        public bool HasValue { get; private set; }
        public bool HasNonDefaultValue
        {
            get { return HasValue && !EqualityComparer<T>.Default.Equals(Value, default(T)); }
        }
        public T Value { get; private set; }

        public static bool operator ==(Optional<T> value1, Optional<T> value2)
        {
            return
                value1.HasValue == value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2.Value);
        }
        public static bool operator !=(Optional<T> value1, Optional<T> value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(Optional<T> value1, T value2)
        {
            return
                value1.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2);
        }
        public static bool operator !=(Optional<T> value1, T value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(T value1, Optional<T> value2)
        {
            return
                value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1, value2.Value);
        }
        public static bool operator !=(T value1, Optional<T> value2)
        {
            return !(value1 == value2);
        }
        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }
        public static explicit operator T(Optional<T> value)
        {
            if (!value.HasValue)
                throw new InvalidCastException("No value exists.");
            return value.Value;
        }

        public T ValueOrDefault(T defaultValue = default(T))
        {
            return HasValue ?
                Value :
                defaultValue;
        }
        public override bool Equals(object obj)
        {
            if (obj is Optional<T>)
            {
                return this == (Optional<T>)obj;
            }
            else if (obj is T)
            {
                return this == (T)obj;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HasValue ?
                int.MinValue :
                Value.GetHashCode();
        }
    }
}
