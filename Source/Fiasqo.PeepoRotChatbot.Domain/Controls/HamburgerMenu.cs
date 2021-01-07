using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public class HamburgerMenu : ContentControl {
#region Constructors

	static HamburgerMenu() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));

#endregion

#region Public Methods

	public override void BeginInit() {
		Content = new List<HamburgerMenuItem>();
		base.BeginInit();
	}

#endregion

#region Properties

	public new static readonly DependencyProperty ContentProperty =
		DependencyProperty.Register("Content", typeof(List<HamburgerMenuItem>), typeof(HamburgerMenu),
									new FrameworkPropertyMetadata(
										null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

	public static readonly DependencyProperty IsOpenProperty =
		DependencyProperty.Register("IsOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(true));

	public static readonly DependencyProperty MenuIconColorProperty =
		DependencyProperty.Register("MenuIconColor", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(Brushes.White));

	public static readonly DependencyProperty SelectionIndicatorColorProperty =
		DependencyProperty.Register("SelectionIndicatorColor", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(Brushes.Red));

	public static readonly DependencyProperty MenuItemForegroundProperty =
		DependencyProperty.Register("MenuItemForeground", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(Brushes.Transparent));

	public static readonly DependencyProperty SelectedIndexProperty =
		DependencyProperty.Register("SelectedIndex", typeof(int), typeof(HamburgerMenu), new PropertyMetadata(0));

	public new List<HamburgerMenuItem> Content { get => (List<HamburgerMenuItem>) GetValue(ContentProperty); set => SetValue(ContentProperty, value); }
	public bool IsOpen { get => (bool) GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
	public Brush MenuIconColor { get => (Brush) GetValue(MenuIconColorProperty); set => SetValue(MenuIconColorProperty, value); }
	public Brush SelectionIndicatorColor { get => (Brush) GetValue(SelectionIndicatorColorProperty); set => SetValue(SelectionIndicatorColorProperty, value); }
	public Brush MenuItemForeground { get => (Brush) GetValue(MenuItemForegroundProperty); set => SetValue(MenuItemForegroundProperty, value); }
	public int SelectedIndex { get => (int) GetValue(SelectedIndexProperty); set => SetValue(SelectedIndexProperty, value); }

#endregion
}
}