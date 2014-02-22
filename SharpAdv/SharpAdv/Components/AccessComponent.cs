using System;
using Artemis.Interface;

namespace SharpAdv
{
	public class AccessComponent : IComponent
	{
		public string Name{ get; set;}
		public bool Visible{ get; set;}

		public AccessComponent ()
		{
		}
	}
}

