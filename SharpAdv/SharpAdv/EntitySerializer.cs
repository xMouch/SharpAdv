using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using Artemis;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SharpAdv
{
	public class EntitySerializer : IBsonSerializer
	{
		private EntityWorld world;

		public EntitySerializer (EntityWorld world)
		{
			this.world = world;
		}

		public object Deserialize (BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			return Deserialize (bsonReader, nominalType, nominalType, options);
		}

		public object Deserialize (BsonReader bsonReader, Type nominalType, Type realType, IBsonSerializationOptions options)
		{
			if (realType.Equals (typeof(Entity))) {
				Entity e = world.CreateEntity ();
				SerializationInfo si = new SerializationInfo ();
				bsonReader.ReadStartDocument ();
				bsonReader.ReadObjectId ("_id");
				si.Name = bsonReader.ReadString ("Name");
				si.SceneName = bsonReader.ReadString ("SceneName");
				si.LevelName = bsonReader.ReadString ("LevelName");
				bsonReader.ReadEndDocument();
				e.AddComponent (si);
				return e;
			} else
				throw new BsonSerializationException ("value has to be an entity");
		}
			
		public void Serialize (BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			if (value is Entity) 
			{
				Entity e = (Entity)value;
				if (e.HasComponent<SerializationInfo> ()) {
					//Real Serialization
					SerializationInfo si = e.GetComponent<SerializationInfo> ();
					bsonWriter.WriteStartDocument ();
					bsonWriter.WriteObjectId ("_id", ObjectId.GenerateNewId ());
					bsonWriter.WriteString ("Name", si.Name);
					bsonWriter.WriteString ("SceneName", si.SceneName);
					bsonWriter.WriteString ("LevelName", si.LevelName);
					bsonWriter.WriteEndDocument ();

				} else
					throw new BsonSerializationException ("The entity must have the SerializationInfo component");
			} 
			else
				throw new BsonSerializationException ("value has to be an Entity");
		}

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			return null;
		}
	}
}

