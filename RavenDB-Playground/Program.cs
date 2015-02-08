using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Json.Linq;

namespace RavenDB_Playground
{
    public class EventsTimeResults
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

             
                DynamicAggregation(store);

                //TranformerResults(store);
            }
        }

        private static void TranformerResults(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                

                var query = session.Query<EventsTime>("EventsTime")
                    
                    //.ProjectFromIndexFieldsInto<EventsTime>()
                    .TransformWith<RavenJObject>("EventsGroupByDay");

                var results = query.ToList();
            }
        }

        public static void DynamicAggregation(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var query = session.Query<EventsTimeResults>("EventsTime")
                    .ProjectFromIndexFieldsInto<EventsTimeResults>()
                    .Where(x => x.EventName != "applicationprobed")
                    .AggregateBy("Grouping", "EventsByNameAndDate")
                    .CountOn(x => x.WhenDate);                  

                var results = query.ToList();
            }
        }
    }
}
