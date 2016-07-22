using CeMaS.Common.Collections;
using CeMaS.Common.Diagnostics;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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

        #region Properties

        /// <summary>
        /// Logs properties of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="log">Log.</param>
        /// <param name="level">Level.</param>
        /// <param name="name">Object name.</param>
        /// <param name="instance">Object instance. null for static properties.</param>
        /// <param name="propertyFilter">Optinal property filter.</param>
        /// <param name="propertyNamePrefix">Optional property name prefix. If empty, "<paramref name="name"/>." is used.</param>
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
        /// <param name="propertyNamePrefix">Optional property name prefix. If empty, "<paramref name="name"/>." is used.</param>
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
            var properties = type.GetProperties().
                Where(i => i.GetIndexParameters().Length == 0). // non-indexed
                Where(i =>
                    instance == null ?
                        i.GetMethod.IsStatic :  // static
                        !i.GetMethod.IsStatic   // instance
                        );
            if (propertyFilter != null)
                properties = properties.Where(propertyFilter);
            if (orderByPropertyName)
                properties = properties.OrderBy(i => i.Name);
            if (propertyNamePrefix == string.Empty)
                propertyNamePrefix = ItemPropertyName(name, null);
            var propertyNamesTemplate = string.Join(PropertyNameSeparator, properties.Select(i => $"{{{propertyNamePrefix + i.Name}}}"));
            string message = $"{name}: {propertyNamesTemplate}";
            var propertyValues = properties.Select(i => i.GetValue(instance)).ToArray();
            log.Write(level, message, propertyValues);
        }

        public static string ItemPropertyName(string itemName, string propertyName)
        {
            return itemName + '_' + propertyName;
        }
        public static string ItemPropertyName<T>(string propertyName)
        {
            return ItemPropertyName(typeof(T).Name(true), propertyName);
        }
        public static string ItemPropertyName<T>(Expression<Func<T>> property)
        {
            Argument.NonNull(property, nameof(property));
            var info = property.PropertyInfo();
            return ItemPropertyName(info.DeclaringType.Name(true), info.Name);
        }

        public static ILogger ForContext<T>(this ILogger logger, Expression<Func<T>> property)
        {
            Argument.NonNull(logger, nameof(logger));
            return logger.ForContext(ItemPropertyName(property), property.Compile()());
        }
        public static ILogger WithIdOf<T>(this ILogger logger, IId<T> id)
        {
            Argument.NonNull(logger, nameof(logger));
            Argument.NonNull(id, nameof(id));
            return logger.ForContext(nameof(IId<T>.Id), id.Id);
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

        /// <summary>
        /// Logs <paramref name="error"/>.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="error">Error.</param>
        /// <param name="name">Caller name.</param>
        public static void Failed(
            this ILogger log,
            Exception error,
            [CallerMemberName] string name = null
            )
        {
            log.Error(error, $"{name} failed.");
        }

        /// <summary>
        /// Logs unhandled <paramref name="error"/>.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="error">Error.</param>
        /// <param name="message">Optional error message.</param>
        public static void Unhandled(
            this ILogger log,
            Exception error,
            string message = null
            )
        {
            const string unhandledError = "Unhandled error";
            if (message == null)
                log.Fatal(error, unhandledError);
            else
                log.Fatal(error, $"{unhandledError}: {{Message}}", message);
        }

        #endregion

        #region Diagnostics

        /// <summary>
        /// Measures time elapsed from this call until the returned object is disposed and logs it.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="message">Action title.</param>
        /// <param name="onStopMessage">Optional message to be appended on stop.</param>
        /// <param name="level">Level.</param>
        /// <param name="arguments">Optional message arguments.</param>
        /// <returns>Object which stops measurement when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="log"/> is null.</exception>
        public static IDisposable MeasureTime(
            this ILogger log,
            [CallerMemberName] string message = null,
            Func<string> onStopMessage = null,
            LogEventLevel level = LogEventLevel.Debug,
            params object[] arguments
            )
        {
            Validate(log);
            if (message == null)
                message = Guid.NewGuid().ToString();
            message = $"{message}: {{Action}}";
            return DiagnosticsHelper.MeasureTime(
                () =>
                {
                    log.Write(
                        level,
                        message,
                        arguments.Concat("start").ToArray()
                        );
                    return null;
                },
                (state, elapsedTime) =>
                {
                    message = message + $", {elapsedTimeProperty}";
                    if (onStopMessage != null)
                        message += '\n' + onStopMessage();
                    log.Write(
                        level,
                        message,
                        arguments.Concat("stop", elapsedTime).ToArray()
                        );
                });
        }

        public static void RunAndMeasureTime(
            this ILogger log,
            Action action,
            [CallerMemberName] string message = null,
            LogEventLevel level = LogEventLevel.Debug
            )
        {
            Argument.NonNull(action, nameof(action));
            using (log.MeasureTime())
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    log.Failed(e);
                    throw;
                }
            }
        }

        public static async Task RunAndMeasureTimeAsync(
            this ILogger log,
            Func<Task> action,
            [CallerMemberName] string message = null,
            LogEventLevel level = LogEventLevel.Debug
            )
        {
            Argument.NonNull(action, nameof(action));
            using (log.MeasureTime())
            {
                try
                {
                    await action();
                }
                catch (Exception e)
                {
                    log.Failed(e);
                    throw;
                }
            }
        }

        public static T RunAndMeasureTime<T>(
            this ILogger log,
            Func<T> function,
            [CallerMemberName] string message = null,
            LogEventLevel level = LogEventLevel.Debug
            )
        {
            Argument.NonNull(function, nameof(function));
            using (log.MeasureTime())
            {
                try
                {
                    return function();
                }
                catch (Exception e)
                {
                    log.Failed(e);
                    throw;
                }
            }
        }

        public static async Task<T> RunAndMeasureTimeAsync<T>(
            this ILogger log,
            Func<Task<T>> function,
            [CallerMemberName] string message = null,
            LogEventLevel level = LogEventLevel.Debug
            )
        {
            Argument.NonNull(function, nameof(function));
            using (log.MeasureTime())
            {
                try
                {
                    return await function();
                }
                catch (Exception e)
                {
                    log.Failed(e);
                    throw;
                }
            }
        }

        public static TimeSpan[] RunAndMeasureTime(
            this ILogger log,
            Action action,
            int repeatCount,
            bool parallel,
            [CallerMemberName] string message = null,
            LogEventLevel level = LogEventLevel.Debug
            )
        {
            Validate(log);
            if (message == null)
                message = Guid.NewGuid().ToString();
            var times = action.RunAndMeasureTime(repeatCount, parallel);
            if (times.Length == 1)
            {
                log.Write(level, $"{message}: {elapsedTimeProperty}", times[0]);
            }
            else
            {
                var totalTime = times.Aggregate((t1, t2) => t1 + t2);
                var averageTime = TimeSpan.FromTicks(totalTime.Ticks / times.Length);
                log.Write(level, $"{message}: {{RepeatCount}} x, {elapsedTimeProperty}, {{AverageTime}}",
                    repeatCount, totalTime, averageTime);
            }
            return times;
        }

        private static void Validate(ILogger log)
        {
            Argument.NonNull(log, nameof(log));
        }

        private const string elapsedTimeProperty = "{ElapsedTime}";

        #endregion
    }
}
