using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.Data.Collections;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using ChatCommand = Fiasqo.PeepoRotChatbot.Model.Data.ChatCommand;

namespace Fiasqo.PeepoRotChatbot.Model.ShittyBot {
public class Bot {
#region Singleton

	static Bot() { }
	private Bot() { }
	private static readonly Lazy<Bot> _lazyInstance = new(() => new Bot());
	public static Bot Instance => _lazyInstance.Value;

#endregion

#region Fields

	private const string _linkRegexPattern = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

	private bool isStreamOnline;
	private DateTime streamStartedAt;
	
	private bool isConnected => (_client?.IsConnected ?? false) && (_client?.IsInitialized ?? false);

	private FollowerService _followerService;
	private LiveStreamMonitorService _liveStreamMonitorService;
	private ConnectionCredentials _creds;
	private TwitchClient _client;
	private TwitchAPI _api;
	private TwitchAPI _userAPI;
	
#endregion

#region Properties

	public TitleAndGameSettings TitleAndGameSettings { get; set; }
	public FollowerNotifierSettings FollowerNotifierSettings { get; set; }
	public NewSubscriberNotifierSettings NewSubscriberNotifierSettings { get; set; }
	public ReSubscriberNotifierSettings ReSubscriberNotifierSettings { get; set; }
	public GiftSubscriberNotifierSettings GiftSubscriberNotifierSettings { get; set; }

	public ChatCommandCollection ChatCommands { get; set; }
	public TimerCollection Timers { get; set; }

	public WordProtection WordProtectorSettings { get; set; }
	public LinkProtection LinkProtectorSettings { get; set; }
	public CapsProtection CapsProtectorSettings { get; set; }

	public Settings Settings { get; set; }
	
#endregion

#region Private Methods

	private void ClientOnLog(object sender, OnLogArgs e) => Console.WriteLine(e.Data);

	private void ClientOnError(object sender, OnErrorEventArgs e) => Logger.Log.Error(e.Exception.Message, e.Exception);

	private void ClientOnConnected(object sender, OnConnectedArgs e) {
		Console.WriteLine(@"[Bot]: Connected");
		_client.SendMessage(Settings.UserChannelName, $"I am inside Chat Kreygasm");
	}

	private void ClientOnDisconnected(object sender, OnDisconnectedEventArgs e) => Console.WriteLine($@"[Bot]: Disonnecting and closing application. {e}");

	private bool HasPermissions(ChatMessage cm, Permissions target) => target switch {
		Permissions.Anyone     => true,
		Permissions.Subscriber => cm.IsModerator || cm.IsSubscriber || cm.IsBroadcaster,
		Permissions.Moderator  => cm.IsModerator || cm.IsBroadcaster,
		_                      => cm.IsModerator || cm.IsBroadcaster
	};

	private void PunishUser(ChatMessage chatMessage, ModToolsProtection protectionSettings) {
		switch (protectionSettings.Punishment) {
			case Punishments.Ban:
				_client.BanUser(Settings.UserChannelName, chatMessage.Username, protectionSettings.Reply);
				break;
			case Punishments.Timeout:
				_client.TimeoutUser(Settings.UserChannelName, chatMessage.Username, TimeSpan.FromSeconds(protectionSettings.PunishmentInSeconds));
				break;
			case Punishments.Purge:
				_client.TimeoutUser(Settings.UserChannelName, chatMessage.Username, TimeSpan.FromSeconds(1));
				break;
		}
	}

#endregion

#region events

	private void OnNewFollower(object sender, OnNewFollowersDetectedArgs e) {
		foreach (Follow follower in e.NewFollowers) {
			var prefix = FollowerNotifierSettings.Prefix.Replace(@"@username", $"@{follower.FromUserName}");
			_client.SendMessage(Settings.UserChannelName, $"{prefix}{FollowerNotifierSettings.Reply}");
		}
	}

	private void OnNewSubscriber(object sender, OnNewSubscriberArgs e) {
		if (!isConnected) return;
		var prefix = NewSubscriberNotifierSettings.Prefix.Replace(@"@username", $"@{e.Subscriber.DisplayName}");
		_client.SendMessage(Settings.UserChannelName, $"{prefix}{NewSubscriberNotifierSettings.Reply}");
	}

	private void OnReSubscriber(object sender, OnReSubscriberArgs e) {
		if (!isConnected) return;
		var prefix = ReSubscriberNotifierSettings.Prefix
												 .Replace(@"@username", $"@{e.ReSubscriber.DisplayName}")
												 .Replace("xN", $"x{e.ReSubscriber.Months}");
		_client.SendMessage(Settings.UserChannelName, $"{prefix}{ReSubscriberNotifierSettings.Reply}");
	}

