using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P7MExtractor.Others
{
    public delegate void ToExecuteWithParam(object param);
    public delegate bool CanExecuteDeterminatingMethodWithParam(object param);
    public delegate bool CanExecuteDeterminatingMethod();
    public class GenericCommand : ICommand
    {

        #region ctor
        public GenericCommand(Action toExecute)
        {
            _toExecute = toExecute;
        }
        public GenericCommand(Action toExecute, bool canExecute)
        {
            _toExecute = toExecute;
            this.canExecute = canExecute;
        }
        public GenericCommand(Action toExecute, CanExecuteDeterminatingMethod canExecuteMethod)
        {
            _toExecute = toExecute;
            determinatingMethod = canExecuteMethod;
        }

        public GenericCommand(ToExecuteWithParam toExecuteWithParam)
        {
            _toExecuteWithParam = toExecuteWithParam;
        }
        public GenericCommand(ToExecuteWithParam toExecuteWithParam, bool canExecute)
        {
            _toExecuteWithParam = toExecuteWithParam;
            this.canExecute = canExecute;
        }
        public GenericCommand(ToExecuteWithParam toExecuteWithParam, CanExecuteDeterminatingMethod canExecuteMethod)
        {
            _toExecuteWithParam = toExecuteWithParam;
            determinatingMethod = canExecuteMethod;
        }
        public GenericCommand(ToExecuteWithParam toExecuteWithParam, CanExecuteDeterminatingMethodWithParam canExecuteMethod)
        {
            _toExecuteWithParam = toExecuteWithParam;
            determinatingMethodWithParam = canExecuteMethod;
        }
        #endregion

        private ToExecuteWithParam _toExecuteWithParam;
        private Action _toExecute;
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

        public void Execute(object parameter)
        {
            _toExecute?.Invoke();
            _toExecuteWithParam?.Invoke(parameter);
        }
    }
}
