using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common
{
    public class Item<T> :
        NotifyPropertyChange,
        IItemWritable<T>
        where T : IEquatable<T>
    {
        public Item(
            T id,
            string name,
            string description = null
            )
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public T Id { get; }

        #region Name

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name.ValidateNonEmpty();
                SetValue(() => Name, ref name, value, OnNameChanged);
            }
        }

        protected virtual void OnNameChanged()
        {
        }

        private string name;

        #endregion

        #region Description

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetValue(() => Description, ref description, value, OnDescriptionChanged);

            }
        }

        protected virtual void OnDescriptionChanged()
        {
        }

        private string description;

        #endregion
    }
}
