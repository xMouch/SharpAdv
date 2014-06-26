using System;
using System.Collections.Generic;

namespace SharpAdv
{
	public static class EventTypes
	{
		private static readonly Dictionary<Type, EventType> types = new Dictionary<Type, EventType>(32);

		public static EventType GetType<T>() where T:Event
		{
			return GetType (typeof(T));
		}

		public static EventType GetType(Type type)
		{
			EventType t;
			if (!types.TryGetValue (type,out t)) 
			{
				t = new EventType ();
				t.Id = EventType.nextId;
				EventType.nextId++;
				types.Add (type, t);
			}

			return t;
		}
	}
}

