using System;

namespace SharpAdv
{
	public class TextOutput : EventProcessor
	{
		public string Text{ get; set; }

		public TextOutput (string text)
		{
			this.Text = text;
		}

		public override void Event(Event e)
		{
			Console.WriteLine (Text);
		}
	}
}

