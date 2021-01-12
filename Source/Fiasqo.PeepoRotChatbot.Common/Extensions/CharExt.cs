namespace Fiasqo.PeepoRotChatbot.Common.Extensions {
public static class CharExt {
	public static bool IsLatinLetter(this char c) => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
}
}