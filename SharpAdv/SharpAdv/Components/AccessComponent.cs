using System;
using Artemis.Interface;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class AccessComponent : IComponent
	{
		public string Name{ get; set;}
		public bool Visible{ get; set;}

		public AccessComponent ()
		{
		}
	}
}

