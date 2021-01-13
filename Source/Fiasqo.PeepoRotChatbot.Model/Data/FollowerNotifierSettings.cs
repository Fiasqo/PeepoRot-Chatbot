using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class FollowerNotifierSettings : TwitchNotifierSettings {
	public FollowerNotifierSettings() : base(@"Thank You For The Follow !", @"@username. ", 2) { }
}
}