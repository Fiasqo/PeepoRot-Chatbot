using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public class HamburgerMenuItem : ListBoxItem {
#region Constructors

	static HamburgerMenuItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));

#endregion

#region Properties

	public static readonly DependencyProperty TextProperty =
		DependencyProperty.Register("Text", typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(string.Empty));

	public static readonly DependencyProperty IconProperty =
		DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(HamburgerMenuItem), new PropertyMetadata(PackIconKind.None));

	public static readonly DependencyProperty SelectionIndicatorColorProperty =
		DependencyProperty.Register("SelectionIndicatorColor", typeof(Brush), typeof(HamburgerMenuItem), new PropertyMetadata(Brushes.Blue));

	public static readonly DependencyProperty SelectionCommandProperty =
		DependencyProperty.Register("SelectionCommand", typeof(ICommand), typeof(HamburgerMenuItem), new PropertyMetadata(null));

	public string Text { get => (string) GetValue(TextProperty); set => SetValue(TextProperty, value); }
	public PackIconKind Icon { get => (PackIconKind) GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public Brush SelectionIndicatorColor { get => (Brush) GetValue(SelectionIndicatorColorProperty); set => SetValue(SelectionIndicatorColorProperty, value); }
	public ICommand SelectionCommand { get => (ICommand) GetValue(SelectionCommandProperty); set => SetValue(SelectionCommandProperty, value); }

#endregion
}
}