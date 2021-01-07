namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class GiftSubscriberNotifierSettings : TwitchNotifierSettings {
	/// <inheritdoc />
	public GiftSubscriberNotifierSettings() : base(@"Congratulations! You Have Received A New Subscription !",
												   @"@username gave a subscription to @username. ", 2) { }
}
}