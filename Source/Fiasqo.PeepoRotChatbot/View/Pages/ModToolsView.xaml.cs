using System.Windows;

namespace Fiasqo.PeepoRotChatbot.View.Pages {
public partial class ModToolsView {
	public ModToolsView() => InitializeComponent();

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
		var b = CanSwitch;
		CanSwitch = !b;
	}
}
}