using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class Settings : PropertyChangedNotifier, IDataErrorInfo, IEquatable<Settings> {
#region Co

	public Settings() {
		_channelName = string.Empty;
		_accessToken = string.Empty;
		_refreshToken = string.Empty;
		_clientID = string.Empty;
	}

#endregion

#region Fields

	private string _channelName;
	private string _accessToken;
	private string _refreshToken;
	private string _clientID;

#endregion

#region Properties

	public string ChannelName { get => _channelName; set => SetField(ref _channelName, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string AccessToken { get => _accessToken; set => SetField(ref _accessToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string RefreshToken { get => _refreshToken; set => SetField(ref _refreshToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string ClienID { get => _clientID; set => SetField(ref _clientID, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

#endregion

#region IDataErrorInfo

	private bool ChannelNameHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (ChannelName == string.Empty) {
			sb.AppendLine($"Please Fill Out {nameof(ChannelName)} Field");
		} else {
			if (!ChannelName.ContainsOnlyLatinDigitsUnderscore()) sb.AppendLine($"{nameof(ChannelName)} Can Only Contain Latin Letters, Digits Or Underscore.");
			if (ChannelName.Length < Constants.MinUserNameLenght) sb.AppendLine($"{nameof(ChannelName)} Is Too Short. Min: {Constants.MinUserNameLenght}.");
			else if (ChannelName.Length > Constants.MaxUserNameLenght) sb.AppendLine($"{nameof(ChannelName)} Is Too Long. Max: {Constants.MaxUserNameLenght}.");
		}

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool AccessTokenHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (AccessToken == string.Empty) sb.AppendLine($"Please Fill Out {nameof(AccessToken)} Field");
		else if (!AccessToken.ContainsOnlyLatinDigitsUnderscore())
			sb.AppendLine($"{nameof(AccessToken)} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool RefreshTokenHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (RefreshToken == string.Empty) sb.AppendLine($"Please Fill Out {nameof(RefreshToken)} Field");
		else if (!RefreshToken.ContainsOnlyLatinDigitsUnderscore())
			sb.AppendLine($"{nameof(RefreshToken)} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool ClienIDHasError(out string errorMsg) {
		var sb = new StringBuilder(256);

		if (ClienID == string.Empty) sb.AppendLine($"Please Fill Out {nameof(ClienID)} Field");
		else if (!ClienID.ContainsOnlyLatinDigitsUnderscore()) sb.AppendLine($"{nameof(ClienID)} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	/// <inheritdoc />
	public string this[string columnName] => columnName switch {
		nameof(ChannelName)  => ChannelNameHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(AccessToken)  => AccessTokenHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(RefreshToken) => RefreshTokenHasError(out var errorMsg) ? errorMsg : string.Empty,
		nameof(ClienID)      => ClienIDHasError(out var errorMsg) ? errorMsg : string.Empty,
		_                    => string.Empty
	};

	/// <inheritdoc />
	public string Error {
		get {
			var sb = new StringBuilder(300);

			var error = this[nameof(ChannelName)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(AccessToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(RefreshToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(ClienID)];
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
	public override bool Equals(object obj) => Equals(obj as Settings);

	/// <inheritdoc />
	public bool Equals(Settings other) {
		if (ReferenceEquals(other, null)) return false;
		if (ReferenceEquals(other, this)) return true;
		return ChannelName.Equals(other.ChannelName) &&
			   AccessToken.Equals(other.AccessToken) &&
			   RefreshToken.Equals(other.RefreshToken) &&
			   ClienID.Equals(other.ClienID);
	}

#endregion
}
}