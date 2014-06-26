using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class EventProcessorContainer
	{
		public Dictionary<EventType, List<EventProcessor>> EventProcessors{ get; set; }

		public EventProcessorContainer ()
		{
		}

		public EventProcessorContainer(Dictionary<EventType, List<EventProcessor>> proceccors)
		{
			this.EventProcessors = proceccors;
		}
	}
}

