using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public class DelegateCommand :
        Command
    {
        #region Init

        #region Any parameters

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Action<object[]> execute,
            Func<object[], bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name, false,
                async (c, p) => execute(p),
                canExecute,
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<object[], Task> execute,
            Func<object[], bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name, false,
                (c, p) => execute(p),
                canExecute,
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<CancellationToken, object[], Task> execute,
            Func<object[], bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name, true,
                (c, p) => execute(c.Value, p),
                canExecute,
                description, parameters
                )
        {
        }

        #endregion

        #region No parameters

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Action execute,
            Func<bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name,
                p => execute(),
                p => canExecute(),
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<Task> execute,
            Func<bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name,
                p => execute(),
                p => canExecute(),
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<CancellationToken, Task> execute,
            Func<bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name,
                (c, p) => execute(c),
                p => canExecute(),
                description, parameters
                )
        {
        }

        #endregion

        private DelegateCommand(
            IObject @object,
            string id,
            string name,
            bool isCancellable,
            Func<CancellationToken?, object[], Task> execute,
            Func<object[], bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            base(@object, id, name, isCancellable, description, parameters)
        {
            execute.ValidateNonNull(nameof(execute));
            this.canExecute = canExecute;
            this.execute = execute;
        }

        #endregion

        protected override bool DoCanExecute(object[] parameters)
        {
            return
                canExecute == null ||
                canExecute(parameters);
        }

        protected override Task DoExecute(
            CancellationToken? cancellation,
            object[] parameters
            )
        {
            return execute(cancellation, parameters);
        }

        private Func<object[], bool> canExecute;
        private Func<CancellationToken?, object[], Task> execute;
    }


    public class DelegateCommand<T> :
        DelegateCommand
    {
        #region Init

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Action<T> execute,
            Func<T, bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            base(
                @object, id, name,
                p => execute(Parameter(p)),
                CanExecute(canExecute),
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<T, Task> execute,
            Func<T, bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            base(
                @object, id, name,
                p => execute(Parameter(p)),
                CanExecute(canExecute),
                description, parameters
                )
        {
        }

        public DelegateCommand(
            IObject @object,
            string id,
            string name,
            Func<CancellationToken, T, Task> execute,
            Func<T, bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            base(
                @object, id, name,
                (c, p) => execute(c, Parameter(p)),
                CanExecute(canExecute),
                description, parameters
                )
        {
        }

        private static T Parameter(object[] p)
        {
            return (T)p[0];
        }
        private static Func<object[], bool> CanExecute(Func<T, bool> canExecute)
        {
            return canExecute == null ?
                (Func<object[], bool>)null :
                p => canExecute((T)p[0]);
        }

        #endregion
    }
}
