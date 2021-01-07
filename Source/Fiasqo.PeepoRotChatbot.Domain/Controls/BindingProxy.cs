using System.Windows;

namespace Fiasqo.PeepoRotChatbot.Domain.Controls {
public class BindingProxy : Freezable {
	public static readonly DependencyProperty DataProperty =
		DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

	public object Data { get => GetValue(DataProperty); set => SetValue(DataProperty, value); }

	/// <inheritdoc />
	protected override Freezable CreateInstanceCore() => new BindingProxy();
}
}