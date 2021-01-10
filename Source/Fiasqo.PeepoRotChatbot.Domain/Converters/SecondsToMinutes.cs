using System;
using System.Globalization;
using System.Windows.Data;

namespace Fiasqo.PeepoRotChatbot.Domain.Converters {
public class SecondsToMinutes : IValueConverter {
	/// <inheritdoc />
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is int intValue) return intValue / 60D;
		return Binding.DoNothing;
	}

	/// <inheritdoc />
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
}