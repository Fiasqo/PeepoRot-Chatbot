using System.Windows;

namespace Fiasqo.PeepoRotChatbot.View.Pages {
public partial class TimersView {
	public TimersView() => InitializeComponent();

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
		var b = CanSwitch;
		CanSwitch = !b;
	}
}
}