using System;

namespace SharpAdv
{
	public abstract class CommandProcessor
	{
		public string[] commands{ get; set;}
		public ParameterSet[] parameters{ get;set;}

		public CommandProcessor ()
		{
		}

		public abstract void Process(string[] args, ParameterSet foundParameters);
		public abstract void Error(ParameterSet[] set, int index);

	}
}

