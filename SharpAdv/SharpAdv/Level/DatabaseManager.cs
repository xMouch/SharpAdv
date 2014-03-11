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
			if (server.DatabaseExists ("sharpA"))
				database = server.GetDatabase ("sharpA");
			else
				database = InitDatabase ();
		}

		private MongoDatabase InitDatabase()
		{
			MongoDatabase db = server.GetDatabase ("sharpA");//CREATE DATABASE, CHECK IF POSSIBLE WITH GETDATABASE

			//Init Collections
			db.GetCollection ("levels");

			return db;
		}
	}
}

