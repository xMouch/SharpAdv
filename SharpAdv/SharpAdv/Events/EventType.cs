using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SharpAdv
{
	public class EventType
	{
		internal static int nextId;

		[BsonId]
		public int Id{ get; set; }

		/**
		 * Nicht verwenden, der parameterlose Konstruktor ist nur für den MongoDB Serializer
		 * */
		public EventType()
		{
		}

		public override bool Equals(Object o)
		{
			if (o is EventType)
				return ((EventType)o).Id == Id;
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}

