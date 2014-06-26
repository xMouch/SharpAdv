using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SharpAdv
{
	/**
	 * ist nur eine Testklasse, nicht verwenden und bald wieder löschen
	 * */
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

