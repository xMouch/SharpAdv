using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Artemis;
using Artemis.Interface;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver.Builders;
using Artemis.Utils;
using Artemis.Manager;
using SharpAdv;
using System.Collections.Generic;

namespace SharpAdv
{
	public class DatabaseManager
	{
		private string connectionString = "mongodb://localhost";
		private MongoClient client;
		public MongoServer server;
		public MongoDatabase database;

		//Collections
		public MongoCollection levels{ get; private set; }
		public MongoCollection levelparameters{ get; private set; }
		public MongoCollection scenes{ get; private set; }
		public MongoCollection sceneparameters{ get; private set; }
		public MongoCollection objects{ get; private set; }
		public MongoCollection components{ get; private set; }
		public MongoCollection eventprocessors{ get; private set; }
		public MongoCollection dialogs{ get; private set; }
		public MongoCollection audiofiles{ get; private set; }

		private EntitySerializer entitySerializer;

		public DatabaseManager (EntityWorld world)
		{
			entitySerializer = new EntitySerializer (world);
		}

		public void Init()
		{
			client = new MongoClient (connectionString);
			server = client.GetServer ();
			database = InitDatabase ();

			//Register Classes to serialize
			BsonClassMap.RegisterClassMap<Level> ();
			BsonClassMap.RegisterClassMap<Scene> ();
			BsonClassMap.RegisterClassMap<Parameter> ();
			BsonClassMap.RegisterClassMap<Dialog> ();

			//Register Components
			ScalarDiscriminatorConvention con = new ScalarDiscriminatorConvention ("_t");
			BsonSerializer.RegisterDiscriminatorConvention (typeof(IComponent), con);
			BsonClassMap.RegisterClassMap <AccessComponent> ();
			BsonClassMap.RegisterClassMap<Description> ();

			//Register all EventProcessors
			BsonClassMap.RegisterClassMap<EventProcessorContainer> ();
			BsonClassMap.RegisterClassMap<EventProcessor> ();
			BsonClassMap.RegisterClassMap<Cancel> ();
			BsonClassMap.RegisterClassMap<Break> ();
			BsonClassMap.RegisterClassMap<TextOutput> ();
			BsonClassMap.RegisterClassMap<EventType> ();

			//Register the custom Entity serializer
			BsonSerializer.RegisterSerializer(typeof(Entity), entitySerializer);

		}

		private MongoDatabase InitDatabase()
		{
			MongoDatabase db = server.GetDatabase ("sharpA");
			levels = db.GetCollection ("levels");
			levelparameters = db.GetCollection ("levelparameters");
			scenes = db.GetCollection ("scenes");
			sceneparameters = db.GetCollection ("sceneparameters");
			objects = db.GetCollection ("objects");
			components = db.GetCollection ("components");
			eventprocessors = db.GetCollection ("eventprocessors");
			dialogs = db.GetCollection ("dialogs");
			audiofiles = db.GetCollection ("audiofiles");

			//TODO Check & Create Indizes
			//
			return db;
		}

		public Level GetLevel(string name)
		{
			return levels.FindOneAs<Level> (Query.EQ ("_id", name));
		}

		public MongoCursor<Level> GetLevels()
		{
			return levels.FindAllAs<Level> ();
		}

		public Parameter GetLevelParameter(string key, Level level)
		{
			return GetLevelParameter (key, level.Name);
		}

		public Parameter GetLevelParameter(string key, string level)
		{
			return levelparameters.FindOneAs<Parameter> (Query.And (Query.EQ ("Key", key), Query.EQ ("Parent", level)));
		}

		public MongoCursor<Parameter> GetLevelParameters(Level level)
		{
			return GetLevelParameters (level.Name);
		}

		public MongoCursor<Parameter> GetLevelParameters(string level)
		{
			return levelparameters.FindAs<Parameter>(Query.EQ("Parent",level));
		}

		public Scene GetScene(string name, Level level)
		{
			return GetScene(name, level.Name);
		}

		public Scene GetScene(string name, string levelName)
		{
			return scenes.FindOneAs<Scene> (Query.And (Query.EQ ("Name", name), Query.EQ ("LevelName", levelName)));
		}

