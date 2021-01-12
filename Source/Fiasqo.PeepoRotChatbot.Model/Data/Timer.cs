using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class Timer : PropertyChangedNotifier, IDataErrorInfo, IEquatable<Timer> {
#region Constructor

	public Timer(string response = "NEW_TIMER",
				 uint intervalInMinutes = 30,
				 bool isActivated = true) {
		_response = response;
		_intervalInMinutes = intervalInMinutes;
		_isActivated = isActivated;
	}

#endregion

#region Fields

	private string _response;
	private uint _intervalInMinutes;
	private bool _isActivated;

#endregion

#region Properties

	public string Response { get => _response; set => SetField(ref _response, value); }
	public uint IntervalInMinutes { get => _intervalInMinutes; set => SetField(ref _intervalInMinutes, value); }
	public bool IsActivated { get => _isActivated; set => SetField(ref _isActivated, value); }

#endregion

#region Pub Methods

	public bool GetHasError() => Error != string.Empty;

#endregion

#region IDataErrorInfo

	private bool ResponseHasError(out string errorMsg) {
		var sb = new StringBuilder(92);

		if (string.IsNullOrWhiteSpace(Response)) {
			sb.AppendLine($"Please Fill Out {nameof(Response)} Field.");
		} else {
			if (Response.Length > Constants.MaxChatMessageLenght) sb.AppendLine($"{nameof(Response)} Is Too Long.");
		}

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
	public override bool Equals(object obj) => Equals(obj as Timer);

	/// <inheritdoc />
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public bool Equals(Timer other) => !ReferenceEquals(other, null) &&
									   Response.Equals(other.Response) &&
									   IntervalInMinutes.Equals(other.IntervalInMinutes) &&
									   IsActivated.Equals(other.IsActivated);

#endregion
}
}