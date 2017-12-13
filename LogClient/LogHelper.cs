using log4net;
using System;
using System.Diagnostics;
namespace LogClient
{
    public static class LogHelper
    {
        private static ILog _log;

        static LogHelper()
        {
            string logName = LogBase.CreateMongoDBPoolAppender();
            //logName = LogBase.CreateMongoDBAppender(); 
            _log = log4net.LogManager.GetLogger(logName);
        }

        /// <summary>
        /// log
        /// </summary>
        private static ILog log { get { return _log; } }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="ex"></param>
        public static void Info(string title, string message, string index = null, Exception ex = null)
        {
            //Task.Factory.StartNew(() =>
            //{
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(message)) return;
            var method = new StackFrame(1).GetMethod();
            LogMessage logMessage = new LogMessage
            {
                Title = title,
                Index = index,
                ClientMessage = message,
                Method = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name),
                APPID = EnvironmentConfig.AppId,
                ServerIP = EnvironmentConfig.CurrLocalIp,
                Group = EnvironmentConfig.LogGroup
            };
            log.Info(logMessage, ex);
            //});
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="ex"></param>
        public static void Warn(string title, string message, string index = null, Exception ex = null)
        {
            //Task.Factory.StartNew(() =>
            //{
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(message)) return;
            LogMessage logMessage = new LogMessage
            {
                Title = title,
                Index = index,
                Method = GetCurrentMethodFullName(2),
                ClientMessage = message,
                APPID = EnvironmentConfig.AppId,
                ServerIP = EnvironmentConfig.CurrLocalIp,
                Group = EnvironmentConfig.LogGroup
            };
            log.Warn(logMessage, ex);
            //});
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="index"></param>
        /// <param name="ex"></param>
        public static void Error(string title, string message, string index = null, Exception ex = null)
        {

            //Task.Factory.StartNew(() =>
            //{
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(message))
                return;
            LogMessage logMessage = new LogMessage
            {
                Title = title,
                Method = GetCurrentMethodFullName(2),
                Index = index,
                ClientMessage = message,
                APPID = EnvironmentConfig.AppId,
                ServerIP = EnvironmentConfig.CurrLocalIp,
                Group = EnvironmentConfig.LogGroup
            };
            log.Error(logMessage, ex);
            //});
        }

        /// <summary>
        /// 获取当前方法的全名
        /// </summary>
        /// <param name="back">默认值为1,可以往上传,调用方法的上级或者在上级,</param>
        /// <returns></returns>
        public static string GetCurrentMethodFullName(int back = 1)
        {
            if (back <= 0) back = 1;
            var method = new StackFrame(back).GetMethod();
            return string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
        }
    }
}
