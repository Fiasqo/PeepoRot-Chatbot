using System;
using System.ComponentModel;
using System.Windows;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.ShittyBot;
using Settings = Fiasqo.PeepoRotChatbot.Properties.Settings;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class ModToolsViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public ModToolsViewModel() {
		ApplyCapsProtectionCmd = new Command(_ => {
			if (ReferenceEquals(CapsProtection, null)) throw new NullReferenceException(nameof(CapsProtection));
			if (CapsProtection.Error != string.Empty) {
				MessageBox.Show("Caps Protection Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			
			try {
				Bot.Instance.CapsProtectorSettings = CapsProtection;
				Bot.Instance.TurnCapsProtector(true);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				CapsProtection = new CapsProtection();
			}
		});

		ApplyLinkProtectionCmd = new Command(_ => {
			if (ReferenceEquals(LinkProtection, null)) throw new NullReferenceException(nameof(LinkProtection));
			if (LinkProtection.Error != string.Empty) {
				MessageBox.Show("Link Protection Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try {
				Bot.Instance.LinkProtectorSettings = LinkProtection;
				Bot.Instance.TurnLinkProtector(true);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				LinkProtection = new LinkProtection();
			}
		});

		ApplyWordProtectionCmd = new Command(_ => {
			if (ReferenceEquals(WordProtection, null)) throw new NullReferenceException(nameof(WordProtection));
			if (WordProtection.Error != string.Empty) {
				MessageBox.Show("Word Protection Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrWhiteSpace(WordProtection.BadWords)) {
				MessageBox.Show("You Need To Write At Least One Foul Word !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try {
				Bot.Instance.WordProtectorSettings = WordProtection;
				Bot.Instance.TurnWordProtector(true);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				WordProtection = new WordProtection();
			}
		});

		PropertyChanged += OnPropertyChanged;

		LoadDataOrDefault();
	}

#endregion

#region Private Methods

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
		switch (e.PropertyName) {
			case nameof(CapsProtectionIsEnabled):
				OnCapsProtectionIsEnabledChanged();
				break;
			case nameof(LinkProtectionIsEnabled):
				OnLinkProtectionIsEnabledChanged();
				break;
			case nameof(WordProtectionIsEnabled):
				OnWordProtectionIsEnabledChanged();
				break;
			case nameof(CapsProtection):
				CapsProtection.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(LinkProtection):
				LinkProtection.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
			case nameof(WordProtection):
				WordProtection.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSwitchPage));
				break;
		}
	}

	private void OnCapsProtectionIsEnabledChanged() { }

	private void OnLinkProtectionIsEnabledChanged() { }

	private void OnWordProtectionIsEnabledChanged() { }

#endregion

#region Fields

	private CapsProtection _capsProtection;
	private LinkProtection _linkProtection;
	private WordProtection _wordProtection;

	private bool _capsProtectionIsEnabled;
	private bool _linkProtectionIsEnabled;
	private bool _wordProtectionIsEnabled;

#endregion

#region Properties

	public CapsProtection CapsProtection { get => _capsProtection; private set => SetField(ref _capsProtection, value); }
	public LinkProtection LinkProtection { get => _linkProtection; private set => SetField(ref _linkProtection, value); }
	public WordProtection WordProtection { get => _wordProtection; private set => SetField(ref _wordProtection, value); }

	public bool CapsProtectionIsEnabled { get => _capsProtectionIsEnabled; set => SetField(ref _capsProtectionIsEnabled, value); }
	public bool LinkProtectionIsEnabled { get => _linkProtectionIsEnabled; set => SetField(ref _linkProtectionIsEnabled, value); }
	public bool WordProtectionIsEnabled { get => _wordProtectionIsEnabled; set => SetField(ref _wordProtectionIsEnabled, value); }

#endregion

#region Commands

	public Command ApplyCapsProtectionCmd { get; }
	public Command ApplyLinkProtectionCmd { get; }
	public Command ApplyWordProtectionCmd { get; }

#endregion

#region IPageViewModel

	/// <inheritdoc />
	public bool CanSwitchPage => CapsProtection.Error == string.Empty &&
								 LinkProtection.Error == string.Empty &&
								 WordProtection.Error == string.Empty;

	/// <inheritdoc />
	public bool IsLoaded { get; private set; }

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		Logger.Log.Info("Loading Data");
		
		if (!ReferenceEquals(Settings.Default.ModToolsVM_CapsProtection, null)) {
			CapsProtection = Settings.Default.ModToolsVM_CapsProtection;
			CapsProtectionIsEnabled = Settings.Default.ModToolsVM_CapsProtectionIsEnabled;
			try {
				Bot.Instance.CapsProtectorSettings = CapsProtection;
				Bot.Instance.TurnCapsProtector(CapsProtectionIsEnabled);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				CapsProtection = new CapsProtection();
				CapsProtectionIsEnabled = false;
			}
		} else {
			CapsProtection = new CapsProtection();
		}
		
		if (!ReferenceEquals(Settings.Default.ModToolsVM_LinkProtection, null)) {
			LinkProtection = Settings.Default.ModToolsVM_LinkProtection;
			LinkProtectionIsEnabled = Settings.Default.ModToolsVM_LinkProtectionIsEnabled;
			try {
				Bot.Instance.LinkProtectorSettings = LinkProtection;
				Bot.Instance.TurnLinkProtector(LinkProtectionIsEnabled);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				LinkProtection = new LinkProtection();
				LinkProtectionIsEnabled = false;
			}
		} else {
			LinkProtection = new LinkProtection();
		}
		
		if (!ReferenceEquals(Settings.Default.ModToolsVM_WordProtection, null)) {
			WordProtection = Settings.Default.ModToolsVM_WordProtection;
			WordProtectionIsEnabled = Settings.Default.ModToolsVM_WordProtectionIsEnabled;
			try {
				Bot.Instance.WordProtectorSettings = WordProtection;
				Bot.Instance.TurnWordProtector(WordProtectionIsEnabled);
			} catch (Exception e) {
				Logger.Log.Error(e.Message, e);
				WordProtection = new WordProtection();
				WordProtectionIsEnabled = false;
			}
		} else {
			WordProtection = new WordProtection();
		}
		
		IsLoaded = true;
	}

	/// <inheritdoc />
	public void SaveData() {
		Logger.Log.Info("Saving Data");
		Settings.Default.ModToolsVM_CapsProtection = CapsProtection;
		Settings.Default.ModToolsVM_LinkProtection = LinkProtection;
		Settings.Default.ModToolsVM_WordProtection = WordProtection;
		Settings.Default.ModToolsVM_CapsProtectionIsEnabled = CapsProtectionIsEnabled;
		Settings.Default.ModToolsVM_LinkProtectionIsEnabled = LinkProtectionIsEnabled;
		Settings.Default.ModToolsVM_WordProtectionIsEnabled = WordProtectionIsEnabled;
		Settings.Default.Save();
	}

#endregion
}
}