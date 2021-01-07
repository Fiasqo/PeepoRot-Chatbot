namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class ReSubscriberNotifierSettings : TwitchNotifierSettings {
	/// <inheritdoc />
	public ReSubscriberNotifierSettings() : base(@"Thank You For Re-subscription !", @"@username (Tier xN). ", 2) { }
}
}