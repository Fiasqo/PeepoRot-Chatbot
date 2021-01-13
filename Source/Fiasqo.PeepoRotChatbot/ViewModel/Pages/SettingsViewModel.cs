using System;
using System.Diagnostics;
using System.Windows;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class SettingsViewModel : PropertyChangedNotifier, IPageViewModel {
#region Fields

	private Settings _settings;

#endregion

#region Constructor

	public SettingsViewModel() {
		ApplySettingsCmd = new Command(_ => {
			if (ReferenceEquals(Settings, null)) throw new NullReferenceException(nameof(Settings));
			if (Settings.Error != string.Empty) {
				MessageBox.Show("Settings Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				// ReSharper disable once RedundantJumpStatement
				return;
			}

			//TODO: Update bot
		});

		PropertyChanged += (_, e) => {
			if (e.PropertyName == nameof(Settings)) Settings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
		};

		LoadDataOrDefault();
	}

#endregion

#region Properties

	public Settings Settings { get => _settings; private set => SetField(ref _settings, value); }

#endregion

#region Commands

	public Command ApplySettingsCmd { get; }

	public Command OnHyperLinkPressedCmd { get; } = new(_ => {
		Process.Start(new ProcessStartInfo("https://twitchtokengenerator.com/") {
			UseShellExecute = true
		});
	});

#endregion

#region IPageViewModel

	/// <inheritdoc />
	public bool CanSwitchPage => Settings.Error == string.Empty;

	/// <inheritdoc />
	public bool IsLoaded { get; private set; }

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		Logger.Log.Info("Loading Data");
		Settings = ReferenceEquals(Properties.Settings.Default.SettingsVM_Settings, null) ? new Settings() : Properties.Settings.Default.SettingsVM_Settings;
		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() {
		Logger.Log.Info("Saving Data");
		Properties.Settings.Default.SettingsVM_Settings = Settings;
		Properties.Settings.Default.Save();
	}

#endregion
}
}