using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace SharpAdv
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Game.Get ();
			Game.Get ().Init ();
		}
	}
}
