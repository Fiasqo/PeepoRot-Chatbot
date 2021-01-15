using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public abstract class TwitchNotifierSettings : PropertyChangedNotifier, IDataErrorInfo, IEquatable<TwitchNotifierSettings> {
#region Constructor

	protected TwitchNotifierSettings(string defaultReply, string prefix, int numberOfUserNames = 1) {
		if (string.IsNullOrWhiteSpace(defaultReply)) throw new ArgumentNullException(nameof(defaultReply));
		if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentNullException(nameof(prefix));
		if (!prefix.Contains("@username")) throw new NotSupportedException($"{nameof(Prefix)} Must Contain At Least One Substring '@username'.");
		if (prefix.Length - 9 * numberOfUserNames + Constants.MaxUserNameLenght * numberOfUserNames >= Constants.MaxChatMessageLenght)
			throw new NotSupportedException($"{nameof(Prefix)} Is Too Long. Should Be Less Than {Constants.MaxChatMessageLenght}");
		Prefix = prefix;
		Reply = defaultReply;
	}

#endregion

#region Private Methods

	private int GetMaxReplyLenght() => Constants.MaxChatMessageLenght - Prefix.Length;

#endregion

#region Fields

	private string _reply;

#endregion

#region Properties

	public string Prefix { get; }
	public string Reply { get => _reply; set => SetField(ref _reply, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

#endregion

#region IDataErrorInfo

	private bool ReplyHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (Reply == string.Empty) sb.AppendLine($"Please Fill Out {nameof(Reply)} Field.");
		else if (Reply.Length > GetMaxReplyLenght()) sb.AppendLine($"{nameof(Reply)} Is Too Long. Max: {GetMaxReplyLenght()}.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}
	
	/// <inheritdoc />
	public virtual string this[string columnName] => columnName switch {
		nameof(Reply)                      => ReplyHasError(out var errorMsg) ? errorMsg : string.Empty,
		_                                  => string.Empty
	};

	/// <inheritdoc />
	public virtual string Error {
		get {
			var sb = new StringBuilder(128);

			var error = this[nameof(Reply)];
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
	public override bool Equals(object obj) => Equals(obj as TwitchNotifierSettings);

	/// <inheritdoc />
	public virtual bool Equals(TwitchNotifierSettings other) {
		if (ReferenceEquals(other, null)) return false;
		if (ReferenceEquals(other, this)) return true;
		return Prefix.Equals(other.Prefix) &&
			   Reply.Equals(other.Reply);
	}

#endregion
}
}