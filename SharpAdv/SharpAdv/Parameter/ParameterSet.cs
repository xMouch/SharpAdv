using System;
using System.Collections.Generic;

namespace SharpAdv
{
	public class ParameterSet
	{
		public List<CommandParameter> Parameters{get;set;}

		public ParameterSet ()
		{
			Parameters = new List<CommandParameter> (4);
		}

		public string[] IsMatching(string[] parameters)
		{
			string[] output = new string[Parameters.Count];
			foreach (CommandParameter cp in Parameters) 
			{
				bool found = false;
				for (int x = 0; x < parameters.Length; x++) 
				{
					if (parameters [x] != null && cp.Matching(parameters[x]))
					{
						output [x] = parameters [x];
						found = true;
						break;
					}
				}
				if (!found)
					return null;
			}
			return output;
		}
	}
}

