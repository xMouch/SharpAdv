using System;
using System.Collections.Generic;
using System.Reflection;

namespace AEngineTest
{
	public abstract class EventProcessor
	{
		public List<EventProcessor> If{ get; set; }
		public List<EventProcessor> Else{ get; set; }

		public EventProcessor ()
		{
			If = new List<EventProcessor> ();
			Else = new List<EventProcessor> ();
		}

		public abstract void Event(Event e);

		public void AddProcessor(EventProcessor processor, bool elseProcessor)
		{
			if (elseProcessor)
				Else.Add (processor);
			else
				If.Add (processor);
		}

		public void AddProcessor(EventProcessor processor, bool elseProcessor, int index)
		{
			if (elseProcessor)
				Else.Insert (index, processor);
			else
				If.Insert (index, processor);
		}

		public void RemoveProcessor(EventProcessor processor, bool elseProcessor)
		{
			if (elseProcessor)
				Else.Remove (processor);
			else
				If.Remove (processor);
		}

		public void RemoveProcessor(int index, bool elseProcessor)
		{
			if (elseProcessor)
			Else.RemoveAt(index);
			else
				If.RemoveAt(index);
		}

		public static void Process(List<EventProcessor> processors,Event e)
		{
			foreach (EventProcessor p in processors) 
			{
				MethodInfo method = p.GetType ().GetMethod ("Event", new Type[]{ e.GetType () });
				method.Invoke (p, new object[]{ e });
				if (e.Cancel)
					break;
				if (e.Break) 
				{
					e.Break = false;
					break;
				}
				if (e.If)
					EventProcessor.Process (p.If, e);
				else
					EventProcessor.Process (p.Else, e);
				if (e.Cancel)
					break;
			}
		}
	}
}

