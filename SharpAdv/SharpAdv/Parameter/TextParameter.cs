using System;

namespace SharpAdv
{
	public class TextParameter : CommandParameter
	{
		private string[] expected;
		public bool Exact{ get; set;}

		public TextParameter (string expected)
		{
			this.expected = new string[]{ expected };
		}

		public TextParameter(string[] expected)
		{
			this.expected = expected;
		}

		public bool Matching(string parameter)
		{
			for(int x = 0;x<expected.Length;x++)
				if((Exact && parameter.Equals(expected[x]))||(!Exact && parameter.ToLower().Equals(expected[x].ToLower())))
					return true;
			return false;
		}
	}
}

