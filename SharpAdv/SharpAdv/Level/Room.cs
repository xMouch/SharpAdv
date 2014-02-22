using System;
using Artemis;
using System.Collections.Generic;

namespace SharpAdv
{
	public class Room
	{
		public string ID{ get; private set; }
		public string Description{ get; set; }
		public uint Width{ get; set; }
		public uint Height{ get; set; }
		public List<Entity> Entities{ get; set; }

		public Room ()
		{

		}

		public Room(string ID, string Description, uint Width, uint Height)
		{
			this.ID = ID;
			this.Description = Description;
			this.Width = Width;
			this.Height = Height;
		}
	}
}

