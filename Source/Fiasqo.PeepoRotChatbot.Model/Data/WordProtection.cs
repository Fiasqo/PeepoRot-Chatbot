using System.Linq;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class WordProtection : ModProtection {
#region Fields

	private string _badWords = string.Empty;

#endregion

#region Constructor

	public WordProtection() : base("That language is not acceptable you little c_nt !") { }

#endregion

#region Properties

	public string BadWords {
		get => _badWords;
		set {
			var newValue = string.Empty;
			if (!string.IsNullOrWhiteSpace(value))
				newValue = string.Join(",", value.Split(',').Where(str => !string.IsNullOrWhiteSpace(str)).Select(x => x.Trim()));
			SetField(ref _badWords, newValue);
		}
	}

#endregion
}
}