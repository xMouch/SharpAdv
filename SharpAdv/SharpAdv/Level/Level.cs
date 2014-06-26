﻿using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpAdv
{
	public class Level
	{
		[BsonId]
		public string Name{ get; set; }
		public string CurrentScene{ get; set; }

		public Level ()
		{

		}
	}
}

