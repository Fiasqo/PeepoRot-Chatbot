using System;
using System.Diagnostics;
using System.Windows;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.ShittyBot;

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
				return;
			}

			try {
				Bot.Instance.Settings = Settings;
				Bot.Instance.Connect();
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Settings = new Settings();
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
		if (!ReferenceEquals(Properties.Settings.Default.SettingsVM_Settings, null)) {
			Settings = Properties.Settings.Default.SettingsVM_Settings;
			try {
				Bot.Instance.Settings = Settings;
				Bot.Instance.Connect();
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				Settings = new Settings();
			}
		} else {
			Settings = new Settings();
		}
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