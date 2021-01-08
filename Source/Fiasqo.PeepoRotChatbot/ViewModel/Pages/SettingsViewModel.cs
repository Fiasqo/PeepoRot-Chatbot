using System;
using System.Diagnostics;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class SettingsViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public SettingsViewModel(bool IsLazyLoading = true) {
		if (IsLazyLoading) LoadDefault();
		else LoadDataOrDefault();
	}

#endregion

#region Fields

#endregion

#region Properties

	public Settings Settings { get; private set; }

#endregion

#region Commands

	public Command ApplySettingsCmd { get; } = new(x => { });
	public Command OnHyperLinkPressedCmd { get; } = new Command(_ => {
		Process.Start(new ProcessStartInfo("https://twitchtokengenerator.com/") {
			UseShellExecute = true,
		});
	});

#endregion

#region IPageViewModel

	public void LoadDefault() => Settings = new Settings();

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