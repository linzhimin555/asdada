using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ConsoleApp1
{
    class Program
    {
        public static List<RongYun_Mongo> models = new List<RongYun_Mongo>();
        static void Main(string[] args)
        {
            //var dd = new MongoClient("mongodb://172.16.10.236:27017");
            //var d = dd.GetServer();
            //var daa = d.GetDatabase("qhdb");
            //var ds = daa.GetCollection<RongYun_Mongo>("RongYun_Mongo");
            // {UserId = 1288, os = "1"};

            for (var i = 0; i < 7; i++)
            {
                var model = new RongYun_Mongo
                {
                    UserId =1288,
                    UserName="tzgfmy",
                    os="os"+i,
                    token = "token"+i,
                    status = i,
                };
                models.Add(model);
                Thread thread = new Thread(ThreadMethod1);
                thread.Start(i);
            }

            var s = "";
        }

        public static void ThreadMethod1(object i) //方法内可以有参数，也可以没有参数
        {
            try
            {
                var dd = MongoServer.Create("mongodb://172.16.10.236:27017");
                var daa = dd.GetDatabase("qhdb");

                var j = Convert.ToInt16(i);
                var ds = daa.GetCollection<RongYun_Mongo>("RongYun_Mongo");
                var query = Query.And(Query.EQ("UserId", models[j].UserId));

                var update = Update.Set("status", models[j].status).Set("token", models[j].token)
                    .Set("os", models[j].os).Set("date", DateTime.Now);

                var sss = ds.FindAndModify(query: query, sortBy: SortBy.Ascending("time"), update: update,
                    returnNew: true,
                    upsert: true);

                Console.WriteLine("{0}开始执行。", Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}开始执行。{1}", Thread.CurrentThread.Name,ex.ToString());

            }
        }

        public class RongYun_Mongo
        {
            public ObjectId _id;

            public string UserName { get; set; }

            public long UserId { get; set; }

            public int status { get; set; }

            public string os { get; set; }

            public string time { get; set; }

            public string token { get; set; }

        }
    }
}
