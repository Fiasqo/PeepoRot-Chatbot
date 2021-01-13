using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class ReSubscriberNotifierSettings : TwitchNotifierSettings {
	/// <inheritdoc />
	public ReSubscriberNotifierSettings() : base(@"Thank You For Re-subscription !", @"@username (Tier xN). ", 1, 2) { }
}
}