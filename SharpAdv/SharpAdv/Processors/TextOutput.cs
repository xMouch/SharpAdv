using System;

namespace SharpAdv
{
	public class TextOutput : EventProcessor
	{
		public string text{ get; set; }

		public TextOutput (string text)
		{
			this.text = text;
		}

		public override void Event(Event e)
		{
			Console.WriteLine (text);
		}
	}
}

