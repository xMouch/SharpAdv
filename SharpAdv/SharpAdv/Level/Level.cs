using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SharpAdv
{
	public class Level
	{
		//TODO
		public List<ObjectId> rooms{get;protected set;}
		public string level{ get; protected set; }
		public ObjectId startRoom{ get; protected set; }

		public Level ()
		{

		}
	}
}

