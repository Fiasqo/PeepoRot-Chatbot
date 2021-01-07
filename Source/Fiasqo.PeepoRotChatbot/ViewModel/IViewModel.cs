using System.ComponentModel;

namespace Fiasqo.PeepoRotChatbot.ViewModel {
public interface IViewModel : INotifyPropertyChanged {
	bool IsLoaded { get; }

	void LoadDataOrDefault();
	void SaveData();
}
}