using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace RavenDB_Playground
{
    public class EventsTime : AbstractIndexCreationTask
    {
        public override string IndexName
        {
            get
            {
                return "EventsTime";
            }
        }
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition
            {
                Map = @"from @event in docs.Events
select new {
    EventName = @event[""@metadata""][""Entity-Type""], 
	When = @event.When,
	WhenDate = @event.When.Date.ToString(""yyyy-MM-dd""),
	Grouping = String.Concat(@event.When.Date.ToString(""yyyy-MM-dd""), '|', @event[""@metadata""][""Entity-Type""])
}",
                Stores =  {
                    {
                        "When",
                        FieldStorage.Yes
                    }
                }
            };
        }
    }
}
