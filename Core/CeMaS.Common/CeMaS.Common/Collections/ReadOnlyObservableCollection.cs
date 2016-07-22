using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    public class ReadOnlyObservableCollection<T> :
        System.Collections.ObjectModel.ReadOnlyObservableCollection<T>,
        INotifyList<T>
    {
        public ReadOnlyObservableCollection(ObservableCollection<T> list)
            : base(list)
        {
        }

        public IObservable<IEnumerable<T>> Adding
        {
            get { return ((ObservableCollection<T>)Items).Adding; }
        }
        public IObservable<IEnumerable<T>> Added
        {
            get { return ((ObservableCollection<T>)Items).Added; }
        }
        public IObservable<IEnumerable<T>> Removing
        {
            get { return ((ObservableCollection<T>)Items).Removing; }
        }
        public IObservable<IEnumerable<T>> Removed
        {
            get { return ((ObservableCollection<T>)Items).Removed; }
        }

        public void Add(IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
        public void Insert(int index, IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
        public void ReplaceWith(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }
        public bool Remove(IEnumerable<T> items)
        {
            throw new InvalidOperationException();
        }
    }
}
