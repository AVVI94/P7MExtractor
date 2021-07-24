using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P7MExtractor.Others
{

    class GenericCommandAsync : ICommand
    {
        #region ctor
        public GenericCommandAsync(Func<Task> toExecute)
        {
            _toExecute = toExecute;
        }
        public GenericCommandAsync(Func<Task> toExecute, bool canExecute)
        {
            _toExecute = toExecute;
            this.canExecute = canExecute;
        }
        public GenericCommandAsync(Func<Task> toExecute, CanExecuteDeterminatingMethod canExecuteMethod)
        {
            _toExecute = toExecute;
            determinatingMethod = canExecuteMethod;
        }

        public GenericCommandAsync(Func<object, Task> toExecuteWithParam)
        {
            _toExecuteWithParam = toExecuteWithParam;
        }
        public GenericCommandAsync(Func<object, Task> toExecuteWithParam, bool canExecute)
        {
            _toExecuteWithParam = toExecuteWithParam;
            this.canExecute = canExecute;
        }
        public GenericCommandAsync(Func<object, Task> toExecuteWithParam, CanExecuteDeterminatingMethod canExecuteMethod)
        {
            _toExecuteWithParam = toExecuteWithParam;
            determinatingMethod = canExecuteMethod;
        }
        public GenericCommandAsync(Func<object, Task> toExecuteWithParam, CanExecuteDeterminatingMethodWithParam canExecuteMethod)
        {
            _toExecuteWithParam = toExecuteWithParam;
            determinatingMethodWithParam = canExecuteMethod;
        }
        #endregion

        private Func<object, Task> _toExecuteWithParam;
        private Func<Task> _toExecute;
        private CanExecuteDeterminatingMethod determinatingMethod;
        private CanExecuteDeterminatingMethodWithParam determinatingMethodWithParam;
        bool canExecute = true;
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (determinatingMethodWithParam != null)
                return determinatingMethodWithParam?.Invoke(parameter) ?? canExecute;

            return determinatingMethod?.Invoke() ?? canExecute;
        }

        public async void Execute(object parameter)
        {
            if (_toExecute != null)
                await _toExecute.Invoke().ConfigureAwait(false);

            if (_toExecuteWithParam != null)
                await _toExecuteWithParam.Invoke(parameter).ConfigureAwait(false);
        }
    }
}
