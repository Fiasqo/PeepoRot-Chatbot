using System;
using System.Linq;
using Fiasqo.PeepoRotChatbot.Common.Utilities;

namespace Fiasqo.PeepoRotChatbot.Common {
public static class Constants {
	public const string NotAvailableMessage = "<N/A>";

	public const double MinMainWindowWidth = 1024.0D;
	public const double MinMainWindowHeight = 576.0D;

	public const int MaxChatMessageLenght = 500;
	public const int MaxStreamTitleLenght = 140;
	public const int MaxUserNameLenght = 25;
	public const int MaxChatCommandLenght = 50;

	public const int TwoYears_Seconds = 63070000;
	public const int OneMonth_Seconds = 2592000;

	public const int MinGapBetweenTimerReplies_Seconds = 600;
	public const int MinGapBetweenEventsReplies_Seconds = 1;
	public const int MaxGapBetweenEventsReplies_Seconds = 120;

	public const double MinGapBetweenTimerReplies_Seconds_AsDouble = MinGapBetweenTimerReplies_Seconds;
	public const double MinGapBetweenEventsReplies_Seconds_AsDouble = MinGapBetweenEventsReplies_Seconds;
	public const double MaxGapBetweenEventsReplies_Seconds_AsDouble = MaxGapBetweenEventsReplies_Seconds;

	public static readonly AssemblyInfo ApplicationAssemblyInfo
		= new(AppDomain.CurrentDomain.GetAssemblies().Single(assembly => assembly.FullName!.Contains("Fiasqo.PeepoRotChatbot,")));
}
}