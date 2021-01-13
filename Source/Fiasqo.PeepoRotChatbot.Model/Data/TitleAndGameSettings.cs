using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class TitleAndGameSettings : PropertyChangedNotifier, IDataErrorInfo, IEquatable<TitleAndGameSettings> {
#region Fields

	private string _title = @"Today is another stream day";
	private string _gameName = @"Just Chatting";

#endregion

#region Properties

	public string Title { get => _title; set => SetField(ref _title, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string GameName { get => _gameName; set => SetField(ref _gameName, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

#endregion

#region IDataErrorInfo

	private bool TitleHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (Title == string.Empty) sb.AppendLine($"Please Fill Out {nameof(Title)} Field.");
		else if (Title.Length > Constants.MaxStreamTitleLenght) sb.AppendLine($"{nameof(Title)} Is Too Long. Max: {Constants.MaxStreamTitleLenght}.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	/// <inheritdoc />
	public string this[string columnName] => columnName switch {
		nameof(Title)    => TitleHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(GameName) => GameName == string.Empty ? $"Please Fill Out {nameof(GameName)} Field." : string.Empty,
		_                => string.Empty
	};

	/// <inheritdoc />
	public string Error {
		get {
			var sb = new StringBuilder(92);

			var error = this[nameof(Title)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(GameName)];
			if (error != string.Empty) sb.AppendLine(error);

			var sbBuilded = sb.ToString();

			return !string.IsNullOrWhiteSpace(sbBuilded)
					   ? sbBuilded.TrimEnd('\n', '\r')
					   : string.Empty;
		}
	}

#endregion

#region IEquatable

	/// <inheritdoc />
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object obj) => Equals(obj as TitleAndGameSettings);

	/// <inheritdoc />
	public bool Equals(TitleAndGameSettings other) => !ReferenceEquals(other, null) &&
													  Title.Equals(other.Title) &&
													  GameName.Equals(other.GameName);

#endregion
}
}