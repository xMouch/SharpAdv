using System;
using Artemis.System;
using Artemis;
using System.Collections.Generic;

namespace SharpAdv
{
	public class EventProcessorManager
	{
		public Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> Entities{ get; private set; }

		public EventProcessorManager (EntityWorld world)
		{
			Entities = new Dictionary<Entity, Dictionary<EventType, List<EventProcessor>>> (32);
			world.EntityManager.AddedEntityEvent += Add;
			world.EntityManager.RemovedEntityEvent += Remove;
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
		public static EventProcessorManager EPManager { get; set; }

		public static void AddProcessor(this Entity entity,EventType type, EventProcessor processor)
		{
			Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> dic = EPManager.Entities;
			if (!dic [entity].ContainsKey(type))
				dic [entity].Add (type, new List<EventProcessor> (16));
			dic[entity][type].Add(processor);
		}

		public static void AddProcessor(this Entity entity,EventType type, EventProcessor processor, int index)
		{
			Dictionary<Entity,Dictionary<EventType, List<EventProcessor>>> dic = EPManager.Entities;
			if (!dic [entity].ContainsKey(type))
				dic [entity].Add (type, new List<EventProcessor> (16));
			dic[entity][type].Insert(index, processor);
		}

		public static void SetProcessors(this Entity entity, Dictionary<EventType,List<EventProcessor>> eventProcessors)
		{
			EPManager.Entities [entity] = eventProcessors;
		}

		public static void RemoveProcessor(this Entity entity,EventType type, EventProcessor processor)
		{
			EPManager.Entities[entity][type].Remove(processor);
		}

		public static Dictionary<EventType, List<EventProcessor>> GetAllEventProcessors(this Entity entity)
		{
			return EPManager.Entities[entity];
		}

		public static List<EventProcessor> GetEventProcessors(this Entity entity, EventType type)
		{
			return EPManager.Entities[entity][type];
		}
	}
}

