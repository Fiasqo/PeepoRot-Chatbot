using System;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public abstract class TwitchNotifierSettings : PropertyChangedNotifier {
#region Constructor

	protected TwitchNotifierSettings(string defaultReply, string prefix, int numberOfUserNames = 1) {
		if (string.IsNullOrWhiteSpace(defaultReply)) throw new ArgumentNullException(nameof(defaultReply));
		if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentNullException(nameof(prefix));
		if (prefix.Length - "@username".Length * numberOfUserNames + Constants.MaxUserNameLenght * numberOfUserNames > Constants.MaxChatMessageLenght - 1)
			throw new NotSupportedException($"{nameof(Prefix)} too long. Should be <= {Constants.MaxChatMessageLenght - 1}");

		Prefix = prefix;
		_reply = defaultReply;
		MaxReplyLenght = Constants.MaxChatMessageLenght - Prefix.Length;
	}

#endregion

#region Fields

	private string _reply;
	private int _gapBetweenReplies_Seconds = Constants.MinGapBetweenEventsReplies_Seconds;

#endregion

#region Properties

	public string Prefix { get; }

	public string Reply {
		get => _reply;
		set => TrySetField(ref _reply, value, (_, value) => !string.IsNullOrWhiteSpace(value) && value.Length <= MaxReplyLenght);
	}

	public int GapBetweenReplies_Seconds {
		get => _gapBetweenReplies_Seconds;
		set => TrySetField(ref _gapBetweenReplies_Seconds,
						   value,
						   (_, value) => value >= Constants.MinGapBetweenEventsReplies_Seconds && value <= Constants.MaxGapBetweenEventsReplies_Seconds);
	}

	public int MaxReplyLenght { get; }

#endregion
}
}