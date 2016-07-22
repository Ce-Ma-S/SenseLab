using System;
using System.ComponentModel;
using System.Threading;

namespace CeMaS.Common.Events
{
    /// <summary>
    /// Helps with events.
    /// </summary>
    public static class EventHelper
    {
        #region RaiseEvent
        
        public static bool RaiseEvent<T>(
            this EventHandler<T> handler,
            object sender,
            T arguments,
            SynchronizationContext context = null
            )
            where T : EventArgs
        {
            bool result = handler != null;
            if (result)
            {
                if (context != null)
                {
                    context.Post(
                        _ => handler(sender, arguments),
                        null
                        );
                }
                else
                {
                    handler(sender, arguments);
                }
            }
            return result;
        }
        public static bool RaiseEvent(
            this EventHandler handler,
            object sender,
            EventArgs arguments = null,
            SynchronizationContext context = null
            )
        {
            bool result = handler != null;
            if (result)
            {
                if (arguments == null)
                    arguments = EventArgs.Empty;
                if (context != null)
                {
                    context.Post(
                        _ => handler(sender, arguments),
                        null
                        );
                }
                else
                {
                    handler(sender, arguments);
                }
            }
            return result;
        }
        public static PropertyChangingEventArgs RaiseEvent(
            this PropertyChangingEventHandler handler,
            object sender,
            string propertyName
            )
        {
            PropertyChangingEventArgs a = null;
            handler?.Invoke(
                sender,
                a = new PropertyChangingEventArgs(propertyName)
                );
            return a;
        }
        public static PropertyChangedEventArgs RaiseEvent(
            this PropertyChangedEventHandler handler,
            object sender,
            string propertyName
            )
        {
            PropertyChangedEventArgs a = null;
            handler?.Invoke(
                sender,
                a = new PropertyChangedEventArgs(propertyName)
                );
            return a;
        }

        #endregion

        public static bool IsPropertyChanged(this EventArgs a, string propertyName)
        {
            return
                a is PropertyChangedEventArgs &&
                ((PropertyChangedEventArgs)a).PropertyName == propertyName;
        }
    }
}
