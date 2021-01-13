using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public abstract class ModToolsProtection : PropertyChangedNotifier, IDataErrorInfo, IEquatable<ModToolsProtection> {
#region Consturctor

	protected ModToolsProtection(string reply = "", Permissions exempt = default, Punishments punishment = default, int punishmentInSeconds = 60) {
		if (punishmentInSeconds <= 0)
			throw new NotSupportedException($"{nameof(punishmentInSeconds)} Must Be Greater Than Zero;");
		if (reply.Length > GetMaxReplyLenght())
			throw new NotSupportedException($"{nameof(reply)} Is Too Long. value: {reply.Length};");
		Reply = reply;
		Exempt = exempt;
		Punishment = punishment;
		PunishmentInSeconds = punishmentInSeconds;
	}

#endregion

#region Private Methods

	private int GetMaxReplyLenght() => Constants.MaxChatMessageLenght - Constants.MaxUserNameLenght;

#endregion

#region Fields

	private Permissions _exempt;
	private Punishments _punishment;
	private int _punishmentInSeconds;
	private string _reply;

#endregion

#region Properties

	public Permissions Exempt { get => _exempt; set => SetField(ref _exempt, value); }
	public Punishments Punishment { get => _punishment; set => SetField(ref _punishment, value); }
	public int PunishmentInSeconds { get => _punishmentInSeconds; set => SetField(ref _punishmentInSeconds, value); }
	public string Reply { get => _reply; set => SetField(ref _reply, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

#endregion

#region IDataErrorInfo

	/// <inheritdoc />
	public virtual string this[string columnName] => columnName switch {
		nameof(PunishmentInSeconds) => PunishmentInSeconds <= 0 ? "The Punishment In Seconds Must Be Greater Than Zero." : string.Empty,
		nameof(Reply) => Reply.Length > GetMaxReplyLenght()
							 ? $"{nameof(Reply)} Is Too Long. Max: {GetMaxReplyLenght()}."
							 : string.Empty,
		_ => string.Empty
	};

	/// <inheritdoc />
	public virtual string Error {
		get {
			var sb = new StringBuilder(128);

			var error = this[nameof(PunishmentInSeconds)];
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
	public override bool Equals(object obj) => Equals(obj as ModToolsProtection);

	/// <inheritdoc />
	public virtual bool Equals(ModToolsProtection other) {
		if (ReferenceEquals(other, null)) return false;
		if (ReferenceEquals(other, this)) return true;
		return PunishmentInSeconds.Equals(other.PunishmentInSeconds) &&
			   Reply.Equals(other.Reply) &&
			   Punishment.Equals(other.Punishment) &&
			   Exempt.Equals(other.Exempt);
	}

#endregion
}
}