#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fiasqo.PeepoRotChatbot.Common.Utilities;

namespace Fiasqo.PeepoRotChatbot.Domain {
[Serializable]
public abstract class PropertyChangedNotifier : INotifyPropertyChanged {
	[field: NonSerialized]
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(propertyName);
		Logger.Log.Debug($"Set {GetType().Name}.{propertyName}. {nameof(value)}: {value?.ToString()}");
		return true;
	}

	protected bool TrySetField<T>(ref T field, T value, Func<T, T, bool> predicate, [CallerMemberName] string? propertyName = null) {
		var predicateResult = predicate.Invoke(field, value);
		if (predicateResult) return SetField(ref field, value, propertyName);
		Logger.Log.Warn($"Cant set {GetType().FullName}.{propertyName}. {nameof(value)}: {value?.ToString()}, {predicate} is {predicateResult}");
		return false;
	}
}
}