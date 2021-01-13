using System;
using System.ComponentModel;
using System.Windows;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Settings = Fiasqo.PeepoRotChatbot.Properties.Settings;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public sealed class DashboardViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public DashboardViewModel(bool IsLazyLoading = true) {
		ApplyTitleAndGameSettingsCmd = new Command(_ => {
			if (ReferenceEquals(TitleAndGameSettings, null)) throw new NullReferenceException(nameof(TitleAndGameSettings));
			if (TitleAndGameSettings.Error != string.Empty) {
				MessageBox.Show("Title And Game Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				// ReSharper disable once RedundantJumpStatement
				return;
			}

			//TODO: Update bot
		});

		ApplyFollowerNotifierSettingsCmd = new Command(_ => {
			if (ReferenceEquals(FollowerNotifierSettings, null)) throw new NullReferenceException(nameof(FollowerNotifierSettings));
			if (FollowerNotifierSettings.Error != string.Empty) {
				MessageBox.Show("Follow Notifier Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				// ReSharper disable once RedundantJumpStatement
				return;
			}

			//TODO: Update bot	
		});

		ApplySubscriberNotifierSettingsCmd = new Command(_ => {
			if (ReferenceEquals(NewSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(NewSubscriberNotifierSettings));
			if (ReferenceEquals(ReSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(ReSubscriberNotifierSettings));
			if (ReferenceEquals(GiftSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(GiftSubscriberNotifierSettings));
			if (NewSubscriberNotifierSettings.Error != string.Empty ||
				ReSubscriberNotifierSettings.Error != string.Empty ||
				GiftSubscriberNotifierSettings.Error != string.Empty) {
				MessageBox.Show("Subscribe Notifier Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				// ReSharper disable once RedundantJumpStatement
				return;
			}

			//TODO: Update bot	
		});

		PropertyChanged += OnPropertyChanged;

		if (IsLazyLoading) LoadDefault();
		else LoadDataOrDefault();
	}

#endregion

#region Private Methods

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
		switch (e.PropertyName) {
			case nameof(FollowerNotifierIsEnabled):
				OnFollowerNotifierIsEnabledChanged();
				break;
			case nameof(SubscriberNotifierIsEnabled):
				OnSubscriberNotifierIsEnabledChanged();
				break;
			case nameof(TitleAndGameSettings):
				TitleAndGameSettings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(FollowerNotifierSettings):
				FollowerNotifierSettings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(NewSubscriberNotifierSettings):
				NewSubscriberNotifierSettings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(ReSubscriberNotifierSettings):
				ReSubscriberNotifierSettings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(GiftSubscriberNotifierSettings):
				GiftSubscriberNotifierSettings.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
		}
	}

	private void OnFollowerNotifierIsEnabledChanged() { }

	private void OnSubscriberNotifierIsEnabledChanged() { }

#endregion

#region Fields

	public TitleAndGameSettings _titleAndGameSettings;
	public FollowerNotifierSettings _followerNotifierSettings;
	public NewSubscriberNotifierSettings _newSubscriberNotifierSettings;
	public ReSubscriberNotifierSettings _reSubscriberNotifierSettings;
	public GiftSubscriberNotifierSettings _giftSubscriberNotifierSettings;

	private bool _followerNotifierIsEnabled;
	private bool _subscriberNotifierIsEnabled;

#endregion

#region Properties

	public TitleAndGameSettings TitleAndGameSettings { get => _titleAndGameSettings; private set => SetField(ref _titleAndGameSettings, value); }

	public FollowerNotifierSettings FollowerNotifierSettings {
		get => _followerNotifierSettings;
		private set => SetField(ref _followerNotifierSettings, value);
	}

	public NewSubscriberNotifierSettings NewSubscriberNotifierSettings {
		get => _newSubscriberNotifierSettings;
		private set => SetField(ref _newSubscriberNotifierSettings, value);
	}

	public ReSubscriberNotifierSettings ReSubscriberNotifierSettings {
		get => _reSubscriberNotifierSettings;
		private set => SetField(ref _reSubscriberNotifierSettings, value);
	}

	public GiftSubscriberNotifierSettings GiftSubscriberNotifierSettings {
		get => _giftSubscriberNotifierSettings;
		private set => SetField(ref _giftSubscriberNotifierSettings, value);
	}

	public int SubscriptionDelayInSeconds {
		get => NewSubscriberNotifierSettings.GapBetweenRepliesInSeconds;
		set {
			NewSubscriberNotifierSettings.GapBetweenRepliesInSeconds =
				ReSubscriberNotifierSettings.GapBetweenRepliesInSeconds =
					GiftSubscriberNotifierSettings.GapBetweenRepliesInSeconds = value;
			OnPropertyChanged();
		}
	}

	public bool FollowerNotifierIsEnabled { get => _followerNotifierIsEnabled; set => SetField(ref _followerNotifierIsEnabled, value); }
	public bool SubscriberNotifierIsEnabled { get => _subscriberNotifierIsEnabled; set => SetField(ref _subscriberNotifierIsEnabled, value); }

#endregion

#region Commands

	public Command ApplyTitleAndGameSettingsCmd { get; }
	public Command ApplyFollowerNotifierSettingsCmd { get; }
	public Command ApplySubscriberNotifierSettingsCmd { get; }

#endregion

#region IPageViewModel

	/// <inheritdoc />
	public bool CanSwitchPage => TitleAndGameSettings.Error == string.Empty &&
								 FollowerNotifierSettings.Error == string.Empty &&
								 NewSubscriberNotifierSettings.Error == string.Empty &&
								 ReSubscriberNotifierSettings.Error == string.Empty &&
								 GiftSubscriberNotifierSettings.Error == string.Empty;

	public void LoadDefault() {
		_followerNotifierIsEnabled = false;
		_subscriberNotifierIsEnabled = false;
		TitleAndGameSettings = new TitleAndGameSettings();
		FollowerNotifierSettings = new FollowerNotifierSettings();
		NewSubscriberNotifierSettings = new NewSubscriberNotifierSettings();
		ReSubscriberNotifierSettings = new ReSubscriberNotifierSettings();
		GiftSubscriberNotifierSettings = new GiftSubscriberNotifierSettings();
	}

	/// <inheritdoc />
	public bool IsLoaded { get; private set; }

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		Logger.Log.Info("Loading Data");

		FollowerNotifierIsEnabled = Settings.Default.DashboardVM_FollowerNotifierIsEnabled;
		SubscriberNotifierIsEnabled = Settings.Default.DashboardVM_SubscriberNotifierIsEnabled;

		TitleAndGameSettings = ReferenceEquals(Settings.Default.DashboardVM_TitleAndGame, null)
								   ? new TitleAndGameSettings()
								   : Settings.Default.DashboardVM_TitleAndGame;
		FollowerNotifierSettings = ReferenceEquals(Settings.Default.DashboardVM_FollowerNotifier, null)
									   ? new FollowerNotifierSettings()
									   : Settings.Default.DashboardVM_FollowerNotifier;
		NewSubscriberNotifierSettings = ReferenceEquals(Settings.Default.DashboardVM_NewSubscriberNotifier, null)
											? new NewSubscriberNotifierSettings()
											: Settings.Default.DashboardVM_NewSubscriberNotifier;
		ReSubscriberNotifierSettings = ReferenceEquals(Settings.Default.DashboardVM_ReSubscriberNotifier, null)
										   ? new ReSubscriberNotifierSettings()
										   : Settings.Default.DashboardVM_ReSubscriberNotifier;
		GiftSubscriberNotifierSettings = ReferenceEquals(Settings.Default.DashboardVM_GiftSubscriberNotifier, null)
											 ? new GiftSubscriberNotifierSettings()
											 : Settings.Default.DashboardVM_GiftSubscriberNotifier;

		SubscriptionDelayInSeconds = (NewSubscriberNotifierSettings.GapBetweenRepliesInSeconds +
									  ReSubscriberNotifierSettings.GapBetweenRepliesInSeconds +
									  GiftSubscriberNotifierSettings.GapBetweenRepliesInSeconds) / 3;

		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() {
		Logger.Log.Info("Saving Data");

		Settings.Default.DashboardVM_FollowerNotifierIsEnabled = FollowerNotifierIsEnabled;
		Settings.Default.DashboardVM_SubscriberNotifierIsEnabled = SubscriberNotifierIsEnabled;

		Settings.Default.DashboardVM_TitleAndGame = TitleAndGameSettings;
		Settings.Default.DashboardVM_FollowerNotifier = FollowerNotifierSettings;
		Settings.Default.DashboardVM_NewSubscriberNotifier = NewSubscriberNotifierSettings;
		Settings.Default.DashboardVM_ReSubscriberNotifier = ReSubscriberNotifierSettings;
		Settings.Default.DashboardVM_GiftSubscriberNotifier = GiftSubscriberNotifierSettings;

		Settings.Default.Save();
	}

#endregion
}
}