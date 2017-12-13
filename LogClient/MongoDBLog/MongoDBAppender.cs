using log4net.Appender;
using log4net.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogClient.MongoDBLog

{
    public class MongoDBAppender : BufferingAppenderSkeleton
    {

        /// <summary>
        /// MongoDB database connection in the format:
        /// mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
        /// See http://www.mongodb.org/display/DOCS/Connections
        /// If no database specified, default to "log4net"
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// The connectionString name to use in the connectionStrings section of your *.config file
        /// If not specified or connectionString name does not exist will use ConnectionString value
        /// </summary>
        public string CollectionName
        {
            get;
            set;
        }

        protected List<MongoDBAppenderParameter> m_parameters;

        public MongoDBAppender()
        {
            m_parameters = new List<MongoDBAppenderParameter>();

        }

        /// <summary>
        /// SendBuffer
        /// </summary>
        /// <param name="events"></param>
        protected override void SendBuffer(log4net.Core.LoggingEvent[] events)
        {
            Task.Factory.StartNew(() =>
            {
                if (events == null || events.Length == 0) return;
                var jsons = new List<BsonDocument>();
                foreach (LoggingEvent loggingEvent in events)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    foreach (MongoDBAppenderParameter p in this.m_parameters)
                        dic.Add(p.ParameterName, p.FormatValue(loggingEvent));
                    jsons.Add(new BsonDocument(dic));
                }
                var mc = GetCollection();
                mc.InsertBatch<BsonDocument>(jsons);
            });

#if DEBUG
            // Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(jsons));
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoDBAppenderParameter"></param>
        public void AddParameter(MongoDBAppenderParameter mongoDBAppenderParameter)
        {
            this.m_parameters.Add(mongoDBAppenderParameter);
        }


        private static MongoClientSettings _mongoClientSettings;

        private static string _databaseName;

        /// <summary>
        /// GetDatabase
        /// </summary>
        /// <returns></returns>
        private MongoDatabase GetDatabase()
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

        /// <summary>
        /// GetCollection
        /// </summary>
        /// <returns></returns>
        private MongoCollection GetCollection()
        {
            MongoDatabase database = this.GetDatabase();
            return database.GetCollection(string.Format("{0}_{1}", this.CollectionName, DateTime.Now.ToString("yyyyMMdd")));
        }
    }
}