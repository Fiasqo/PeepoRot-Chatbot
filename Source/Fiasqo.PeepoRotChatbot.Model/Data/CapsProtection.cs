using System;
using System.Text;

namespace Fiasqo.PeepoRotChatbot.Model.Data {
public sealed class CapsProtection : ModToolsProtection, IEquatable<CapsProtection> {
#region Fields

	private int _minCapsAmount = 10;

#endregion

#region Constructor

	public CapsProtection() : base("Please refrain from using caps !") { }

#endregion

#region Properties

	public int MinCapsAmount { get => _minCapsAmount; set => SetField(ref _minCapsAmount, value); }

#endregion

#region IDataErrorInfo

	/// <inheritdoc />
	public override string this[string columnName] => columnName switch {
		nameof(MinCapsAmount) => MinCapsAmount <= 0 ? "Min. Caps Amount Must Be Greater Than Zero." : string.Empty,
		_                     => base[columnName]
	};

	/// <inheritdoc />
	public override string Error {
		get {
			var sb = new StringBuilder(92);

			var error = this[nameof(MinCapsAmount)];
			if (error != string.Empty) sb.AppendLine(error);

			error = base.Error;
			if (error != string.Empty) sb.AppendLine(error);

			var sbBuilded = sb.ToString();

			return !string.IsNullOrWhiteSpace(sbBuilded)
					   ? sbBuilded.TrimEnd('\n', '\r')
					   : string.Empty;
		}
	}

#endregion

#region IEquatable

	/// <inheritdoc />
	// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
	public override int GetHashCode() => base.GetHashCode();

	/// <inheritdoc />
	public override bool Equals(object obj) => Equals(obj as CapsProtection);

	/// <inheritdoc />
	public bool Equals(CapsProtection other) {
		if (ReferenceEquals(other, null)) return false;
		if (ReferenceEquals(other, this)) return true;
		return base.Equals(other) && MinCapsAmount.Equals(other.MinCapsAmount);
	}

#endregion
}
}