using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseWork
{

    public class RelayCommand(Action? execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action? _execute = execute;
        private readonly Func<bool>? _canExecute = canExecute;

        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute();

        public void Execute(object? parameter) => _execute?.Invoke();

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
