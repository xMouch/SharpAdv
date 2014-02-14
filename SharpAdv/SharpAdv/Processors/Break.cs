using System;

namespace SharpAdv
{
	public class Break : EventProcessor
	{
		public Break ()
		{
		}

		public override void Event(Event e)
		{
			e.Break = true;
		}
	}
}

