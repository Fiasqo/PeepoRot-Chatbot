using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class NewSubscriberNotifierSettings : TwitchNotifierSettings {
	/// <inheritdoc />
	public NewSubscriberNotifierSettings() : base(@"Thank You For The Subscription !", @"@username. ") { }
}
}