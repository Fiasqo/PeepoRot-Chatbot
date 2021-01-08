using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class Settings : PropertyChangedNotifier {
#region Fields

	private string _channelName;
	private string _accessToken;
	private string _refreshToken;
	private string _clientID;

#endregion

#region Properties

	public string ChannelName {
		get => _channelName;
		set => TrySetField(ref _channelName, value, (_, value) => value.ContainsOnlyLatinDigitsUnderscore() && value.Length <= Constants.MaxUserNameLenght);
	}

	public string AccessToken {
		get => _accessToken;
		set => TrySetField(ref _accessToken, value, (_, value) => value.ContainsOnlyLatinDigitsUnderscore());
	}

	public string RefreshToken {
		get => _refreshToken;
		set => TrySetField(ref _refreshToken, value, (_, value) => value.ContainsOnlyLatinDigitsUnderscore());
	}

	public string ClienID {
		get => new string('#', _clientID.Length);
		set => TrySetField(ref _clientID, value, (_, value) => value.ContainsOnlyLatinDigitsUnderscore());
	}

#endregion
}
}