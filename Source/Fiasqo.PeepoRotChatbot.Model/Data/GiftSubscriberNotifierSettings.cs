using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class GiftSubscriberNotifierSettings : TwitchNotifierSettings {
	/// <inheritdoc />
	public GiftSubscriberNotifierSettings() : base(@"Congratulations! You Have Received A New Subscription !",
												   @"@username gave a subscription to @username. ", 1, 2) { }
}
}