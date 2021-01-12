using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class ChatCommand : PropertyChangedNotifier, IDataErrorInfo, IEquatable<ChatCommand> {
#region Constructor

	public ChatCommand(string chatCommand = "NEW_COMMAND",
					   string reply = "NEW_RESPONSE",
					   Permissions permission = Permissions.Anyone,
					   bool isActivated = true) {
		_command = chatCommand;
		_reply = reply;
		_permission = permission;
		_isActivated = isActivated;
	}

#endregion

#region Fields

	private string _command;
	private string _reply;
	private Permissions _permission;
	private bool _isActivated;

#endregion

#region Properties

	public string Command { get => _command; set => SetField(ref _command, value.Trim()); }
	public string Reply { get => _reply; set => SetField(ref _reply, value); }
	public Permissions Permission { get => _permission; set => SetField(ref _permission, value); }
	public bool IsActivated { get => _isActivated; set => SetField(ref _isActivated, value); }

#endregion

#region Pub Methods

	public bool GetHasError() => Error != string.Empty;

#endregion
	
#region IDataErrorInfo

	private bool CommandHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (string.IsNullOrWhiteSpace(Command)) {
			sb.AppendLine("Please Fill Out Command Field");
		} else {
			if (Command.Length > Constants.MaxChatCommandLenght) sb.AppendLine("Command is too long.");
			if (Command.Any(char.IsWhiteSpace)) sb.AppendLine("Command Can't Contain Spaces Or Whitespaces.");
			if (!Command[0].IsLatinLetter()) sb.AppendLine("Command Must Start With A Latin Letter.");
			if (!Command.Substring(1).ContainsOnlyLatinDigitsUnderscore())
				sb.AppendLine("Command Can Only Contain Latin Letters, Digits Or Underscore.");
		}

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool ReplyHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (string.IsNullOrWhiteSpace(Reply)) {
			sb.AppendLine("Please Fill Out Reply Field");
		} else {
			if (Reply.Length > Constants.MaxChatMessageLenght - Constants.MaxUserNameLenght) sb.AppendLine("Reply is too long.");
		}

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	/// <inheritdoc />
	public string this[string columnName] => columnName switch {
		nameof(Command) => CommandHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(Reply)   => ReplyHasError(out var errorMsg) ? errorMsg : string.Empty,
		_               => string.Empty
	};

	/// <inheritdoc />
	public string Error {
		get {
			var sb = new StringBuilder(300);

			var error = this[nameof(Command)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(Reply)];
			if (error != string.Empty) sb.AppendLine(error);

			var sbBuilded = sb.ToString();

			return !string.IsNullOrWhiteSpace(sbBuilded)
					   ? sbBuilded.TrimEnd('\n', '\r')
					   : string.Empty;
		}
	}

#endregion

#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object obj) => Equals(obj as ChatCommand);

	/// <inheritdoc />
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public bool Equals(ChatCommand other) => !ReferenceEquals(other, null) &&
											 Command.Equals(other.Command) &&
											 Reply.Equals(other.Reply) &&
											 Permission.Equals(other.Permission) &&
											 IsActivated.Equals(other.IsActivated);

#endregion
}
}