using System;
using System.Linq;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class WordProtection : ModToolsProtection, IEquatable<WordProtection> {
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

#region IEquatable

	/// <inheritdoc />
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object obj) => Equals(obj as WordProtection);

	/// <inheritdoc />
	public bool Equals(WordProtection other) {
		if (ReferenceEquals(other, null)) return false;
		if (ReferenceEquals(other, this)) return true;
		return base.Equals(other) && BadWords.Equals(other.BadWords);
	}

#endregion
}
}