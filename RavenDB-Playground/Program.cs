using System;
using Raven.Client;
using Raven.Client.Document;

namespace RavenDB_Playground
{
    public class EventsTime
    {
        public DateTime When { get; set; }

        public DateTime WhenDate { get; set; }

        public string EventName { get; set; }

        public int Count { get; set; }
    }

    public class Program
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
