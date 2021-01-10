using System;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class CapsProtection : ModProtection {
#region Fields

	private int _minCapsAmount = 10;

#endregion

#region Constructor

	public CapsProtection() : base("Please refrain from using caps !") { }

#endregion

#region Properties

	public int MinCapsAmount { get => _minCapsAmount; set => TrySetField(ref _minCapsAmount, Math.Abs(value), (_, value) => value > 0); }

#endregion
}
}