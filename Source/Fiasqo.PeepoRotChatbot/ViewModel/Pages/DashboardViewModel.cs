using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public sealed class DashboardViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public DashboardViewModel(bool IsLazyLoading = true) {
		if (IsLazyLoading) LoadDefault();
		else LoadDataOrDefault();
	}

#endregion

#region Fields

	private bool _followerNotifierIsEnabled;
	private bool _subscriberNotifierIsEnabled;

#endregion

#region Properties

	public TitleAndGameSettings TitleAndGameSettings { get; private set; }
	public FollowerNotifierSettings FollowerNotifierSettings { get; private set; }
	public NewSubscriberNotifierSettings NewSubscriberNotifierSettings { get; private set; }
	public ReSubscriberNotifierSettings ReSubscriberNotifierSettings { get; private set; }
	public GiftSubscriberNotifierSettings GiftSubscriberNotifierSettings { get; private set; }

	public int SubscriptionDelayInSeconds {
		get => NewSubscriberNotifierSettings.GapBetweenReplies_Seconds;
		set {
			NewSubscriberNotifierSettings.GapBetweenReplies_Seconds =
				ReSubscriberNotifierSettings.GapBetweenReplies_Seconds =
					GiftSubscriberNotifierSettings.GapBetweenReplies_Seconds = value;
			OnPropertyChanged();
		}
	}

	public bool FollowerNotifierIsEnabled { get => _followerNotifierIsEnabled; set => SetField(ref _followerNotifierIsEnabled, value); }
	public bool SubscriberNotifierIsEnabled { get => _subscriberNotifierIsEnabled; set => SetField(ref _subscriberNotifierIsEnabled, value); }

#endregion

#region Commands

	public Command ApplyTitleAndGameSettingsCmd { get; } = new(x => { });
	public Command ApplyFollowerNotifierSettingsCmd { get; } = new(x => { });
	public Command ApplySubscriberNotifierSettingsCmd { get; } = new(x => { });

	public Command TurnOnOffFollowerNotifierCmd { get; } = new(x => { });
	public Command TurnOnOffSubscriberNotifierCmd { get; } = new(x => { });

#endregion

#region IPageViewModel

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

		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() => Logger.Log.Info("Saving Data");

#endregion
}
}