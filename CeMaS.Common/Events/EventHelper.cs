using CeMaS.Common.Validation;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;

namespace CeMaS.Common.Events
{
    /// <summary>
    /// Helps with events.
    /// </summary>
    public static class EventHelper
    {
        #region RaiseEvent
        
        public static T RaiseEvent<T>(this EventHandler<T> handler, object sender, Func<T> arguments,
            SynchronizationContext context = null)
            where T : EventArgs
        {
            T a = null;
            if (handler != null)
            {
                if (context != null)
                {
                    context.Post(
                        _ => handler(
                            sender,
                            a = arguments()
                        ),
                        null);
                }
                else
                {
                    handler(
                        sender,
                        a = arguments()
                        );
                }
            }
            return a;
        }
        public static EventArgs RaiseEvent(this EventHandler handler, object sender, Func<EventArgs> arguments = null,
            SynchronizationContext context = null)
        {
            EventArgs a = null;
            if (handler != null)
            {
                a = arguments == null ?
                    EventArgs.Empty :
                    arguments();
                if (context != null)
                {
                    context.Post(
                        _ => handler(sender, a),
                        null);
                }
                else
                {
                    handler(sender, a);
                }
            }
            return a;
        }
        public static PropertyChangedEventArgs RaiseEvent(this PropertyChangedEventHandler handler, object sender, string propertyName)
        {
            PropertyChangedEventArgs a = null;
            if (handler != null)
            {
                handler(
                    sender,
                    a = new PropertyChangedEventArgs(propertyName)
                    );
            }
            return a;
        }
        public static PropertyChangedEventArgs RaiseEvent(this PropertyChangedEventHandler handler, object sender, Expression<Func<object>> property)
        {
            PropertyChangedEventArgs a = null;
            if (handler != null)
            {
                handler(
                    sender,
                    a = new PropertyChangedEventArgs(property.PropertyName())
                    );
            }
            return a;
        }

        #endregion

        public static bool IsPropertyChanged<I, V>(this EventArgs a, Expression<Func<I, V>> property)
        {
            return
                a is PropertyChangedEventArgs &&
                ((PropertyChangedEventArgs)a).HasProperty(property);
        }
        public static bool HasProperty<I, V>(this PropertyChangedEventArgs a, Expression<Func<I, V>> property)
        {
            a.ValidateNonNull(nameof(a));
            var propertyName = property.PropertyName();
            return a.PropertyName == propertyName;
        }
    }
}
