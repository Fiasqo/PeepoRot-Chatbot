using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.Data.Collections;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Pages {
public class TimersViewModel : PropertyChangedNotifier, IPageViewModel {
#region Constructor

	public TimersViewModel(bool IsLazyLoading = true) {
		EditRowCmd = new Command(sender => {
			for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
				if (!(vis is DataGridRow row)) continue;
				if (row.Item is Timer editItem) {
					NewTimer.Response = editItem.Response;
					NewTimer.IntervalInMinutes = editItem.IntervalInMinutes;
					NewTimer.IsActivated = editItem.IsActivated;
				}

				DeleteRowCmd.Execute(sender);
				break;
			}
		});

		DeleteRowCmd = new Command(sender => {
			for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
				if (!(vis is DataGridRow row)) continue;
				Timers.Remove(row.Item as Timer);
				break;
			}
		});

		AddNewTimerCmd = new Command(_ => {
			if (ReferenceEquals(NewTimer, null)) throw new NullReferenceException(nameof(NewTimer));
			if (ReferenceEquals(Timers, null)) throw new NullReferenceException(nameof(Timers));
			if (NewTimer.GetHasError()) {
				MessageBox.Show("This Timer Contains Errors !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (Timers.Any(timer => timer.Response.Equals(NewTimer.Response) || timer.Equals(NewTimer))) {
				MessageBox.Show("This Timer Has Already Been Added To The Table !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Timers.Add(NewTimer);
			NewTimer = null;
			NewTimer = new Timer();
			//TODO: Update bot
		});

		if (IsLazyLoading) LoadDefault();
		else LoadDataOrDefault();
	}

#endregion

#region Fields

	private Timer _newTimer;
	private TimerCollection _timers;
	private bool _timersIsEnabled;

#endregion

#region Commands

	public Command AddNewTimerCmd { get; }
	public Command EditRowCmd { get; }
	public Command DeleteRowCmd { get; }
	public Command ApplyTimersCmd { get; } = new(_ => { });
	public Command TurnOnOffTimersCmd { get; } = new(_ => { });

#endregion

#region Properties

	public Timer NewTimer { get => _newTimer; private set => SetField(ref _newTimer, value); }
	public TimerCollection Timers { get => _timers; private set => SetField(ref _timers, value); }
	public bool TimersIsEnabled { get => _timersIsEnabled; set => SetField(ref _timersIsEnabled, value); }

#endregion

#region IPageViewModel

	public void LoadDefault() {
		Timers = new TimerCollection();
		NewTimer = new Timer();
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