using System;
using System.Windows.Input;

namespace WpfHelloWorld.MvvmHelp
{
    /// <summary> Why does WPF make so many things so damn hard? </summary>
    public class DelegateCommand<T> : ICommand where T : class
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T?> _execute;

        public event EventHandler? CanExecuteChanged;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public DelegateCommand(Action<T?> execute) : this(execute, null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
        }

        public DelegateCommand(Action<T?> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;
#pragma warning disable CS8604 // Possible null reference argument.
            return _canExecute.Invoke(parameter as T);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void Execute(object? parameter)
        {
            _execute(parameter as T);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
