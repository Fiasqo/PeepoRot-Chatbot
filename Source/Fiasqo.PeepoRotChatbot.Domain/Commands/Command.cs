using System;
using System.Windows.Input;

namespace Fiasqo.PeepoRotChatbot.Domain.Commands {
/// <summary>
/// </summary>
public class Command : ICommand {
#region Constructors

	public Command(Action<object> execute, Func<object, bool> canExecute = null) {
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute ?? (_ => true);
	}

#endregion

#region Public Methods

	/// <summary>Validate all commands</summary>
	public static void Refresh() => CommandManager.InvalidateRequerySuggested();

#endregion

#region Fields

	private readonly Func<object, bool> _canExecute;
	private readonly Action<object> _execute;

#endregion

#region ICommand

	/// <inheritdoc />
	public bool CanExecute(object parameter) => _canExecute(parameter);

	/// <inheritdoc />
	public void Execute(object parameter) => _execute(parameter);

	/// <inheritdoc />
	public event EventHandler CanExecuteChanged { add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value; }

#endregion
}
}