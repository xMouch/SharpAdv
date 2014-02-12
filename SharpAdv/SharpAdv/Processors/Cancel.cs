using System;

namespace AEngineTest
{
	public class Cancel : EventProcessor
	{
		public Cancel ()
		{
		}

		public override void Event(Event e)
		{
			e.Cancel = true;
		}
	}
}

