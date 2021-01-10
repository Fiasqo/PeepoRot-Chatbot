using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fiasqo.PeepoRotChatbot.Common.Utilities;
using Fiasqo.PeepoRotChatbot.Domain;
using Fiasqo.PeepoRotChatbot.View.Windows;
using Fiasqo.PeepoRotChatbot.ViewModel.Windows;
using MaterialDesignThemes.Wpf;

namespace Fiasqo.PeepoRotChatbot {
/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	private Window _mainWindow;

	private void App_OnStartup(object sender, StartupEventArgs e) {
		try {
			_mainWindow = new MainWindowView {
				DataContext = new MainWindowViewModel(),
				FontFamily = new FontFamily(new Uri("pack://application:,,,/Fiasqo.PeepoRotChatbot;component/Resources/"), "#Comfortaa"),
				Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Fiasqo.PeepoRotChatbot;component/Resources/icon.ico")),
				WindowIcon = new BitmapImage(new Uri("pack://application:,,,/Fiasqo.PeepoRotChatbot;component/Resources/GifIcon.gif")),
			};

			ThemeManager.SetTheme(new MaterialDesignDarkTheme(), Color.FromRgb(55, 55, 60), Color.FromRgb(96, 125, 139));

			_mainWindow.Show();
		} catch (Exception exception) {
			Logger.Log.Fatal(exception.Message, exception);
			throw;
		}
	}

	private void App_OnExit(object sender, ExitEventArgs e) {
		if (_mainWindow.DataContext is IWindowViewModel mainWindowViewModel)
			mainWindowViewModel.SaveData();
		Logger.Log.Debug($"App Exit Code: {e.ApplicationExitCode}");
	}
}
}