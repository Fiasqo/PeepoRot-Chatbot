using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class Timer : PropertyChangedNotifier, IDataErrorInfo, IEquatable<Timer> {
#region Constructor

	public Timer(string response = "NEW_TIMER",
				 uint intervalInMinutes = 30,
				 bool isActivated = true) {
		Response = response;
		IntervalInMinutes = intervalInMinutes;
		IsActivated = isActivated;
	}

#endregion

#region Fields

	private string _response;
	private uint _intervalInMinutes;
	private bool _isActivated;

#endregion

#region Properties

	public string Response { get => _response; set => SetField(ref _response, string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim()); }
	public uint IntervalInMinutes { get => _intervalInMinutes; set => SetField(ref _intervalInMinutes, value); }
	public bool IsActivated { get => _isActivated; set => SetField(ref _isActivated, value); }

#endregion

#region IDataErrorInfo

	private bool ResponseHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (Response == string.Empty) sb.AppendLine($"Please Fill Out {nameof(Response)} Field.");
		else if (Response.Length > Constants.MaxChatMessageLenght) sb.AppendLine($"{nameof(Response)} Is Too Long. Max: {Constants.MaxChatMessageLenght}.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	/// <inheritdoc />
	public string this[string columnName] => columnName switch {
		nameof(Response)          => ResponseHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(IntervalInMinutes) => IntervalInMinutes <= 5 ? "The Interval Must Be Greater Than Five" : string.Empty,
		_                         => string.Empty
	};

	/// <inheritdoc />
	public string Error {
		get {
			var sb = new StringBuilder(92);

			var error = this[nameof(Response)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(IntervalInMinutes)];
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
	public override bool Equals(object obj) => Equals(obj as Timer);

	/// <inheritdoc />
	public bool Equals(Timer other) => !ReferenceEquals(other, null) &&
									   Response.Equals(other.Response) &&
									   IntervalInMinutes.Equals(other.IntervalInMinutes) &&
									   IsActivated.Equals(other.IsActivated);

#endregion
}
}