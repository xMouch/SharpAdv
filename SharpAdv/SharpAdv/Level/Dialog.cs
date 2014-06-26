using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class Dialog
	{
		public string Name{ get; set; }
		public string LevelName{ get; set; }
		public string ScriptContent{ get; set; }

		public Dialog ()
		{
		}
	}
}

