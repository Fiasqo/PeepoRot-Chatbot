using System.Windows;

namespace Fiasqo.PeepoRotChatbot.View.Pages {
public partial class SettingsView {
	public SettingsView() => InitializeComponent();

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
		var b = CanSwitch;
		CanSwitch = !b;
	}
}
}