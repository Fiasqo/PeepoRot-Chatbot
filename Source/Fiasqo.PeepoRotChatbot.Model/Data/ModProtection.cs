using System;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public abstract class ModProtection : PropertyChangedNotifier {
#region Consturctor

	protected ModProtection(string defaultReply = "", Permissions exempt = default, Punishments punishment = default, int punishmentInSeconds = 60) {
		if (punishmentInSeconds <= 0)
			throw new NotSupportedException($"{nameof(punishmentInSeconds)} can't be <= 0");
		_reply = defaultReply;
		_exempt = exempt;
		_punishment = punishment;
		_punishmentInSeconds = punishmentInSeconds;
	}

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
	public int PunishmentInSeconds { get => _punishmentInSeconds; set => TrySetField(ref _punishmentInSeconds, Math.Abs(value), (_, value) => value > 0); }

	public string Reply {
		get => _reply;
		set {
			if (string.IsNullOrWhiteSpace(value)) value = string.Empty;
			SetField(ref _reply, value);
		}
	}

#endregion
}
}