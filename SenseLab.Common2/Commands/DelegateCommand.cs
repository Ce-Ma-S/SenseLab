using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
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
            Object @object,
            string id,
            string name,
            System.Action<object[]> execute,
            System.Func<object[], bool> canExecute = null,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            this(
                @object, id, name, false,
#pragma warning disable 1998
                async (c, p) => execute(p),
                canExecute,
                description, parameters
                )
        {
        }

        public DelegateCommand(
            Object @object,
            string id,
            string name,
            System.Func<object[], Task> execute,
            System.Func<object[], bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Func<CancellationToken, object[], Task> execute,
            System.Func<object[], bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Action execute,
            System.Func<bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Func<Task> execute,
            System.Func<bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Func<CancellationToken, Task> execute,
            System.Func<bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            bool isCancellable,
            System.Func<CancellationToken?, object[], Task> execute,
            System.Func<object[], bool> canExecute = null,
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

        private System.Func<object[], bool> canExecute;
        private System.Func<CancellationToken?, object[], Task> execute;
    }


    public class DelegateCommand<T> :
        DelegateCommand
    {
        #region Init

        public DelegateCommand(
            Object @object,
            string id,
            string name,
            System.Action<T> execute,
            System.Func<T, bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Func<T, Task> execute,
            System.Func<T, bool> canExecute = null,
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
            Object @object,
            string id,
            string name,
            System.Func<CancellationToken, T, Task> execute,
            System.Func<T, bool> canExecute = null,
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
        private static System.Func<object[], bool> CanExecute(System.Func<T, bool> canExecute)
        {
            return canExecute == null ?
                (System.Func<object[], bool>)null :
                p => canExecute((T)p[0]);
        }

        #endregion
    }
}
