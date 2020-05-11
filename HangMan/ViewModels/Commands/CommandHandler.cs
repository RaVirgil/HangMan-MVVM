using System;
using System.Windows.Input;

namespace HangMan.ViewModels.Commands
{
    public class CommandHandler : ICommand
    {
        private Action<object> _actionParam;
        private Action _action;
        private Func<bool> _canExecute;

        /// <summary>
        /// Creates instance of the command handler
        /// </summary>
        /// <param name="action">Action to be executed by the command</param>
        /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
        public CommandHandler(Action<object> actionParam, Func<bool> canExecute)
        {
            _actionParam = actionParam;
            _canExecute = canExecute;
        }

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Wires CanExecuteChanged event 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forcess checking if execute is allowed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            if (_actionParam != null)
            {
                _actionParam(parameter);
                return;
            }

            if (_action != null)
            {
                _action();
                return;
            }
        }
    }




}
