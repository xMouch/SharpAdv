using System;

namespace AEngineTest
{
	public abstract class CommandProcessor
	{
		public string[] commands{ get; set;}
		public CommandParameter[] parameters{ get;set;}

		public CommandProcessor ()
		{
		}

		public abstract void Process(string[] args);
		public abstract void Error(int parameter);

	}
}

