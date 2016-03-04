using System;
using System.Linq;
using System.Reflection;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Gets message from <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Exception.</param>
        public static string Message(this Exception e)
        {
            if (e is TargetInvocationException)
                return e.InnerException.Message;
            else if (e is AggregateException)
                return string.Join("\n", ((AggregateException)e).InnerExceptions.Select(ee => Message(ee)));
            else
                return e.Message;
        }
    }
}
