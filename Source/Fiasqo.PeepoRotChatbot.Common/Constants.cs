﻿using System;
using System.Linq;
using Fiasqo.PeepoRotChatbot.Common.Utilities;

namespace Fiasqo.PeepoRotChatbot.Common {
public static class Constants {
	public const string NotAvailableMessage = "<N/A>";

	public const double MinMainWindowWidth = 1024D;
	public const double MinMainWindowHeight = 613D;

	public const int MaxChatMessageLenght = 500;
	public const int MaxStreamTitleLenght = 140;
	public const int MinUserNameLenght = 4;
	public const int MaxUserNameLenght = 25;
	public const int MaxChatCommandLenght = 50;

	public const int MinGapBetweenEventsRepliesInSeconds = 1;
	public const int MaxGapBetweenEventsRepliesInSeconds = 120;

	public const double MinGapBetweenEventsRepliesInSecondsAsDouble = MinGapBetweenEventsRepliesInSeconds;
	public const double MaxGapBetweenEventsRepliesInSecondsAsDouble = MaxGapBetweenEventsRepliesInSeconds;

	public static readonly AssemblyInfo ApplicationAssemblyInfo
		= new(AppDomain.CurrentDomain.GetAssemblies().Single(assembly => assembly.FullName!.Contains("Fiasqo.PeepoRotChatbot,")));
}
}