using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using LogClient.MongoDBLog;
using System.Configuration;
using System;
using Configuration;

namespace LogClient
{
    /// <summary>
    /// 日志操作
    /// </summary>
    internal static class LogBase
    {
        //private static readonly ILog log = log4net.LogManager.GetLogger("AdoNetAppender");         
        //static string LogDBString = ConfigurationManager.ConnectionStrings["LogDBConnectionString"] == null ? "mongodb://10.10.12.165:27017" : ConfigurationManager.ConnectionStrings["LogDBConnectionString"].ToString();
        //static string LogDBString = ConfigurationManager.ConnectionStrings["LogDB_Write"] == null ? "mongodb://10.10.12.165:27017" : ConfigurationManager.ConnectionStrings["LogDB_Write"].ToString();
        static string LogDBString = "mongodb://10.10.22.231:27017/testDB"; 
        public static string CreateMongoDBPoolAppender()
        {
            var hier = LogManager.GetRepository();
            if (hier == null) return string.Empty;
            MongoDBPoolAppender appender = new MongoDBPoolAppender();
            appender.Name = string.Format("mongoDBAppender_{0}", DateTime.Now.Ticks);
            MongoDBPoolAppender.ConnectionString = LogDBString;
            MongoDBPoolAppender.CollectionName = "logs";
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "level", Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "appid", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%appid")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "serverip", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%serverip")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "method", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%method")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "title", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%title")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "index", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%index")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "group", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%group")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "message", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%clientMessage")) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "exception", Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
            appender.AddParameter(new MongoDBAppenderParameter { ParameterName = "logdate", Layout = new RawTimeStampLayout() });
            BasicConfigurator.Configure(appender);
            appender.ActivateOptions();
            return appender.Name;
        }

        /// <summary>
        /// 创建MongoDBAppender
        /// </summary>
        /// <returns>返回logName</returns>
        public static string CreateMongoDBAppender()
        {
            var hier = LogManager.GetRepository();
            if (hier == null) return string.Empty;
            MongoDBAppender mongoDBAppender = new MongoDBAppender();
            mongoDBAppender.Name = string.Format("mongoDBAppender_{0}", DateTime.Now.Ticks);
            mongoDBAppender.ConnectionString = LogDBString;
            mongoDBAppender.CollectionName = "logs";
            mongoDBAppender.BufferSize = 2;
#if DEBUG
            mongoDBAppender.BufferSize = 1;
#endif
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "level", Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "appid", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%appid")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "serverip", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%serverip")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "method", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%method")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "title", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%title")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "index", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%index")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "group", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%group")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "message", Layout = new Layout2RawLayoutAdapter(new CustomLayout("%clientMessage")) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "exception", Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
            mongoDBAppender.AddParameter(new MongoDBAppenderParameter { ParameterName = "logdate", Layout = new RawTimeStampLayout() });
            BasicConfigurator.Configure(mongoDBAppender);
            mongoDBAppender.ActivateOptions();
            return mongoDBAppender.Name;
        }
        /// <summary>
        /// CreateAdoNetAppender
        /// </summary>
        /// <returns>返回logName</returns>
        public static string CreateAdoNetAppender()
        {
            var hier = LogManager.GetRepository();
            if (hier == null) return string.Empty;
            AdoNetAppender adoAppender = new AdoNetAppender();
            adoAppender.Name = string.Format("AdoNetAppender_{0}", DateTime.Now.Ticks);
            adoAppender.CommandType = System.Data.CommandType.Text;
            adoAppender.BufferSize = 1;
            adoAppender.ConnectionType = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data";
            adoAppender.ConnectionString = LogDBString; //"Server=192.168.1.30;Database=logdb;Uid=dev;Pwd=server1@taolx.com;Port=3306;";
            adoAppender.CommandText = @"INSERT INTO `log`(`Level`,`APPID`,`ServerIP`,`Title`,`Index`,`Message`,`Exception`,`LogDate`) 
                                                           values(@level,@appid,@serverip,@title,@index, @message,@exception, @logdate)";
            //adoAppender.CommandText = @"INSERT INTO `log`(`Level`,`Title`,`Index`,`Message`,`Exception`,`LogDate`) 
            //                                           values(@level,@title,@index, @message,@exception, @logdate)"; 
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@level", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@appid", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%appid")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@serverip", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%serverip")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@method", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%method")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@title", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%title")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@index", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%index")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@message", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new CustomLayout("%clientMessage")) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
            adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logdate", DbType = System.Data.DbType.DateTime, Layout = new RawTimeStampLayout() });
            adoAppender.ActivateOptions();
            BasicConfigurator.Configure(adoAppender);
            return adoAppender.Name;
        }

    }
}
