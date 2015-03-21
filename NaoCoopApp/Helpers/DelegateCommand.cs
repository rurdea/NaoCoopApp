using System;
using System.Windows.Input;

namespace NaoCoopApp.Helpers
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _noParameterCommand;
        private readonly Action<object> _command;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action command, Func<bool> canExecute = null)
        {
            if (command == null)
                throw new ArgumentNullException();
            _canExecute = canExecute;
            _noParameterCommand = command;
        }

        public DelegateCommand(Action<object> command, Func<bool> canExecute = null)
        {
            if (command == null)
                throw new ArgumentNullException();
            _canExecute = canExecute;
            _command = command;
        }

        public void Execute(object parameter)
        {
            if (_noParameterCommand != null)
            {
                _noParameterCommand();
            }
            else if (_command != null)
            {
                _command(parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

    }

    //public class DelegateCommand<T> : DelegateCommand where T : Object
    //{
    //    public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    //        : base((o) => Execute(o), (o) => CanExecute((T)o))
    //    {
    //    }
    //}
}