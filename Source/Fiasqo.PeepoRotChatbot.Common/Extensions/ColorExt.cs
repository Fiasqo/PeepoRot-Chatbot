using System.Windows.Media;

namespace Fiasqo.PeepoRotChatbot.Common.Extensions {
public static class ColorExt {
	public static Color Invert(this Color @this) => Color.FromRgb((byte) ~@this.R, (byte) ~@this.G, (byte) ~@this.B);
}
}