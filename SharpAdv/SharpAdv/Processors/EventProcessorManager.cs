using System;
using Artemis.System;
using Artemis;
using System.Collections.Generic;

namespace SharpAdv
{
	public class EventProcessorManager
	{
		public Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> Entities{ get; private set; }

		public EventProcessorManager ()
		{
			Entities = new Dictionary<Entity, Dictionary<EventType, List<EventProcessor>>> (32);
		}
			
		public void Add(Entity entity)
		{
			Entities.Add (entity, new Dictionary<EventType, List<EventProcessor>> (16));
		}

		public void Remove(Entity entity)
		{
			Entities.Remove (entity);
		}
	}

	public static class ProcessorExtension
	{
		public static void AddProcessor(this Entity entity,EventType type, EventProcessor processor)
		{
			Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> dic = Game.Get ().EPManager.Entities;
			if (!dic [entity].ContainsKey(type))
				dic [entity].Add (type, new List<EventProcessor> (16));
			dic[entity][type].Add(processor);
		}

		public static void AddProcessor(this Entity entity,EventType type, EventProcessor processor, int index)
		{
			Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> dic = Game.Get ().EPManager.Entities;
			if (!dic [entity].ContainsKey(type))
				dic [entity].Add (type, new List<EventProcessor> (16));
			dic[entity][type].Insert(index, processor);
		}

		public static void RemoveProcessor(this Entity entity,EventType type, EventProcessor processor)
		{
			Game.Get ().EPManager.Entities[entity][type].Remove(processor);
		}

		public static Dictionary<EventType, List<EventProcessor>> GetAllEventProcessors(this Entity entity)
		{
			return Game.Get ().EPManager.Entities[entity];
		}

		public static List<EventProcessor> GetEventProcessors(this Entity entity, EventType type)
		{
			return Game.Get ().EPManager.Entities[entity][type];
		}
	}
}

