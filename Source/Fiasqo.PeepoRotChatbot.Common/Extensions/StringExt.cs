using System.Text.RegularExpressions;

namespace Fiasqo.PeepoRotChatbot.Common.Extensions {
public static class StringExt {
	public static bool ContainsOnlyLatinDigitsUnderscore(this string @this) => Regex.IsMatch(@this, @"^[a-zA-Z0-9_\s]+$");
}
}