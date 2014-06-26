using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpAdv
{
	public class Cancel : EventProcessor
	{

		public Cancel ()
		{
		}

		public override void Event(Event e)
		{
			e.Cancel = true;
		}
	}
}

