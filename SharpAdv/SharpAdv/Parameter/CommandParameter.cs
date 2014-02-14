using System;

namespace SharpAdv
{
	public interface CommandParameter
	{
		bool Matching(string parameter);
	}
}

