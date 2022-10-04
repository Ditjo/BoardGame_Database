using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Brætspils_Database.Command
{
    //Delegate Command Class of ICommand
    //Used in Button Bindings
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute, Func<object?,bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            this._execute = execute;
            this._canExecute = canExecute;
        }

        //Calls see if it can execute now using the CanExecute Method
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? CanExecuteChanged;

        //Is called to see if the Command can be executed
        public bool CanExecute(object? parameter)
        {
            return _canExecute is null ? true : _canExecute(parameter);
        }

        //The Execute Method of the given command
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