		public MongoCursor<Scene> GetScenes(Level level)
		{
			return GetScenes (level.Name);
		}

		public MongoCursor<Scene> GetScenes(string levelName)
		{
			return scenes.FindAs<Scene> (Query.EQ ("LevelName", levelName));
		}

		public Parameter GetSceneParameter(string key, Scene scene)
		{
			return GetSceneParameter (key, scene.Name);
		}

		public Parameter GetSceneParameter(string key, string scene)
		{
			return sceneparameters.FindOneAs<Parameter> (Query.And (Query.EQ ("Key", key), Query.EQ ("Parent", scene)));
		}

		public MongoCursor<Parameter> GetSceneParameters(Scene scene)
		{
			return GetSceneParameters (scene.Name);
		}

		public MongoCursor<Parameter> GetSceneParameters(string scene)
		{
			return sceneparameters.FindAs<Parameter>(Query.EQ("Parent",scene));
		}

		public Dialog GetDialog(string name, Level level)
		{
			return GetDialog (name, level.Name);
		}

		public Dialog GetDialog(string name, string levelName)
		{
			return dialogs.FindOneAs<Dialog> (Query.And (Query.EQ ("Name", name), Query.EQ ("LevelName", levelName)));
		}

		public MongoCursor<Dialog> GetDialogs(Level level)
		{
			return GetDialogs (level.Name);
		}

		public MongoCursor<Dialog> GetDialogs(string levelName)
		{
			return dialogs.FindAs<Dialog> (Query.EQ ("LevelName", levelName));
		}

		public Entity GetEntity(string name, Scene scene)
		{
			return GetEntity (name, scene.Name, scene.LevelName);
		}

		public Entity GetEntity(string name, string sceneName, string levelName)
		{
			Entity e = objects.FindOneAs<Entity> (Query.And (Query.EQ ("Name", name), Query.EQ ("SceneName", sceneName), Query.EQ ("LevelName", levelName)));
			GetComponents (e);
			GetEventProcessors (e);
			return e;
		}


		public MongoCursor<Entity> GetEntities(Scene scene)
		{
			return GetEntities (scene.Name, scene.LevelName);
		}

		public MongoCursor<Entity> GetEntities(string sceneName, string levelName)
		{
			MongoCursor<Entity> entities = objects.FindAs<Entity> (Query.And (Query.EQ ("SceneName", sceneName), Query.EQ ("LevelName", levelName)));
			foreach (Entity e in entities) {
				GetComponents (e);
				GetEventProcessors (e);
			}
			return entities;
		}

		/**
		 * Added die Components zu einem Entity
		 * */
		private Entity GetComponents(Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			MongoCursor<IComponent> comps = components.FindAs<IComponent> (Query.And (Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)));

			foreach (IComponent c in comps) {
				entity.AddComponent(c);
			}
			return entity;
		}

		/**
		 * added die Eventprocessors zu einem Entity
		 * */
		private Entity GetEventProcessors(Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			EventProcessorContainer epc = eventprocessors.FindOneAs<EventProcessorContainer> (Query.And (Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)));

