using System;

namespace SharpAdv
{
	public class NumberParameter :CommandParameter
	{
		private bool isNumber;

		private int[] expected;
		private int min = 1;
		private int max = -1;
		private int[] excluded;

		public NumberParameter()
		{
			isNumber = true;
		}

		public NumberParameter (int expected):
			this(new int[]{ expected })
		{
		}

		public NumberParameter(int[] expected)
		{
			this.expected = expected;
		}

		public NumberParameter(int min, int max):
			this(min,max,null)
		{
		}

		public NumberParameter(int min, int max, int[] excluded)
		{
			this.min = min;
			this.max = max;
			this.excluded = excluded;
		}

		public bool Matching (string parameter)
		{
			int x;
			if (!int.TryParse (parameter, out x))
				return false;
			if (isNumber)
				return true;
			return ((expected!=null && Array.IndexOf<int>(expected,x)>=0)||
				(x>=min && x<=max && (excluded==null || Array.IndexOf<int>(excluded,x)<0)));
		}
	}
}