	private void OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e) {
		if (!isConnected) return;
		var regex = new Regex(Regex.Escape(@"@username"));
		var prefix = regex
					.Replace(GiftSubscriberNotifierSettings.Prefix, e.GiftedSubscription.DisplayName, 1)
					.Replace(@"@username", $"@{e.GiftedSubscription.MsgParamRecipientDisplayName}");
		_client.SendMessage(Settings.UserChannelName, $"{prefix}{GiftSubscriberNotifierSettings.Reply}");
	}

	private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
		if (!isConnected) return;
		if (e.Command.CommandText.ToLower() == "showtime") {
			if (isStreamOnline) {
				var time = (DateTime.Now - streamStartedAt);
				_client.SendMessage(Settings.UserChannelName, $"@{e.Command.ChatMessage.DisplayName} {time:G}");
			} else {
				var time = (DateTime.Now - DateTime.Now.AddMinutes(10).AddSeconds(23).AddMilliseconds(53));
				_client.SendMessage(Settings.UserChannelName, $"@{e.Command.ChatMessage.DisplayName} {time:G}");
			}
		}
		else {
			foreach (ChatCommand chatCommand in ChatCommands) {
				if (chatCommand.Command != e.Command.CommandText) continue;
				if (HasPermissions(e.Command.ChatMessage, chatCommand.Permission)) _client.SendMessage(Settings.UserChannelName, chatCommand.Reply);
				break;
			}	
		}
	}

	private void CheckMessageForCapsViolation(object sender, OnMessageReceivedArgs e) {
		if (!isConnected) return;
		ChatMessage cm = e.ChatMessage;
		if (cm.Message.Count(char.IsUpper) >= CapsProtectorSettings.MinCapsAmount && !HasPermissions(cm, CapsProtectorSettings.Exempt))
			PunishUser(cm, CapsProtectorSettings);
	}

	private void CheckMessageForLinksViolation(object sender, OnMessageReceivedArgs e) {
		if (!isConnected) return;
		ChatMessage cm = e.ChatMessage;
		if (Regex.IsMatch(cm.Message, _linkRegexPattern) && !HasPermissions(cm, LinkProtectorSettings.Exempt))
			PunishUser(cm, LinkProtectorSettings);
	}


	private void CheckMessageForWordsViolation(object sender, OnMessageReceivedArgs e) {
		if (!isConnected) return;
		ChatMessage cm = e.ChatMessage;
		foreach (var badWord in WordProtectorSettings.BadWords.Split(','))
			if (cm.Message.ToLower().Contains(badWord.ToLower()) && !HasPermissions(cm, WordProtectorSettings.Exempt))
				PunishUser(cm, WordProtectorSettings);
	}

	private void OnStreamOnline(object sender, OnStreamOnlineArgs e) {
		isStreamOnline = true;
		streamStartedAt = e.Stream.StartedAt;
	}

	private void OnStreamOffline(object sender, OnStreamOfflineArgs e) => isStreamOnline = false;
	
#endregion

