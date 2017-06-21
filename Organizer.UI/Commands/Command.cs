using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Organizer.UI.Commands
{
    public class Command : RoutedUICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private bool _isExecuting;
        private bool _isAsyncCommand;
        private CommandBinding _commandBinding;

        private Command(string text, string name, Type ownerType, Func<bool> canExecute, Action execute) : base(text, name, ownerType)
        {
            _canExecute = canExecute;
            _execute = execute;
            _isExecuting = false;
        }

        public static Command CreateAsyncCommand(string text, string name, Type ownerType, Func<Task> execute, Func<bool> canExecute = null)
        {
            var command =
                new Command(text, name, ownerType, canExecute, async () => await execute()) {_isAsyncCommand = true};
            return command;
        }

        public static Command CreateCommand(string text, string name, Type ownerType, Action execute, Func<bool> canExecute = null)
        {
            return new Command(text, name, ownerType, canExecute, execute);
        }

        public void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!_isAsyncCommand)
                e.CanExecute = _canExecute == null || _canExecute();
            else
            {
                e.CanExecute = !_isExecuting && (_canExecute == null || _canExecute());
            }
            e.Handled = true;
        }

        public void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _isExecuting = true;
            _execute?.Invoke();
            _isExecuting = false;
        }

        public static void RegisterCommandBinding(Window window, Command command)
        {
            command._commandBinding = new CommandBinding(command, command.Executed, command.CanExecute);
            window.CommandBindings.Add(command._commandBinding);
        }

        public static void UnregisterCommandBinding(Window window, Command command)
        {
            if (command._commandBinding != null)
            {
                window.CommandBindings.Remove(command._commandBinding);
            }
        }
    }
}