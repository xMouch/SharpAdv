using System;
using Artemis;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SharpAdv
{
	[BsonIgnoreExtraElements]
	public class Scene
	{
		public string Name{ get; set; }	
		public string LevelName{ get; set; }
		public string Description{ get; set; }
		public uint Width{ get; set; }
		public uint Height{ get; set; }

		public Scene ()
		{

		}

		public Scene(string Name, string Description, uint Width, uint Height)
		{
			this.Name = Name;
			this.Description = Description;
			this.Width = Width;
			this.Height = Height;
		}
	}
}

