using CeMaS.Common.Diagnostics;
using CeMaS.Common.Validation;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Reflection;

namespace CeMaS.Common.Logging
{
    public static class LogHelper
    {
        #region Configure

        public static LoggerConfiguration EnrichWithApplication(this LoggerConfiguration configuration, string name)
        {
            return configuration.Enrich.WithProperty("Application", name);
        }

        #endregion

        #region Write

        /// <summary>
        /// Logs properties of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="log">Log.</param>
        /// <param name="level">Level.</param>
        /// <param name="name">Object name.</param>
        /// <param name="instance">Object instance. null for static properties.</param>
        /// <param name="propertyFilter">Optinal property filter.</param>
        /// <param name="propertyNamePrefix">Optional property name prefix. If empty, <paramref name="name"/> is used.</param>
        /// <param name="orderByPropertyName">Whether to order properties by names.</param>
        public static void WritePropertiesOf<T>(
            this ILogger log,
            LogEventLevel level,
            string name,
            T instance = default(T),
            Func<PropertyInfo, bool> propertyFilter = null,
            string propertyNamePrefix = null,
            bool orderByPropertyName = false
            )
        {
            var type = typeof(T);
            log.WritePropertiesOf(type, level, name, instance, propertyFilter, propertyNamePrefix, orderByPropertyName);
        }

        /// <summary>
        /// Logs properties of <paramref name="type"/>.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="type">Object type.</param>
        /// <param name="level">Level.</param>
        /// <param name="name">Object name.</param>
        /// <param name="instance">Object instance. null for static properties.</param>
        /// <param name="propertyFilter">Optinal property filter.</param>
        /// <param name="propertyNamePrefix">Optional property name prefix. If empty, <paramref name="name"/> is used.</param>
        /// <param name="orderByPropertyName">Whether to order properties by names.</param>
        public static void WritePropertiesOf(
            this ILogger log,
            Type type,
            LogEventLevel level,
            string name,
            object instance = null,
            Func<PropertyInfo, bool> propertyFilter = null,
            string propertyNamePrefix = null,
            bool orderByPropertyName = false
            )
        {
            var properties = type.GetTypeInfo().DeclaredProperties;
            properties = properties.Where(i => i.GetIndexParameters().Length == 0); // non-indexed
            properties = properties.Where(i =>
                instance == null ?
                    i.GetMethod.IsStatic :  // static
                    !i.GetMethod.IsStatic   // instance
                    );
            if (propertyFilter != null)
                properties = properties.Where(propertyFilter);
            if (orderByPropertyName)
                properties = properties.OrderBy(i => i.Name);
            if (propertyNamePrefix == string.Empty)
                propertyNamePrefix = name;
            var propertyNamesTemplate = string.Join(PropertyNameSeparator, properties.Select(i => $"{{{propertyNamePrefix + i.Name}}}"));
            string message = $"{name}: {propertyNamesTemplate}";
            var propertyValues = properties.Select(i => i.GetValue(instance)).ToArray();
            log.Write(level, message, propertyValues);
        }

        public static Func<PropertyInfo, bool> IncludeProperties(params string[] names)
        {
            return i => names.Contains(i.Name);
        }
        public static Func<PropertyInfo, bool> ExcludeProperties(params string[] names)
        {
            return i => !names.Contains(i.Name);
        }

        private const string PropertyNameSeparator = ", ";

        #endregion

        #region Diagnostics

        /// <summary>
        /// Measures time elapsed from this call until the returned object is disposed and logs it.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="message">Action title. If null, guid is generated.</param>
        /// <param name="onStopMessage">Optional message to be appended on stop.</param>
        /// <returns>Object which stops measurement when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is null.</exception>
        public static IDisposable MeasureTime(
            this ILogger log,
            string message = null,
            Func<string> onStopMessage = null
            )
        {
            Validate(log);
            if (message == null)
                message = Guid.NewGuid().ToString();
            message = $"{message}: {{Action}}";
            return DiagnosticsHelper.MeasureTime(
                () =>
                {
                    log.Debug(message, "start");
                    return null;
                },
                (state, elapsedTime) =>
                {
                    message = message + $", {elapsedTimeProperty}";
                    if (onStopMessage != null)
                        message += '\n' + onStopMessage();
                    log.Debug(message, "stop", elapsedTime);
                });
        }

        public static TimeSpan[] RunAndMeasureTime(
            this ILogger log,
            Action action,
            int repeatCount,
            bool parallel,
            string message = null
            )
        {
            Validate(log);
            if (message == null)
                message = Guid.NewGuid().ToString();
            var times = action.RunAndMeasureTime(repeatCount, parallel);
            if (times.Length == 1)
            {
                log.Debug($"{message}: {elapsedTimeProperty}", times[0]);
            }
            else
            {
                var totalTime = times.Aggregate((t1, t2) => t1 + t2);
                var averageTime = TimeSpan.FromTicks(totalTime.Ticks / times.Length);
                log.Debug($"{message}: {{RepeatCount}} x, {elapsedTimeProperty}, {{AverageTime}}",
                    repeatCount, totalTime, averageTime);
            }
            return times;
        }

        private static void Validate(ILogger log)
        {
            log.ValidateNonNull(nameof(log));
        }

        private const string elapsedTimeProperty = "{ElapsedTime}";

        #endregion
    }
}
