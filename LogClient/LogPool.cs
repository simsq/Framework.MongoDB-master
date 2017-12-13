using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using LogClient.MongoDBLog;

namespace LogClient
{
    public class LogPool
    {
        private static readonly ConcurrentQueue<BsonDocument> LogQueue = new ConcurrentQueue<BsonDocument>();
        private const int MaxCountPerTime = 50;
        private const int SleepSeconds = 10000;

        public static void AddRange(List<BsonDocument> bsonDocuments)
        {
            if (bsonDocuments.Count() <= 0)
            {
                return;
            }
            bsonDocuments.ForEach(p => LogQueue.Enqueue(p));
        }

        public static void Add(BsonDocument bsonDocument)
        {
            LogQueue.Enqueue(bsonDocument);
        }

        public static void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        try
                        {
                            //Thread.Sleep(SleepSeconds);
                            if (LogQueue.Count > 0)
                            {
                                var mc = MongoDBPoolAppender.GetCollection();
                                var times = Partition();
                                for (int i = 0; i < times; i++)
                                {
                                    int num = MaxCountPerTime;
                                    BsonDocument bsonDocument;
                                    var list = new List<BsonDocument>();
                                    while (num > 0 && LogQueue.TryDequeue(out bsonDocument))
                                    {
                                        list.Add(bsonDocument);
                                        num--;
                                    }
                                    mc.InsertBatch<BsonDocument>(list);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {

                            throw;
                        }
                        //2017-05-20 为了方便调试才注释的这个
                       
                    }
                    catch {
                    }
                }
            });
        }

        private static int Partition()
        {
            var count = LogQueue.Count;
            var times = count / MaxCountPerTime;
            if (count % MaxCountPerTime != 0)
            {
                times++;
            }
            return times;
        }
    }
}
