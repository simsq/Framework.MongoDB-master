using log4net.Appender;
using log4net.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;



namespace LogClient.MongoDBLog

{
    public class MongoDBPoolAppender : AppenderSkeleton
    {
        private static MongoClientSettings _mongoClientSettings;
        private static string _databaseName;
        protected List<MongoDBAppenderParameter> m_parameters;

        public static string ConnectionString { get; set; }
        public static string CollectionName { get; set; }

        public MongoDBPoolAppender()
        {
            m_parameters = new List<MongoDBAppenderParameter>();
            LogPool.Start();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (MongoDBAppenderParameter p in this.m_parameters)
            {
                dic.Add(p.ParameterName, p.FormatValue(loggingEvent));
            }
            var bsonDocument = new BsonDocument(dic);
            LogPool.Add(bsonDocument);
        }

        public void AddParameter(MongoDBAppenderParameter mongoDBAppenderParameter)
        {
            this.m_parameters.Add(mongoDBAppenderParameter);
        }

        private static MongoDatabase GetDatabase()
        {
            if (_mongoClientSettings == null)
            {
                MongoUrl mongoUrl = MongoUrl.Create(ConnectionString);
                _mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
                _mongoClientSettings.SocketTimeout = new TimeSpan(0, 0, 0, 2);
                _mongoClientSettings.ConnectTimeout = new TimeSpan(0, 0, 0, 1);
                _mongoClientSettings.ServerSelectionTimeout = new TimeSpan(0, 0, 0, 1);
                _databaseName = mongoUrl.DatabaseName;
            }
            MongoClient client = new MongoClient(_mongoClientSettings);
            MongoServer mongoServer = client.GetServer();
            MongoDatabase database = mongoServer.GetDatabase(_databaseName);
            return database;
        }

        internal static MongoCollection GetCollection()
        {
            MongoDatabase database = null;
            try
            {
                database = GetDatabase();
            }
            catch (Exception ex)
            {

                throw;
            }
            return database.GetCollection(string.Format("{0}_{1}", CollectionName, DateTime.Now.ToString("yyyyMMdd")));
        }
    }
}
