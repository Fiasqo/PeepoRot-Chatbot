using System;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Fiasqo.PeepoRotChatbot.Common.Utilities {
public static class Logger {
	public static ILog Log { get; } = Init();

	private static ILog Init() {
		_logFullPath = Assembly.GetExecutingAssembly().Location.Replace($"{Assembly.GetExecutingAssembly().GetName().Name}.dll",
																		@$"{_logDirectory}\{_logFilePrefix}_log4net.{_logFileExtension}");

		var log4netConfig = new XmlDocument();
		log4netConfig.LoadXml(
			@$"<?xml version=""1.0"" encoding=""utf-8""?>
				<configuration>
					<log4net>
						<appender name=""Console"" type=""log4net.Appender.ColoredConsoleAppender"">
							<mapping>
								<level value=""FATAL"" />
								<foreColor value=""Purple, HighIntensity"" />
							</mapping>
							<mapping>
								<level value=""ERROR"" />
								<foreColor value=""Red"" />
							</mapping>
							<mapping>
								<level value=""WARN"" />
								<foreColor value=""Yellow"" />
							</mapping>
							<mapping>
								<level value=""DEBUG"" />
								<foreColor value=""Blue"" />
							</mapping>
							<mapping>
								<level value=""INFO"" />
								<foreColor value=""Blue, HighIntensity"" />
							</mapping>
							<layout type=""log4net.Layout.PatternLayout"">
								<conversionPattern value=""%timestamp; %-5level; %thread; [%file:%line]; [message: %message]; [stacktrace: %stacktrace{"{1}"}];%newline""/>
							</layout>
						</appender>
						<appender name=""RollingFile"" type=""log4net.Appender.RollingFileAppender""> 
							<file value=""{_logDirectory}\""/>
							<datePattern value=""'{_logFilePrefix}'-dd.MM.yy-HH.mm.ms'.{_logFileExtension}'"" />
							<staticLogFileName value=""false"" />
							<appendToFile value=""true""/>
							<maxSizeRollBackups value=""5""/>
							<rollingStyle value=""Composite"" />
							<maximumFileSize value=""30MB""/>
							<lockingModel type=""log4net.Appender.FileAppender+MinimalLock""/>
							<layout type=""log4net.Layout.PatternLayout"">
								<conversionPattern value=""%timestamp; %-5level; %thread; [%file:%line]; [message: %message]; [stacktrace: %stacktrace{"{10}"}];%newline""/>
							</layout>
						</appender>
						<logger name=""{_loggerName}"">
							<appender-ref ref=""Console"" />
							<appender-ref ref=""RollingFile""/>
						</logger>
					</log4net>
				</configuration>");

		ILoggerRepository repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
		XmlConfigurator.Configure(repository, (XmlElement) log4netConfig.GetElementsByTagName("log4net")[0]);

		DeleteLogFiles(DateTime.Now.AddDays(-3));

		return LogManager.GetLogger(_loggerName);
	}

	public static void DeleteLogFiles(DateTime startDateOfActualFiles) {
		var dirInfo = new DirectoryInfo(_logFullPath);
		if (!dirInfo.Exists) return;

		FileInfo[] fileInfos = dirInfo.GetFiles($"{_logFilePrefix}*.{_logFileExtension}");
		if (fileInfos.Length == 0) return;

		foreach (FileInfo info in fileInfos)
			if (info.CreationTime < startDateOfActualFiles)
				info.Delete();
	}

#region Config

	private const string _logFilePrefix = "fprc";
	private const string _logFileExtension = "log";
	private const string _logDirectory = "logs";
	private const string _loggerName = "DefLogger";
	private static string _logFullPath;

#endregion
}
}