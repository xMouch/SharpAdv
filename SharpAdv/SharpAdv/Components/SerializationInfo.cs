using System;
using Artemis.Interface;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpAdv
{
	public class SerializationInfo : IComponent
	{
		public string Name{get;set;}
		public string SceneName{ get; set;}
		public string LevelName{ get; set;}

		public SerializationInfo ()
		{
		}

		public SerializationInfo(string Name, string SceneName, string LevelName)
		{
			this.Name = Name;
			this.SceneName = SceneName;
			this.LevelName = LevelName;
		}
	}
}

