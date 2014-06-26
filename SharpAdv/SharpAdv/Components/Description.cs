using System;
using MongoDB.Bson.Serialization.Attributes;
using Artemis.Interface;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class Description : IComponent
	{
		public string Desc{get;set;}

		public Description ()
		{
		}
	}
}

