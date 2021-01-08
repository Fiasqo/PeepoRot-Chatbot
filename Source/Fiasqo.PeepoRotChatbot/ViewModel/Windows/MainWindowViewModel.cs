using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Fiasqo.PeepoRotChatbot.Common;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.Domain.Commands;
using Fiasqo.PeepoRotChatbot.Domain.Controls;
using Fiasqo.PeepoRotChatbot.View.Pages;
using Fiasqo.PeepoRotChatbot.ViewModel.Pages;
using Page = Fiasqo.PeepoRotChatbot.Domain.Controls.Page;

namespace Fiasqo.PeepoRotChatbot.ViewModel.Windows {
public class MainWindowViewModel : PropertyChangedNotifier, IWindowViewModel {
#region Constructor

	public MainWindowViewModel() {
		foreach (Page page in _pages) {
			page.Disable();
			MenuItems.Add(new HamburgerMenuItem {
				Text = page.PageName,
				Icon = page.PageIconKind,
				SelectionCommand = new Command(_ => {
					if (ReferenceEquals(SelectedPage, page)) return;

					Logger.Log.Info($"Changin page from {SelectedPage.PageName} to {page.PageName}");

					if (SelectedPage.DataContext is IPageViewModel {IsLoaded: true} oldPageViewModel) oldPageViewModel.SaveData();
					SelectedPage.Disable();

					if (page.DataContext is IPageViewModel {IsLoaded: false} newPageViewModel) newPageViewModel.LoadDataOrDefault();
					page.Enable();

					SelectedPage = page;
				}, _ => SelectedPage.CanSwitch),
			});
		}

		_pages[SelectedPageIndex].Enable();
		SelectedPage = _pages[SelectedPageIndex];
	}

#endregion

#region Methods

	private void UpdatePageMargin() {
		int b = UniformPageMargin - 14, r = b;
		if (SelectedPage.HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled) b = UniformPageMargin;
		if (SelectedPage.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled) r = UniformPageMargin;
		PageMargin = new Thickness(UniformPageMargin, UniformPageMargin, r, b);
	}

#endregion

#region Fields

	private readonly List<Page> _pages = new(5) {
		new DashboardView {
			DataContext = new DashboardViewModel(),
		},
		new CommandsView(),
		new TimersView(),
		new ModToolsView(),
		new SettingsView() {
			DataContext = new SettingsViewModel(),
		},
	};

	private string _title = $"{Constants.ApplicationAssemblyInfo.Title} 一 Version: {Constants.ApplicationAssemblyInfo.AppVersion}";
	private Page _selectedPage;
	private int _selectedPageIndex = 4;
	private Thickness _pageMargin;
	private int _uniformPageMargin = 16;

#endregion

#region Properties

	public List<HamburgerMenuItem> MenuItems { get; } = new(5);

	public string Title { get => _title; set => TrySetField(ref _title, value, (_, value) => !string.IsNullOrEmpty(value)); }

	public Page SelectedPage {
		get => _selectedPage;
		private set {
			if (!TrySetField(ref _selectedPage, value, (_, value) => !ReferenceEquals(value, null))) return;
			SelectedPageIndex = _pages.IndexOf(_selectedPage);
			UpdatePageMargin();
		}
	}

	public int SelectedPageIndex {
		get => _selectedPageIndex;
		set {
			if (value != _pages.IndexOf(_selectedPage)) {
				if (value >= 0 && value < _pages.Count) SelectedPage = _pages[value];
				return;
			}

			SetField(ref _selectedPageIndex, value);
		}
	}

	public Thickness PageMargin { get => _pageMargin; private set => SetField(ref _pageMargin, value); }

	public int UniformPageMargin {
		get => _uniformPageMargin;
		set {
			if (TrySetField(ref _uniformPageMargin, value, (_, value) => value >= 16)) UpdatePageMargin();
		}
	}

#endregion

#region IWindowViewModel

	/// <inheritdoc />
	public bool IsLoaded => _pages.Count(value => value.DataContext is not IPageViewModel pageViewModel || pageViewModel.IsLoaded) == _pages.Count;

	/// <inheritdoc />
	public void LoadDataOrDefault() {
		foreach (Page page in _pages)
			if (page.DataContext is IPageViewModel {IsLoaded: false} pageViewModel)
				pageViewModel.LoadDataOrDefault();
	}

	/// <inheritdoc />
	public void SaveData() {
		foreach (Page page in _pages)
			if (page.DataContext is IPageViewModel {IsLoaded: false} pageViewModel)
				pageViewModel.SaveData();
	}

#endregion
}
}