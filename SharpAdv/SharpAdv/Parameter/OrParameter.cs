using System;

namespace SharpAdv
{
	public class OrParameter : CommandParameter
	{
		private CommandParameter expr1;
		private CommandParameter expr2;

		public OrParameter (CommandParameter expr1, CommandParameter expr2)
		{
			this.expr1 = expr1;
			this.expr2 = expr2;
		}

		public bool Matching(string parameter)
		{
			return (expr1.Matching(parameter) || expr2.Matching(parameter));
		}
	}
}