#region Pub Methods

	public void Connect() {
		while (true) {
			if (ReferenceEquals(Settings, null)) throw new NullReferenceException(nameof(Settings));

			_creds = new ConnectionCredentials(Settings.BotChannelName, Settings.BotAccessToken);
			_client = new TwitchClient();
			_api = new TwitchAPI {Settings = {ClientId = Settings.BotClienID, AccessToken = Settings.BotAccessToken, Secret = Settings.BotRefreshToken}};

			_client.Initialize(_creds, Settings.UserChannelName);
			_client.OnLog += ClientOnLog;

			_client.OnError += ClientOnError;

			_client.Connect();
			_client.OnConnected += ClientOnConnected;
			_client.OnDisconnected += ClientOnDisconnected;

			_userAPI = new TwitchAPI() {
				Settings = {
					ClientId = Settings.UserClienID,
					AccessToken = Settings.UserAccessToken
				}
			};
			_liveStreamMonitorService = new LiveStreamMonitorService(_userAPI);
			_liveStreamMonitorService.OnStreamOffline += OnStreamOffline;
			_liveStreamMonitorService.OnStreamOnline += OnStreamOnline;
			
			if (_client.IsConnected) break;
		}
	}

	public void Disconnect() {
		Console.WriteLine(@"[Bot]: Disonnecting and closing application");
		if (isConnected) {
			TurnFollowerNotifier(false);
			TurnSubscriberNotifier(false);
			TurnChatCommands(false);
			TurnTimers(false);
			TurnCapsProtector(false);
			TurnLinkProtector(false);
			TurnWordProtector(false);

			_client.Disconnect();
		}
	}

	public async void UpdateTitleAndGameAsync() {
		try {
			var userAPI = new TwitchAPI() {
				Settings = {
					ClientId = Settings.UserClienID,
					AccessToken = Settings.UserAccessToken
				}
			};
			await userAPI.V5.Channels.UpdateChannelAsync(userAPI.V5.Channels.GetChannelAsync(Settings.UserAccessToken).Result.Id, TitleAndGameSettings.Title, TitleAndGameSettings.GameName);
		} catch (Exception e) {
			Logger.Log.Error(e.Message, e);
		}
	}

	public void TurnFollowerNotifier(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(FollowerNotifierSettings, null)) throw new NullReferenceException(nameof(FollowerNotifierSettings));
			if (!ReferenceEquals(_followerService, null)) return;
			_followerService = new FollowerService(_api);
			_followerService.SetChannelsByName(new List<string>(1) {Settings.UserChannelName});
			_followerService.OnNewFollowersDetected += OnNewFollower;
			_followerService.Start();
		} else {
			if (ReferenceEquals(_followerService, null)) return;
			_followerService.Stop();
			_followerService.OnNewFollowersDetected -= OnNewFollower;
			_followerService = null;
		}
	}

	public void TurnSubscriberNotifier(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(NewSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(NewSubscriberNotifierSettings));
			if (ReferenceEquals(ReSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(ReSubscriberNotifierSettings));
			if (ReferenceEquals(GiftSubscriberNotifierSettings, null)) throw new NullReferenceException(nameof(GiftSubscriberNotifierSettings));
			try {
				_client.OnNewSubscriber -= OnNewSubscriber;
				_client.OnReSubscriber -= OnReSubscriber;
				_client.OnGiftedSubscription -= OnGiftedSubscription;
				_client.OnNewSubscriber += OnNewSubscriber;
				_client.OnReSubscriber += OnReSubscriber;
				_client.OnGiftedSubscription += OnGiftedSubscription;
			} catch (Exception e) {
				Logger.Log.Warn(e.Message, e);
				_client.OnNewSubscriber += OnNewSubscriber;
				_client.OnReSubscriber += OnReSubscriber;
				_client.OnGiftedSubscription += OnGiftedSubscription;
			}
		} else {
			_client.OnNewSubscriber -= OnNewSubscriber;
			_client.OnReSubscriber -= OnReSubscriber;
			_client.OnGiftedSubscription -= OnGiftedSubscription;
		}
	}

	public void TurnChatCommands(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(ChatCommands, null)) throw new NullReferenceException(nameof(ChatCommands));
			try {
				_client.OnChatCommandReceived -= OnChatCommandReceived;
				_client.OnChatCommandReceived += OnChatCommandReceived;
			} catch (Exception e) {
				Logger.Log.Warn(e.Message, e);
				_client.OnChatCommandReceived += OnChatCommandReceived;
			}
		} else {
			_client.OnChatCommandReceived -= OnChatCommandReceived;
		}
	}

	public void TurnTimers(bool on) {
		if (!isConnected) return;
		//TODO: TIMERS
		if (on) {
			if (ReferenceEquals(Timers, null)) throw new NullReferenceException(nameof(Timers));
		} else {
			
		} 
	}

	public void TurnCapsProtector(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(CapsProtectorSettings, null)) throw new NullReferenceException(nameof(CapsProtectorSettings));
			try {
				_client.OnMessageReceived -= CheckMessageForCapsViolation;
				_client.OnMessageReceived += CheckMessageForCapsViolation;
			} catch (Exception e) {
				Logger.Log.Warn(e.Message, e);
				_client.OnMessageReceived += CheckMessageForCapsViolation;
			}
		} else {
			_client.OnMessageReceived -= CheckMessageForCapsViolation;
		}
	}

	public void TurnLinkProtector(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(LinkProtectorSettings, null)) throw new NullReferenceException(nameof(LinkProtectorSettings));
			try {
				_client.OnMessageReceived -= CheckMessageForLinksViolation;
				_client.OnMessageReceived += CheckMessageForLinksViolation;
			} catch (Exception e) {
				Logger.Log.Warn(e.Message, e);
				_client.OnMessageReceived += CheckMessageForLinksViolation;
			}
		} else {
			_client.OnMessageReceived -= CheckMessageForLinksViolation;
		}
	}

	public void TurnWordProtector(bool on) {
		if (!isConnected) return;
		if (on) {
			if (ReferenceEquals(WordProtectorSettings, null)) throw new NullReferenceException(nameof(WordProtectorSettings));
			try {
				_client.OnMessageReceived -= CheckMessageForWordsViolation;
				_client.OnMessageReceived += CheckMessageForWordsViolation;
			} catch (Exception e) {
				Logger.Log.Warn(e.Message, e);
				_client.OnMessageReceived += CheckMessageForWordsViolation;
			}
		} else {
			_client.OnMessageReceived -= CheckMessageForWordsViolation;
		}
	}

#endregion
}
}
//"Bot is not connected but an attempt to enable the feature."