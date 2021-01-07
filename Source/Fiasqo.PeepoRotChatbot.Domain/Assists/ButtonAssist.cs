using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

/*
 * TODO: Переписать целиком и полностью
 */

namespace Fiasqo.PeepoRotChatbot.Domain.Assists {
public class ButtonAssist {
	private static readonly Dictionary<Button, MaterialDesignColors> _fromForegroundBrushes = new();
	private static readonly Dictionary<Button, MaterialDesignColors> _toForegroundBrushes = new();

	public static readonly DependencyProperty AnimateMouseOverProperty = DependencyProperty.RegisterAttached(
		"AnimateMouseOver",
		typeof(bool),
		typeof(ButtonAssist),
		new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, AnimateMouseOverChangedCallback));

	private static void AnimateMouseOverChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
		if (!(d is Button button)) return;
		if ((bool) e.NewValue) {
			button.MouseEnter += ButtonOnMouseEnter;
			button.MouseLeave += ButtonOnMouseLeave;
		} else {
			_fromForegroundBrushes.Remove(button);
			_toForegroundBrushes.Remove(button);
			button.MouseEnter -= ButtonOnMouseEnter;
			button.MouseLeave -= ButtonOnMouseLeave;
		}
	}

	public static void SetAnimateMouseOver(DependencyObject element, bool value) => element.SetValue(AnimateMouseOverProperty, value);

	public static bool GetAnimateMouseOver(DependencyObject element) => (bool) element.GetValue(AnimateMouseOverProperty);

	public static void SetTo(DependencyObject element, MaterialDesignColors value) {
		if (!(element is Button button)) return;
		if (_toForegroundBrushes.ContainsKey(button)) _toForegroundBrushes.Remove(button);
		_toForegroundBrushes.Add(button, value);
	}

	public static void SetFrom(DependencyObject element, MaterialDesignColors value) {
		if (!(element is Button button)) return;
		if (_fromForegroundBrushes.ContainsKey(button)) _fromForegroundBrushes.Remove(button);
		_fromForegroundBrushes.Add(button, value);
	}

	private static void ButtonOnMouseEnter(object sender, MouseEventArgs e) {
		if (!(sender is Button button)) return;
		if (!_fromForegroundBrushes.ContainsKey(button)) return;
		if (!_toForegroundBrushes.ContainsKey(button)) return;

		button.Foreground = new SolidColorBrush(((SolidColorBrush) button.Foreground).Color);
		var foregroundColorAnimation = new ColorAnimation(
			ChangeColorBrightness((Color) Application.Current.Resources[_fromForegroundBrushes[button].ToString()], 0.33f),
			ChangeColorBrightness((Color) Application.Current.Resources[_toForegroundBrushes[button].ToString()], 0.33f),
			new Duration(TimeSpan.FromMilliseconds(200)));
		button.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColorAnimation);
	}

	private static void ButtonOnMouseLeave(object sender, MouseEventArgs e) {
		if (!(sender is Button button)) return;
		if (!_fromForegroundBrushes.ContainsKey(button)) return;
		if (!_toForegroundBrushes.ContainsKey(button)) return;

		button.Foreground = new SolidColorBrush(((SolidColorBrush) button.Foreground).Color);
		var foregroundColorAnimation = new ColorAnimation(
			((SolidColorBrush) button.Foreground).Color,
			(Color) Application.Current.Resources[_fromForegroundBrushes[button].ToString()],
			new Duration(TimeSpan.FromMilliseconds(200)));

		foregroundColorAnimation.Completed += (_, _) => {
			var resName = _fromForegroundBrushes[button] switch {
				MaterialDesignColors.Error500             => "ErrorMid",
				MaterialDesignColors.Accent200            => "SecondaryHueMidBrush",
				MaterialDesignColors.Accent400            => "SecondaryHueMidBrush",
				MaterialDesignColors.Accent700            => "SecondaryHueMidBrush",
				MaterialDesignColors.Primary200           => "PrimaryHueLightBrush",
				MaterialDesignColors.Primary500           => "PrimaryHueMidBrush",
				MaterialDesignColors.Primary700           => "PrimaryHueDarkBrush",
				MaterialDesignColors.Primary200Foreground => "PrimaryHueLightForegroundBrush",
				MaterialDesignColors.Primary500Foreground => "PrimaryHueMidForegroundBrush",
				MaterialDesignColors.Primary700Foreground => "PrimaryHueDarkForegroundBrush",
				_                                         => "SecondaryHueMidForegroundBrush",
			};

			button.SetResourceReference(Control.ForegroundProperty, resName);
		};
		button.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, foregroundColorAnimation);
	}

	private static Color ChangeColorBrightness(Color color, float correctionFactor) {
		float red = color.R;
		float green = color.G;
		float blue = color.B;

		if (correctionFactor < 0) {
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		} else {
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}

		return Color.FromArgb(color.A, (byte) red, (byte) green, (byte) blue);
	}
}
}