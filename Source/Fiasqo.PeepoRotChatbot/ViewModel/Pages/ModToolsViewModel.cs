using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class ModToolsViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public ModToolsViewModel(bool IsLazyLoading = true) {
		if (IsLazyLoading) LoadDefault();
		else LoadDataOrDefault();
	}

#endregion

#region Fields

	private bool _capsProtectionIsEnabled;
	private bool _linkProtectionIsEnabled;
	private bool _wordProtectionIsEnabled;

#endregion

#region Properties

	public CapsProtection CapsProtection { get; private set; }
	public LinkProtection LinkProtection { get; private set; }
	public WordProtection WordProtection { get; private set; }

	public bool CapsProtectionIsEnabled { get => _capsProtectionIsEnabled; set => SetField(ref _capsProtectionIsEnabled, value); }
	public bool LinkProtectionIsEnabled { get => _linkProtectionIsEnabled; set => SetField(ref _linkProtectionIsEnabled, value); }
	public bool WordProtectionIsEnabled { get => _wordProtectionIsEnabled; set => SetField(ref _wordProtectionIsEnabled, value); }

#endregion

#region Commands

	public Command ApplyCapsProtectionCmd { get; } = new(x => { });
	public Command ApplyLinkProtectionCmd { get; } = new(x => { });
	public Command ApplyWordProtectionCmd { get; } = new(x => { });

	public Command TurnOnOffCapsProtectionCmd { get; } = new(x => { });
	public Command TurnOnOffLinkProtectionCmd { get; } = new(x => { });
	public Command TurnOnOffWordProtectionCmd { get; } = new(x => { });

#endregion

#region IPageViewModel

	public void LoadDefault() {
		CapsProtection = new CapsProtection();
		LinkProtection = new LinkProtection();
		WordProtection = new WordProtection();
	}


	/// <inheritdoc />
	public bool IsLoaded { get; private set; }

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		Logger.Log.Info("Loading Data");

		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() => Logger.Log.Info("Saving Data");

#endregion
}
}