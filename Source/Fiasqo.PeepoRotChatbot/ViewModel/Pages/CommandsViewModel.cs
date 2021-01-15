using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.Data.Collections;
using Fiasqo.PeepoRotChatbot.Model.ShittyBot;
using Settings = Fiasqo.PeepoRotChatbot.Properties.Settings;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class CommandsViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public CommandsViewModel() {
		EditRowCmd = new Command(sender => {
			for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
				if (!(vis is DataGridRow row)) continue;
				if (row.Item is ChatCommand editItem) {
					NewChatCommand.Command = editItem.Command;
					NewChatCommand.Reply = editItem.Reply;
					NewChatCommand.Permission = editItem.Permission;
					NewChatCommand.IsActivated = editItem.IsActivated;
				}

				DeleteRowCmd.Execute(sender);
				break;
			}
		});

		DeleteRowCmd = new Command(sender => {
			for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
				if (!(vis is DataGridRow row)) continue;
				ChatCommands.Remove(row.Item as ChatCommand);
				break;
			}
		});

		AddNewChatCommandCmd = new Command(_ => {
			if (ReferenceEquals(NewChatCommand, null)) throw new NullReferenceException(nameof(NewChatCommand));
			if (ReferenceEquals(ChatCommands, null)) throw new NullReferenceException(nameof(ChatCommands));
			if (NewChatCommand.Error != string.Empty) {
				MessageBox.Show("This Chat Command Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (ChatCommands.Any(chatCommand => chatCommand.Command.Equals(NewChatCommand.Command) || chatCommand.Equals(NewChatCommand))) {
				MessageBox.Show("This Chat Command Has Already Been Added To The Table !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			ChatCommands.Add(NewChatCommand);
			NewChatCommand = null;
			NewChatCommand = new ChatCommand();
		});

		ApplyChatCommandCmd = new Command(_ => {
			if (ReferenceEquals(ChatCommands, null)) throw new NullReferenceException(nameof(ChatCommands));
			if (ChatCommands.Count == 0) {
				MessageBox.Show("You Need To Create At Least One Command !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try {
				Bot.Instance.ChatCommands = ChatCommands;
				Bot.Instance.TurnChatCommands(true);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				ChatCommands = new ChatCommandCollection();
			}
		});

		PropertyChanged += OnChatCommandsIsEnabledChanged;

		LoadDataOrDefault();
	}

#endregion

#region Private Methods

	private void OnChatCommandsIsEnabledChanged(object sender, PropertyChangedEventArgs e) {
		if (e.PropertyName == nameof(ChatCommandsIsEnabled)) { }
	}

#endregion

#region Fields

	private ChatCommand _newChatCommand;
	private ChatCommandCollection _chatCommands;
	private bool _chatCommandsIsEnable;

#endregion

#region Commands

	public Command AddNewChatCommandCmd { get; }
	public Command EditRowCmd { get; }
	public Command DeleteRowCmd { get; }
	public Command ApplyChatCommandCmd { get; }

#endregion

#region Properties

	public ChatCommand NewChatCommand { get => _newChatCommand; private set => SetField(ref _newChatCommand, value); }
	public ChatCommandCollection ChatCommands { get => _chatCommands; private set => SetField(ref _chatCommands, value); }
	public bool ChatCommandsIsEnabled { get => _chatCommandsIsEnable; set => SetField(ref _chatCommandsIsEnable, value); }

#endregion

#region IPageViewModel

	/// <inheritdoc />
	public bool CanSwitchPage { get; } = true;

	/// <inheritdoc />
	public bool IsLoaded { get; private set; }

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		Logger.Log.Info("Loading Data");

		if (!ReferenceEquals(Settings.Default.CommandsVM_ChatCommands, null)) {
			ChatCommands = Settings.Default.CommandsVM_ChatCommands;
			ChatCommandsIsEnabled = Settings.Default.CommandsVM_ChatCommandsIsEnabled;
			try {
				Bot.Instance.ChatCommands = ChatCommands;
				Bot.Instance.TurnWordProtector(ChatCommandsIsEnabled);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				ChatCommands = new ChatCommandCollection();
				ChatCommandsIsEnabled = false;
			}
		} else {
			ChatCommands = new ChatCommandCollection();
		}
		
		NewChatCommand ??= new ChatCommand();

		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() {
		Logger.Log.Info("Saving Data");
		Settings.Default.CommandsVM_ChatCommands = ChatCommands;
		Settings.Default.CommandsVM_ChatCommandsIsEnabled = ChatCommandsIsEnabled;
		Settings.Default.Save();
	}

#endregion
}
}