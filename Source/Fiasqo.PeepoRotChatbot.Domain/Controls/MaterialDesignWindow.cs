﻿using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public class MaterialDesignWindow : Window {
	public static readonly DependencyProperty WindowIconProperty =
		DependencyProperty.Register(nameof(WindowIcon),
									typeof(BitmapImage),
									typeof(MaterialDesignWindow),
									new FrameworkPropertyMetadata(null,
																  FrameworkPropertyMetadataOptions.AffectsRender |
																  FrameworkPropertyMetadataOptions.AffectsMeasure));

#region Constructors

	static MaterialDesignWindow()
		=> DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialDesignWindow), new FrameworkPropertyMetadata(typeof(MaterialDesignWindow)));

#endregion

	public BitmapImage WindowIcon { get => (BitmapImage) GetValue(WindowIconProperty); set => SetValue(WindowIconProperty, value); }

#region Public Methods

	public override void OnApplyTemplate() {
		Button minimizeButton = GetTemplateChild("MinimizeButton") as Button ?? throw new NoNullAllowedException("'MinimizeButton' is NULL.");
		Button restoreButton = GetTemplateChild("RestoreButton") as Button ?? throw new NoNullAllowedException("'RestoreButton' is NULL.");
		Button maximizeButton = GetTemplateChild("MaximizeButton") as Button ?? throw new NoNullAllowedException("'MaximizeButton' is NULL.");
		Button closeButton = GetTemplateChild("CloseButton") as Button ?? throw new NoNullAllowedException("'CloseButton' is NULL.");

		minimizeButton.Click += Minimize;
		restoreButton.Click += Restore;
		maximizeButton.Click += Maximize;
		closeButton.Click += Close;

		base.OnApplyTemplate();
	}

#endregion

#region Private Methods

	private void Minimize(object sender, RoutedEventArgs e) => SystemCommands.MinimizeWindow(this);
	private void Restore(object sender, RoutedEventArgs e) => SystemCommands.RestoreWindow(this);
	private void Maximize(object sender, RoutedEventArgs e) => SystemCommands.MaximizeWindow(this);
	private void Close(object sender, RoutedEventArgs e) => SystemCommands.CloseWindow(this);

#endregion
}
}