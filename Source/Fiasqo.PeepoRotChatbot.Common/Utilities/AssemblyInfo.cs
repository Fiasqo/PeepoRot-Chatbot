using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace Fiasqo.PeepoRotChatbot.Common.Utilities {
public class AssemblyInfo {
#region Constructors

	public AssemblyInfo(Assembly assembly = null) {
		assembly ??= Assembly.GetExecutingAssembly();

		T GetAttributeOrNull<T>() where T : Attribute => TryGetAssemblyAttribute(assembly, out T attribute) ? attribute : null;

		Title = GetAttributeOrNull<AssemblyTitleAttribute>()?.Title ?? Constants.NotAvailableMessage;
		Description = GetAttributeOrNull<AssemblyDescriptionAttribute>()?.Description ?? Constants.NotAvailableMessage;
		Company = GetAttributeOrNull<AssemblyCompanyAttribute>()?.Company ?? Constants.NotAvailableMessage;
		Product = GetAttributeOrNull<AssemblyProductAttribute>()?.Product ?? Constants.NotAvailableMessage;
		Copyright = GetAttributeOrNull<AssemblyCopyrightAttribute>()?.Copyright ?? Constants.NotAvailableMessage;
		Trademark = GetAttributeOrNull<AssemblyTrademarkAttribute>()?.Trademark ?? Constants.NotAvailableMessage;
		FileVersion = GetAttributeOrNull<AssemblyFileVersionAttribute>()?.Version ?? Constants.NotAvailableMessage;
		Guid = GetAttributeOrNull<GuidAttribute>()?.Value ?? Constants.NotAvailableMessage;
		NeutralLanguage = GetAttributeOrNull<NeutralResourcesLanguageAttribute>()?.CultureName ?? Constants.NotAvailableMessage;
		ComVisibility = GetAttributeOrNull<ComVisibleAttribute>()?.Value.ToString() ?? Constants.NotAvailableMessage;
		AppVersion = assembly.GetName().Version?.ToString();
		if (string.IsNullOrEmpty(AppVersion) || AppVersion == "0.0.0.0") AppVersion = Constants.NotAvailableMessage;
	}

#endregion

#region Public Methods

	public static bool TryGetAssemblyAttribute<T>(Assembly assembly, out T attribute) where T : Attribute {
		object[] attributes = assembly.GetCustomAttributes(typeof(T), true);
		var result = attributes.Length == 0;
		attribute = result ? null : (T) attributes[0];
		return !result;
	}

#endregion

#region Properties

	public string Title { get; }
	public string Description { get; }
	public string Company { get; }
	public string Product { get; }
	public string Copyright { get; }
	public string Trademark { get; }
	public string AppVersion { get; }
	public string FileVersion { get; }
	public string Guid { get; }
	public string NeutralLanguage { get; }
	public string ComVisibility { get; }

#endregion
}
}