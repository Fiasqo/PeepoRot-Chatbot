using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public abstract class Page : UserControl {
#region Properties

	public static readonly DependencyProperty PageNameProperty =
		DependencyProperty.Register("PageName", typeof(string), typeof(Page), new PropertyMetadata("No name"));

	public static readonly DependencyProperty PageIconKindProperty =
		DependencyProperty.Register("PageIconKind", typeof(PackIconKind), typeof(Page), new PropertyMetadata(PackIconKind.None));

	public static readonly DependencyProperty CanSwitchProperty =
		DependencyProperty.Register("CanSwitch", typeof(bool), typeof(Page), new PropertyMetadata(true));

	public static readonly DependencyProperty CanContentScrollProperty =
		DependencyProperty.Register("CanContentScroll", typeof(bool), typeof(Page), new PropertyMetadata(false));

	public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
		DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(Page), new PropertyMetadata(ScrollBarVisibility.Auto));

	public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
		DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(Page), new PropertyMetadata(ScrollBarVisibility.Auto));

	public string PageName { get => (string) GetValue(PageNameProperty); set => SetValue(PageNameProperty, value); }
	public PackIconKind PageIconKind { get => (PackIconKind) GetValue(PageIconKindProperty); set => SetValue(PageIconKindProperty, value); }
	public bool CanSwitch { get => (bool) GetValue(CanSwitchProperty); set => SetValue(CanSwitchProperty, value); }
	public bool CanContentScroll { get => (bool) GetValue(CanContentScrollProperty); set => SetValue(CanContentScrollProperty, value); }

	public ScrollBarVisibility HorizontalScrollBarVisibility {
		get => (ScrollBarVisibility) GetValue(HorizontalScrollBarVisibilityProperty);
		set => SetValue(HorizontalScrollBarVisibilityProperty, value);
	}

	public ScrollBarVisibility VerticalScrollBarVisibility {
		get => (ScrollBarVisibility) GetValue(VerticalScrollBarVisibilityProperty);
		set => SetValue(VerticalScrollBarVisibilityProperty, value);
	}

#endregion

#region Methods

	public void Disable() {
		IsHitTestVisible = false;
		IsEnabled = false;
		Visibility = Visibility.Collapsed;
	}

	public void Enable() {
		IsHitTestVisible = true;
		IsEnabled = true;
		Visibility = Visibility.Visible;
	}

#endregion
}
}