using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class ChatCommand : PropertyChangedNotifier, IDataErrorInfo, IEquatable<ChatCommand> {
#region Constructor

	public ChatCommand(string chatCommand = "NEW_COMMAND",
					   string reply = "NEW_RESPONSE",
					   Permissions permission = Permissions.Anyone,
					   bool isActivated = true) {
		Command = chatCommand;
		Reply = reply;
		Permission = permission;
		IsActivated = isActivated;
	}

#endregion

#region Private Methods

	private int GetMaxReplyLenght() => Constants.MaxChatMessageLenght - Constants.MaxUserNameLenght;

#endregion

#region Fields

	private string _command;
	private string _reply;
	private Permissions _permission;
	private bool _isActivated;

#endregion

#region Properties

	public string Command { get => _command; set => SetField(ref _command, string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim()); }
	public string Reply { get => _reply; set => SetField(ref _reply, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public Permissions Permission { get => _permission; set => SetField(ref _permission, value); }
	public bool IsActivated { get => _isActivated; set => SetField(ref _isActivated, value); }

#endregion

#region IDataErrorInfo

	private bool CommandHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (Command == string.Empty) {
			sb.AppendLine($"Please Fill Out {nameof(Command)} Field");
		} else {
			if (Command.Length > Constants.MaxChatCommandLenght) sb.AppendLine($"{nameof(Command)} Is Too Long. Max: {Constants.MaxChatCommandLenght}.");
			if (Command.Any(char.IsWhiteSpace)) sb.AppendLine($"{nameof(Command)} Can't Contain Spaces Or Whitespaces.");
			if (!Command[0].IsLatinLetter()) sb.AppendLine($"{nameof(Command)} Must Start With A Latin Letter.");
			if (!Command.Substring(1).ContainsOnlyLatinDigitsUnderscore())
				sb.AppendLine($"{nameof(Command)} Can Only Contain Latin Letters, Digits Or Underscore.");
		}

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool ReplyHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (Reply == string.Empty) sb.AppendLine($"Please Fill Out {nameof(Reply)} Field");
		else if (Reply.Length > GetMaxReplyLenght()) sb.AppendLine($"{nameof(Reply)} Is Too Long. Max: {GetMaxReplyLenght()}.");

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
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object obj) => Equals(obj as ChatCommand);

	/// <inheritdoc />
	public bool Equals(ChatCommand other) => !ReferenceEquals(other, null) &&
											 Command.Equals(other.Command) &&
											 Reply.Equals(other.Reply) &&
											 Permission.Equals(other.Permission) &&
											 IsActivated.Equals(other.IsActivated);

#endregion
}
}