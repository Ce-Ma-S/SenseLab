using System;

namespace SenseLab.Common.Values
{
    public abstract class ValueInfo<T> :
        Item<string>,
        IValueInfoWritable
    {
        public ValueInfo(
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }

        public Type Type
        {
            get { return typeof(T); }
        }
    }
}
