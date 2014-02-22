using System;

namespace SharpAdv
{
	public class EventType
	{
		public Type Type{ get; private set;}

		public EventType (Event e)
		{
			Type = e.GetType ();
		}

		public EventType(Type t)
		{
			if (t.IsSubclassOf (typeof(Event)))
				Type = t;
			else
				throw new ArgumentException ("Type has to be an Event");
		}

		public override bool Equals(Object o)
		{
			return Type.GUID.Equals(o.GetType().GUID);
		}

		public override int GetHashCode()
		{
			return Type.GetHashCode ();
		}
	}
}

