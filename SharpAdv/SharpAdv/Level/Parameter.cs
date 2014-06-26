using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class Parameter
	{
		public string Parent{ get; set; }
		public string Key{ get; set; }
		public string Value{ get; set; }

		public Parameter ()
		{
		}
	}
}

