using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SharpAdv
{
	public class DatabaseManager
	{
		private string connectionString = "mongodb://localhost";
		private MongoClient client;
		public MongoServer server;
		public MongoDatabase database;

		public DatabaseManager ()
		{
		}

		public void Init()
		{
			client = new MongoClient (connectionString);
			server = client.GetServer ();
			database = server.GetDatabase ("sharpA");
		}
	}
}

