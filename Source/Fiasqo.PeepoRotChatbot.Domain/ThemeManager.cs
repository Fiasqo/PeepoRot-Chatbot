using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Fiasqo.PeepoRotChatbot.Common.Extensions;
using MaterialDesignThemes.Wpf;

namespace Fiasqo.PeepoRotChatbot.Domain {
public static class ThemeManager {
	public static void SetTheme(IBaseTheme baseTheme, Color primaryColor, Color accentColor) {
		var theme = new Theme();
		theme.SetBaseTheme(baseTheme ?? new MaterialDesignDarkTheme());
		theme.SetPrimaryColor(primaryColor);
		theme.SetSecondaryColor(accentColor);
		new PaletteHelper().SetTheme(theme);

		ResourceDictionary res = Application.Current.Resources;

		res.SetColor("Primary200", "PrimaryLight", theme.PrimaryLight.Color);
		res.SetColor("Primary500", "PrimaryMid", theme.PrimaryMid.Color);
		res.SetColor("Primary700", "PrimaryDark", theme.PrimaryDark.Color);
		res.SetColor("Accent200", "AccentLight", theme.SecondaryLight.Color);
		res.SetColor("Accent500", "AccentMid", theme.SecondaryMid.Color);
		res.SetColor("Accent700", "AccentDark", theme.SecondaryDark.Color);

		res.SetColor("Primary200Foreground", "PrimaryLightForeground",
					 theme.PrimaryLight.ForegroundColor ?? theme.PrimaryLight.Color.Invert());
		res.SetColor("Primary500Foreground", "PrimaryMidForeground",
					 theme.PrimaryMid.ForegroundColor ?? theme.PrimaryMid.Color.Invert());
		res.SetColor("Primary700Foreground", "PrimaryDarkForeground",
					 theme.PrimaryDark.ForegroundColor ?? theme.PrimaryDark.Color.Invert());
		res.SetColor("Accent200Foreground", "AccentLightForeground",
					 theme.SecondaryLight.ForegroundColor ?? theme.SecondaryLight.Color.Invert());
		res.SetColor("Accent500Foreground", "AccentMidForeground",
					 theme.SecondaryMid.ForegroundColor ?? theme.SecondaryMid.Color.Invert());
		res.SetColor("Accent700Foreground", "AccentDarkForeground",
					 theme.SecondaryDark.ForegroundColor ?? theme.SecondaryDark.Color.Invert());
	}

	internal static void SetColor(this ResourceDictionary sourceDictionary,
								  string colorName,
								  string brushName,
								  Color value) {
		if (sourceDictionary == null)
			throw new ArgumentNullException(nameof(sourceDictionary));
		if (string.IsNullOrWhiteSpace(colorName))
			throw new ArgumentNullException(nameof(colorName));
		if (string.IsNullOrWhiteSpace(brushName))
			throw new ArgumentNullException(nameof(brushName));

		sourceDictionary[colorName] = value;

		if (sourceDictionary[brushName] is SolidColorBrush source) {
			if (source.Color == value)
				return;
			if (!source.IsFrozen) {
				var colorAnimation1 = new ColorAnimation {
					From = source.Color,
					To = value,
					Duration = new Duration(TimeSpan.FromMilliseconds(300.0)),
				};
				ColorAnimation colorAnimation2 = colorAnimation1;
				source.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation2);
				return;
			}
		}

		var solidColorBrush = new SolidColorBrush(value);
		solidColorBrush.Freeze();
		sourceDictionary[brushName] = solidColorBrush;
	}
}
}