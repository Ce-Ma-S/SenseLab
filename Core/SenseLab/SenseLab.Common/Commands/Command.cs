using CeMaS.Common;
using CeMaS.Common.Commands;
using CeMaS.Common.Identity;
using SenseLab.Common.Objects;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public class Command<TParameter, TResult> :
        ObjectItem,
        ICommand
    {
        #region Init

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Func<TParameter, CancellationTokenSource, TResult> execute,
            IEnumerable<ValueInfo> parameters = null,
            IEnumerable<ValueInfo> results = null,
            Func<TParameter, bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            this(@object, id, name, parameters, results, description, values)
        {
            Value = new DelegateCommand<object[], object[]>(
                (p, c) => ToResult(execute(ToParameter(p), c)),
                p => CanExecute(p, canExecute),
                isSynchronous,
                allowsParallelExecution
                );
        }

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Func<TParameter, CancellationTokenSource, Task<TResult>> executeTaskFactory,
            IEnumerable<ValueInfo> parameters = null,
            IEnumerable<ValueInfo> results = null,
            Func<TParameter, bool> canExecute = null,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            this(@object, id, name, parameters, results, description, values)
        {
            Value = new DelegateCommand<object[], object[]>(
                async (p, c) => ToResult(await executeTaskFactory(ToParameter(p), c)),
                p => CanExecute(p, canExecute),
                allowsParallelExecution
                );
        }

        private Command(
            Objects.Object @object,
            string id,
            string name,
            IEnumerable<ValueInfo> parameters,
            IEnumerable<ValueInfo> results,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
        {
            Parameters = parameters == null ?
                new List<ValueInfo>() :
                new List<ValueInfo>(parameters);
            Results = results == null ?
                new List<ValueInfo>() :
                new List<ValueInfo>(results);
        }

        #endregion

        public DelegateCommand<object[], object[]> Value { get; }
        ICommand<object[], object[]> ICommand.Value
        {
            get { return Value; }
        }
        public List<ValueInfo> Parameters { get; }
        IReadOnlyList<IValueInfo> ICommand.Parameters
        {
            get { return Parameters; }
        }
        public List<ValueInfo> Results { get; }
        IReadOnlyList<IValueInfo> ICommand.Results
        {
            get { return Results; }
        }

        private static TParameter ToParameter(object[] parameter)
        {
            if (
                parameter != null &&
                parameter.Length > 0
                )
            {
                if (parameter.Length == 1)
                {
                    return (TParameter)parameter[0];
                }
                else if (typeof(TParameter) == typeof(object[]))
                {
                    return (TParameter)(object)parameter;
                }
                else if (typeof(TParameter).IsArray)
                {
                    var t = Array.CreateInstance(typeof(TParameter).GetElementType(), parameter.Length);
                    parameter.CopyTo(t, parameter.Length);
                    return (TParameter)(object)t;
                }
            }
            return default(TParameter);
        }

        private static object[] ToResult(TResult result)
        {
            if (
                result == null ||
                result is Unit
                )
            {
                return Array.Empty<object>();
            }
            else if (result is object[])
            {
                return (object[])(object)result;
            }
            else if (result is Array)
            {
                var s = (Array)(object)result;
                var t = new object[s.Length];
                Array.Copy(s, t, s.Length);
                return t;
            }
            return new object[] { result };
        }
        private bool CanExecute(object[] parameters, Func<TParameter, bool> canExecute)
        {
            return
                ParametersAreValid(parameters) &&
                (
                    canExecute == null ||
                    canExecute(ToParameter(parameters))
                );
        }

        private bool ParametersAreValid(object[] parameters)
        {
            return parameters == null &&
                Parameters.Count == 0 ||
                parameters != null &&
                Parameters.Count == parameters.Length &&
                TypesAreValid(Parameters, parameters);
        }

        private bool TypesAreValid(
            List<ValueInfo> parameterInfos,
            object[] parameters
            )
        {
            for (int i = 0; i < parameterInfos.Count; i++)
            {
                var parameterInfo = parameterInfos[i];
                var parameter = parameters[i];
                if (!parameter.Is(parameterInfo.Type))
                    return false;
            }
            return true;
        }
    }


    public class Command<TParameter> :
        Command<TParameter, Unit>
    {
        #region Init

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Action<TParameter, CancellationTokenSource> execute,
            IEnumerable<ValueInfo> parameters = null,
            Func<TParameter, bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(
                @object,
                id,
                name,
                (p, c) =>
                {
                    execute(p, c);
                    return Unit.Default;
                },
                parameters,
                null,
                canExecute,
                isSynchronous,
                allowsParallelExecution,
                description,
                values
                )
        { }

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Func<TParameter, CancellationTokenSource, Task> executeTaskFactory,
            IEnumerable<ValueInfo> parameters = null,
            Func<TParameter, bool> canExecute = null,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(
                @object,
                id,
                name,
                async (p, c) =>
                {
                    await executeTaskFactory(p, c);
                    return Unit.Default;
                },
                parameters,
                null,
                canExecute,
                allowsParallelExecution,
                description,
                values
                )
        { }

        #endregion
    }


    public class Command :
        Command<Unit>
    {
        #region Init

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Action<CancellationTokenSource> execute,
            Func<bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(
                @object,
                id,
                name,
                (p, c) => execute(c),
                null,
                p => canExecute(),
                isSynchronous,
                allowsParallelExecution,
                description,
                values
                )
        { }

        public Command(
            Objects.Object @object,
            string id,
            string name,
            Func<CancellationTokenSource, Task> executeTaskFactory,
            Func<bool> canExecute = null,
            bool allowsParallelExecution = false,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(
                @object,
                id,
                name,
                (p, c) => executeTaskFactory(c),
                null,
                p => canExecute(),
                allowsParallelExecution,
                description,
                values
                )
        { }

        #endregion
    }
}
