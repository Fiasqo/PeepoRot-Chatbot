using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;
using Fiasqo.PeepoRotChatbot.Model.Data;
using Fiasqo.PeepoRotChatbot.Model.Data.Collections;

namespace Fiasqo.PeepoRotChatbot.Properties {
[CompilerGenerated()]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
internal sealed partial class Settings : ApplicationSettingsBase {
	private static Settings defaultInstance = ((Settings) (Synchronized(new Settings())));

	public static Settings Default { get { return defaultInstance; } }

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public Model.Data.Settings SettingsVM_Settings {
		get { return ((Model.Data.Settings) (this["SettingsVM_Settings"])); }
		set { this["SettingsVM_Settings"] = value; }
	}

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public TimerCollection TimersVM_Timers { get { return ((TimerCollection) (this["TimersVM_Timers"])); } set { this["TimersVM_Timers"] = value; } }

	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool TimersVM_TimersIsEnabled { get { return ((bool) (this["TimersVM_TimersIsEnabled"])); } set { this["TimersVM_TimersIsEnabled"] = value; } }

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public ChatCommandCollection CommandsVM_ChatCommands {
		get { return ((ChatCommandCollection) (this["CommandsVM_ChatCommands"])); }
		set { this["CommandsVM_ChatCommands"] = value; }
	}

	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool CommandsVM_ChatCommandsIsEnabled {
		get { return ((bool) (this["CommandsVM_ChatCommandsIsEnabled"])); }
		set { this["CommandsVM_ChatCommandsIsEnabled"] = value; }
	}

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public CapsProtection ModToolsVM_CapsProtection {
		get { return ((CapsProtection) (this["ModToolsVM_CapsProtection"])); }
		set { this["ModToolsVM_CapsProtection"] = value; }
	}

	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool ModToolsVM_CapsProtectionIsEnabled {
		get { return ((bool) (this["ModToolsVM_CapsProtectionIsEnabled"])); }
		set { this["ModToolsVM_CapsProtectionIsEnabled"] = value; }
	}

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public LinkProtection ModToolsVM_LinkProtection {
		get { return ((LinkProtection) (this["ModToolsVM_LinkProtection"])); }
		set { this["ModToolsVM_LinkProtection"] = value; }
	}

	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool ModToolsVM_LinkProtectionIsEnabled {
		get { return ((bool) (this["ModToolsVM_LinkProtectionIsEnabled"])); }
		set { this["ModToolsVM_LinkProtectionIsEnabled"] = value; }
	}

	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public WordProtection ModToolsVM_WordProtection {
		get { return ((WordProtection) (this["ModToolsVM_WordProtection"])); }
		set { this["ModToolsVM_WordProtection"] = value; }
	}

	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool ModToolsVM_WordProtectionIsEnabled {
		get { return ((bool) (this["ModToolsVM_WordProtectionIsEnabled"])); }
		set { this["ModToolsVM_WordProtectionIsEnabled"] = value; }
	}
	
	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public TitleAndGameSettings DashboardVM_TitleAndGame {
		get { return ((TitleAndGameSettings) (this["DashboardVM_TitleAndGame"])); }
		set { this["DashboardVM_TitleAndGame"] = value; }
	}
	
	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public FollowerNotifierSettings DashboardVM_FollowerNotifier {
		get { return ((FollowerNotifierSettings) (this["DashboardVM_FollowerNotifier"])); }
		set { this["DashboardVM_FollowerNotifier"] = value; }
	}
	
	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public NewSubscriberNotifierSettings DashboardVM_NewSubscriberNotifier {
		get { return ((NewSubscriberNotifierSettings) (this["DashboardVM_NewSubscriberNotifier"])); }
		set { this["DashboardVM_NewSubscriberNotifier"] = value; }
	}
	
	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public ReSubscriberNotifierSettings DashboardVM_ReSubscriberNotifier {
		get { return ((ReSubscriberNotifierSettings) (this["DashboardVM_ReSubscriberNotifier"])); }
		set { this["DashboardVM_ReSubscriberNotifier"] = value; }
	}
	
	[UserScopedSetting()]
	[SettingsSerializeAs(SettingsSerializeAs.Binary)]
	public GiftSubscriberNotifierSettings DashboardVM_GiftSubscriberNotifier {
		get { return ((GiftSubscriberNotifierSettings) (this["DashboardVM_GiftSubscriberNotifier"])); }
		set { this["DashboardVM_GiftSubscriberNotifier"] = value; }
	}
	
	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool DashboardVM_FollowerNotifierIsEnabled {
		get { return ((bool) (this["DashboardVM_FollowerNotifierIsEnabled"])); }
		set { this["DashboardVM_FollowerNotifierIsEnabled"] = value; }
	}
	
	[UserScopedSetting()]
	[DefaultSettingValue("False")]
	public bool DashboardVM_SubscriberNotifierIsEnabled {
		get { return ((bool) (this["DashboardVM_SubscriberNotifierIsEnabled"])); }
		set { this["DashboardVM_SubscriberNotifierIsEnabled"] = value; }
	}
}
}