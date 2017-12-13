using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.IO;
using System.Linq;

namespace LogClient
{
    /// <summary>
    /// 日志消息
    /// </summary>
    internal class LogMessage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public LogMessage() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="title"></param>
        public LogMessage(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// APPID
        /// </summary>
        public string APPID { set; get; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// Index
        /// </summary>
        public string Index { set; get; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { set; get; }

        /// <summary>
        /// ClientMessage
        /// </summary>
        public string ClientMessage { set; get; }

        /// <summary>
        /// ServerIP
        /// </summary>
        public string ServerIP { set; get; }

        /// <summary>
        /// 日志所在方法
        /// </summary>
        public string Method { set; get; }
    }

    public class APPIDPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.APPID);
            else
                writer.Write(EnvironmentConfig.AppId);
        }
    }

    public class TitlePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Title);
            else
                writer.Write(string.Empty);
        }
    }

    public class IndexPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Index);
            else
                writer.Write(AppDomain.CurrentDomain.BaseDirectory);
        }
    }

    public class GroupPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Group);
        }
    }

    public class ClientMessagePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ClientMessage);
            else
                writer.Write(loggingEvent.MessageObject);
        }
    }

    public class ServerIPPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.ServerIP);
            else
                writer.Write(EnvironmentConfig.CurrLocalIp);
        }
    }

    public class MethodPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogMessage logMessage = loggingEvent.MessageObject as LogMessage;
            if (logMessage != null)
                writer.Write(logMessage.Method);
            else
            {
                var stackFrame = loggingEvent.LocationInformation.StackFrames.Count() > 1 ? loggingEvent.LocationInformation.StackFrames[1]
                                                                                          : loggingEvent.LocationInformation.StackFrames[0];
                writer.Write(string.Format("{0}.{1}", stackFrame.ClassName, stackFrame.Method));
            }
        }
    }
}
