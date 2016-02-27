using System;

namespace CeMaS.Common.Events
{
    /// <summary>
    /// Data carrying event arguments.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public class EventArgs<T> :
        EventArgs
    {
        public EventArgs(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Event data.
        /// </summary>
        public T Data { get; private set; }
    }
}
