using System.Windows;

namespace Fiasqo.PeepoRotChatbot.View.Pages {
public partial class CommandsView {
	public CommandsView() => InitializeComponent();

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e) => CanSwitch = !CanSwitch;
}
}