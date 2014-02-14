using System;

namespace SharpAdv
{
	public class NotParameter : CommandParameter
	{
		private CommandParameter parameter;

		public NotParameter (CommandParameter parameter)
		{
			this.parameter = parameter;
		}

		public bool Matching(string parameter)
		{
			return !this.parameter.Matching (parameter);
		}
	}
}

