using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Domain;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public class TitleAndGameSettings : PropertyChangedNotifier {
#region Fields

	private string _title = @"Today is another stream day";
	private readonly string _gameName = @"Just Chatting";

#endregion

#region Properties

	public string Title {
		get => _title;
		set => TrySetField(ref _title, value, (_, value) => !string.IsNullOrWhiteSpace(value) && value.Length <= Constants.MaxStreamTitleLenght);
	}

	public string GameName { get => _gameName; set => TrySetField(ref _title, value, (_, value) => !string.IsNullOrWhiteSpace(value)); }

#endregion
}
}