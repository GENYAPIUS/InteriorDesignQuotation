using System;
using System.Windows.Input;

namespace WPFLibrary;

public class RelayCommand : ICommand
{
    private readonly Func<object, bool> _canExecuteHandler;
    private readonly Action<object> _executeHandler;

    public RelayCommand(Action<object> executeHandler, Func<object, bool> canExecuteHandler)
    {
        _executeHandler = executeHandler ?? throw new ArgumentNullException(nameof(executeHandler));
        _canExecuteHandler = canExecuteHandler ?? throw new ArgumentNullException(nameof(canExecuteHandler));
    }

    public RelayCommand(Action<object> executeHandler) : this(executeHandler, x => true) { }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecuteHandler(parameter);
    }

    public void Execute(object? parameter)
    {
        _executeHandler(parameter);
    }

    public void RaiseCommand()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}