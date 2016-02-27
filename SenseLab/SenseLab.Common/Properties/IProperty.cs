using CeMaS.Common.Events;
using SenseLab.Common.Objects;
using SenseLab.Common.Values;
using System;

namespace SenseLab.Common.Properties
{
    public interface IProperty :
        IObjectItem,
        IValueInfo
    {
        bool HasValue { get; }
        object Value { get; }
        event EventHandler<ValueChangeEventArgs> ValueChanged;
    }


    public interface IProperty<T> :
        IProperty
    {
        new T Value { get; }
        new event EventHandler<ValueChangeEventArgs<T>> ValueChanged;
    }
}
