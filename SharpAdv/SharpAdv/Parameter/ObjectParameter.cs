using System;
using Artemis;
using Artemis.Utils;

namespace SharpAdv
{
	public class ObjectParameter : CommandParameter
	{
		private Aspect aspect;

		public ObjectParameter()
		{
			aspect = Aspect.Empty ();
		}

		public ObjectParameter (Aspect aspect)
		{
			if(aspect!=null)
				this.aspect = aspect;
			aspect = Aspect.Empty ();
		}

		public bool Matching(string parameter)
		{
			Bag<Entity> entities = Game.Get ().World.EntityManager.GetEntities (Aspect.All(typeof(AccessComponent)));
			foreach (Entity e in entities) {
				AccessComponent ac = e.GetComponent<AccessComponent> ();
				if (ac.Visible && ac.Name.ToLower().Equals (parameter.ToLower()) && aspect.Interests (e))
					return true;
			}

			return false;
		}
	}
}