			entity.SetProcessors (epc.EventProcessors);
			return entity;
		}

		/**
		 * Je nachdem ob das Entity schon in der Datenbank ist, wird entweder ein Insert oder ein Update ausgeführt
		 * */
		public void SaveEntity(Entity entity)
		{
			if(entity.HasComponent<SerializationInfo>())
			{
				SerializationInfo si = entity.GetComponent<SerializationInfo> ();
				Entity e = objects.FindOneAs<Entity>(Query.And(Query.EQ("Name",si.Name),Query.EQ("SceneName",si.SceneName),Query.EQ("LevelName",si.LevelName)));
				if (e ==null) {
					InsertEntity (entity);
				} else {
					objects.Update (Query.And (Query.EQ ("Name", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)),
						Update.Set ("Name", si.Name).Set ("SceneName", si.SceneName).Set ("LevelName", si.LevelName));
					UpdateComponents (entity);
					UpdateEventProcessors (entity);
				}
			}
		}

		public void InsertLevel(Level level)
		{
			levels.Insert<Level> (level);
		}

		public void InsertLevelParameter(Parameter parameter)
		{
			levelparameters.Insert<Parameter> (parameter);
		}

		public void InsertScene(Scene scene)
		{
			scenes.Insert<Scene> (scene);
		}

		public void InsertSceneParameter(Parameter parameter)
		{
			sceneparameters.Insert<Parameter> (parameter);
		}

		public void InsertDialog(Dialog dialog)
		{
			dialogs.Insert<Dialog> (dialog);
		}

		public void InsertEntity(Entity entity)
		{
			objects.Insert<Entity> (entity);
			InsertComponents (entity);
			InsertEventProcessors (entity);
		}

		private void InsertComponents(Entity entity)
		{
			Bag<IComponent> comps = entity.Components;
			comps.Remove(entity.GetComponent<SerializationInfo>());

			SerializationInfo si = entity.GetComponent<SerializationInfo> ();

			foreach (IComponent component in comps) {
				BsonDocument doc = new BsonDocument ();
				BsonWriter writer = BsonWriter.Create (doc);
				doc.Add("EntityName", si.Name);
				doc.Add ("SceneName", si.SceneName);
				doc.Add ("LevelName", si.LevelName);
				BsonSerializer.Serialize (writer, typeof(IComponent), component);
				components.Insert(doc);
				writer.Dispose ();
			}
		}

		private void InsertEventProcessors(Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			EventProcessorContainer container = new EventProcessorContainer (entity.GetAllEventProcessors ());

			BsonDocument doc = new BsonDocument ();
			BsonWriter writer = BsonWriter.Create (doc);
			doc.Add("EntityName", si.Name);
			doc.Add ("SceneName", si.SceneName);
			doc.Add ("LevelName", si.LevelName);
			BsonSerializer.Serialize (writer, typeof(EventProcessorContainer), container);
			eventprocessors.Insert (doc);
			writer.Dispose ();
		}

		public void UpdateLevel(Level level)
		{
			levels.Update (Query.EQ ("_id", level.Name),Update.Set("CurrentScene",level.CurrentScene));
		}

		public void UpdateLevelParameter(Parameter parameter)
		{
			levelparameters.Update (Query.And (Query.EQ ("Key", parameter.Key), Query.EQ ("Parent", parameter.Parent)), Update.Set ("Value", parameter.Value));
		}

		public void UpdateScene(Scene scene)
		{
			scenes.Update (Query.And (Query.EQ ("Name", scene.Name), Query.EQ ("LevelName", scene.LevelName)),
				Update.Set("Description", scene.Description).Set("Width", scene.Width).Set("Height", scene.Height));
		}

		public void UpdateSceneParameter(Parameter parameter)
		{
			sceneparameters.Update (Query.And (Query.EQ ("Key", parameter.Key), Query.EQ ("Parent", parameter.Parent)), Update.Set ("Value", parameter.Value));
		}

		public void UpdateDialog(Dialog dialog)
		{
			dialogs.Update (Query.And (Query.EQ ("Name", dialog.Name), Query.EQ ("LevelName", dialog.LevelName)), Update.Set ("ScriptContent", dialog.ScriptContent));
		}

		private void UpdateComponents(Entity entity)
		{
			Bag<IComponent> comps = entity.Components;
			comps.Remove(entity.GetComponent<SerializationInfo>());

			SerializationInfo si = entity.GetComponent<SerializationInfo> ();

			foreach (IComponent component in comps) {
				BsonDocument doc = new BsonDocument ();
				BsonWriter writer = BsonWriter.Create (doc);
				doc.Add("EntityName", si.Name);
				doc.Add ("SceneName", si.SceneName);
				doc.Add ("LevelName", si.LevelName);
				BsonSerializer.Serialize (writer, typeof(IComponent), component);
				components.Update (Query.And (Query.EQ("_t", component.GetType().Name), Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)),new UpdateDocument(){{"$set",doc}});
				writer.Dispose ();

			}
		}

		private void UpdateEventProcessors (Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			EventProcessorContainer container = new EventProcessorContainer (entity.GetAllEventProcessors ());

			BsonDocument doc = new BsonDocument ();
			BsonWriter writer = BsonWriter.Create (doc);
			doc.Add("EntityName", si.Name);
			doc.Add ("SceneName", si.SceneName);
			doc.Add ("LevelName", si.LevelName);
			BsonSerializer.Serialize (writer, typeof(EventProcessorContainer), container);
			eventprocessors.Update (Query.And (Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)), new UpdateDocument (){ {
					"$set",
					doc
				} });
			writer.Dispose ();
		}

		public void RemoveLevel(Level level)
		{
			RemoveLevel (level.Name);
		}

		public void RemoveLevel(string level)
		{
			levels.Remove(Query.EQ("_id",level));
		}

		public void RemoveLevelParameter(Parameter parameter)
		{
			RemoveLevelParameter (parameter.Key, parameter.Parent);
		}

		public void RemoveLevelParameter(string key, string level)
		{
			levelparameters.Remove (Query.And (Query.EQ ("Key", key), Query.EQ ("Parent", level)));
		}

		public void RemoveLevelParameters(Level level)
		{
			RemoveLevelParameters (level.Name);
		}

		public void RemoveLevelParameters(string levelName)
		{
			levelparameters.Remove(Query.EQ("Parent",levelName));
		}

		public void RemoveScene(Scene scene)
		{
			RemoveScene (scene.Name, scene.LevelName);
		}

		public void RemoveScene(string name, string levelName)
		{
			scenes.Remove (Query.And (Query.EQ ("Name", name), Query.EQ ("LevelName", levelName)));
		}

		public void RemoveScenes(Level level)
		{
			RemoveScenes (level.Name);
		}

		public void RemoveScenes(string levelName)
		{
			scenes.Remove (Query.EQ ("LevelName", levelName));
		}

		public void RemoveSceneParameter(Parameter parameter)
		{
			RemoveSceneParameter (parameter.Key, parameter.Parent);
		}

		public void RemoveSceneParameter(string key, string scene)
		{
			sceneparameters.Remove (Query.And (Query.EQ ("Key", key), Query.EQ ("Parent", scene)));
		}

		public void RemoveSceneParameters(Scene scene)
		{
			RemoveSceneParameters (scene.Name);
		}

		public void RemoveSceneParameters(string sceneName)
		{
			sceneparameters.Remove(Query.EQ("Parent",sceneName));
		}

		public void RemoveDialog(Dialog dialog)
		{
			RemoveDialog (dialog.Name, dialog.LevelName);
		}

		public void RemoveDialog(string name, string levelName)
		{
			dialogs.Remove (Query.And (Query.EQ ("Name", name), Query.EQ ("LevelName", levelName)));
		}

		public void RemoveDialogs(Level level)
		{
			RemoveDialog (level.Name);
		}

		public void RemoveDialogs(string levelName)
		{
			dialogs.Remove (Query.EQ ("LevelName", levelName));
		}

		public void RemoveEntity(Entity entity)
		{
			if (entity.HasComponent<SerializationInfo> ()) {
				SerializationInfo si = entity.GetComponent<SerializationInfo> ();
				objects.Remove (Query.And (Query.EQ ("Name", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)));
				RemoveComponents (entity);
				RemoveEventProcessors (entity);
			}
		}

		public void RemoveEntities(Scene scene)
		{
			RemoveEntities (scene.Name, scene.LevelName);
		}

		public void RemoveEntities(string sceneName, string levelName)
		{
			objects.Remove (Query.And (Query.EQ ("SceneName", sceneName), Query.EQ ("LevelName", levelName)));
			RemoveComponents (sceneName, levelName);
			RemoveEventProcessors (sceneName, levelName);
		}

		private void RemoveComponents(Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			components.Remove (Query.And (Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)));
		}

		private void RemoveComponents(string sceneName, string levelName)
		{
			components.Remove (Query.And (Query.EQ ("SceneName", sceneName), Query.EQ ("LevelName", levelName)));
		}

		private void RemoveEventProcessors(Entity entity)
		{
			SerializationInfo si = entity.GetComponent<SerializationInfo> ();
			eventprocessors.Remove (Query.And (Query.EQ ("EntityName", si.Name), Query.EQ ("SceneName", si.SceneName), Query.EQ ("LevelName", si.LevelName)));
		}

		private void RemoveEventProcessors(string sceneName, string levelName)
		{
			eventprocessors.Remove (Query.And (Query.EQ ("SceneName", sceneName), Query.EQ ("LevelName", levelName)));
		}
	}
}

