using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace RavenDB_Playground
{
    public class EventsTime
    {
        public DateTime When { get; set; }

        public DateTime WhenDate { get; set; }

        public string EventName { get; set; }

        public int Count { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var store = new DocumentStore())
            {
                store.Url = "http://live-test-ravendb-laj3iyh5zf-olympus-zetes-com.cloudapp.net";
                store.DefaultDatabase = "BPost-20150205111400";

                store.Initialize();

                using (var session = store.OpenSession())
                {
                    var query = session.Query<EventsTime>("EventsTime")
                        .AggregateBy("WhenDate", "Period")
                        .CountOn(x => x.When)
                        .AndAggregateOn("EventName")
                        .CountOn(x => x.When);

                    var results = query.ToList();
                }

            }
        }
    }
}
