using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class LinkProtection : ModToolsProtection {
#region Constructor

	public LinkProtection() : base("Links are not allowed for you !") { }

#endregion
}
}