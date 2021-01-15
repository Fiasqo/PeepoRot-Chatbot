using System;
using System.ComponentModel;
using System.Text;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
[Serializable]
public sealed class Settings : PropertyChangedNotifier, IDataErrorInfo, IEquatable<Settings> {
#region Fields

	private string _userChannelName = string.Empty;
	private string _userAccessToken = string.Empty;
	private string _userRefreshToken = string.Empty;
	private string _userClientID = string.Empty;
	private string _botChannelName = string.Empty;
	private string _botAccessToken = string.Empty;
	private string _botRefreshToken = string.Empty;
	private string _botClientID = string.Empty;

#endregion

#region Properties

	public string UserChannelName { get => _userChannelName; set => SetField(ref _userChannelName, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string UserAccessToken { get => _userAccessToken; set => SetField(ref _userAccessToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string UserRefreshToken { get => _userRefreshToken; set => SetField(ref _userRefreshToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string UserClienID { get => _userClientID; set => SetField(ref _userClientID, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

	public string BotChannelName { get => _botChannelName; set => SetField(ref _botChannelName, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string BotAccessToken { get => _botAccessToken; set => SetField(ref _botAccessToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string BotRefreshToken { get => _botRefreshToken; set => SetField(ref _botRefreshToken, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
	public string BotClienID { get => _botClientID; set => SetField(ref _botClientID, string.IsNullOrWhiteSpace(value) ? string.Empty : value); }

#endregion

#region IDataErrorInfo

	private bool ChannelNameHasError(out string errorMsg, string field, string fieldName) {
		var sb = new StringBuilder(192);

		if (field == string.Empty) {
			sb.AppendLine($"Please Fill Out {fieldName} Field");
		} else {
			if (!field.ContainsOnlyLatinDigitsUnderscore()) sb.AppendLine($"{fieldName} Can Only Contain Latin Letters, Digits Or Underscore.");
			if (field.Length < Constants.MinUserNameLenght) sb.AppendLine($"{fieldName} Is Too Short. Min: {Constants.MinUserNameLenght}.");
			else if (field.Length > Constants.MaxUserNameLenght) sb.AppendLine($"{fieldName} Is Too Long. Max: {Constants.MaxUserNameLenght}.");
		}

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool AccessTokenHasError(out string errorMsg, string field, string fieldName) {
		var sb = new StringBuilder(128);

		if (field == string.Empty) sb.AppendLine($"Please Fill Out {fieldName} Field");
		else if (!field.ContainsOnlyLatinDigitsUnderscore())
			sb.AppendLine($"{fieldName} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool RefreshTokenHasError(out string errorMsg, string field, string fieldName) {
		var sb = new StringBuilder(128);

		if (field == string.Empty) sb.AppendLine($"Please Fill Out {fieldName} Field");
		else if (!field.ContainsOnlyLatinDigitsUnderscore())
			sb.AppendLine($"{fieldName} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	private bool ClienIDHasError(out string errorMsg, string field, string fieldName) {
		var sb = new StringBuilder(128);

		if (field == string.Empty) sb.AppendLine($"Please Fill Out {fieldName} Field");
		else if (!field.ContainsOnlyLatinDigitsUnderscore()) sb.AppendLine($"{fieldName} Can Only Contain Latin Letters, Digits Or Underscore.");

		errorMsg = sb.ToString().TrimEnd('\n', '\r');
		return !string.IsNullOrEmpty(errorMsg);
	}

	/// <inheritdoc />
	public string this[string columnName] => columnName switch {
		nameof(UserChannelName)  => ChannelNameHasError(out var errorMsg, UserChannelName, nameof(UserChannelName)) ? errorMsg : string.Empty,
		nameof(UserAccessToken)  => AccessTokenHasError(out var errorMsg, UserAccessToken, nameof(UserAccessToken)) ? errorMsg : string.Empty,
		nameof(UserRefreshToken) => RefreshTokenHasError(out var errorMsg, UserRefreshToken, nameof(UserRefreshToken)) ? errorMsg : string.Empty,
		nameof(UserClienID)      => ClienIDHasError(out var errorMsg, UserClienID, nameof(UserClienID)) ? errorMsg : string.Empty,
		
		nameof(BotChannelName)  => ChannelNameHasError(out var errorMsg, BotChannelName, nameof(BotChannelName)) ? errorMsg : string.Empty,
		nameof(BotAccessToken)  => AccessTokenHasError(out var errorMsg, BotAccessToken, nameof(BotAccessToken)) ? errorMsg : string.Empty,
		nameof(BotRefreshToken) => RefreshTokenHasError(out var errorMsg, BotRefreshToken, nameof(BotRefreshToken)) ? errorMsg : string.Empty,
		nameof(BotClienID)      => ClienIDHasError(out var errorMsg, BotClienID, nameof(BotClienID)) ? errorMsg : string.Empty,
		_                       => string.Empty
	};

	/// <inheritdoc />
	public string Error {
		get {
			var sb = new StringBuilder(300);

			var error = this[nameof(UserChannelName)];
			if (error != string.Empty) sb.AppendLine(error);
			
			error = this[nameof(UserAccessToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(UserRefreshToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(UserClienID)];
			if (error != string.Empty) sb.AppendLine(error);
			
			error = this[nameof(BotChannelName)];
			if (error != string.Empty) sb.AppendLine(error);
			
			error = this[nameof(BotAccessToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(BotRefreshToken)];
			if (error != string.Empty) sb.AppendLine(error);

			error = this[nameof(BotClienID)];
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
		return UserChannelName.Equals(other.UserChannelName) &&
			   UserAccessToken.Equals(other.UserAccessToken) &&
			   UserRefreshToken.Equals(other.UserRefreshToken) &&
			   UserClienID.Equals(other.UserClienID) &&
			   
			   BotChannelName.Equals(other.BotChannelName) &&
			   BotAccessToken.Equals(other.BotAccessToken) &&
			   BotRefreshToken.Equals(other.BotRefreshToken) &&
			   BotClienID.Equals(other.BotClienID);
	}

#endregion
}
}