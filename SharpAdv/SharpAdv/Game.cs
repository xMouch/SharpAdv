using System;
using System.Collections.Generic;
using System.Collections;

namespace SharpAdv
{
	public class Game :InputListener
	{
		private static Game game;
		public static Game get()
		{
			if (game == null)
				game = new Game ();
			return game;
		}

		private Dictionary<string,CommandProcessor> commands;
		public InputListener InputListener{ get; set;}

		public Game ()
		{
			commands = new Dictionary<string, CommandProcessor> (30);
		}

		public void registerCommandProcessor(CommandProcessor processor)
		{
			for (int x = 0; x < processor.commands.Length; x++)
				commands.Add (processor.commands [x], processor);
		}

		public void input(string[] input)
		{
			CommandProcessor cp = commands[input[0]];
			if (cp != null) 
			{
				if (cp.parameters == null || cp.parameters.Length == 0) {
					cp.Process (null);
				} else {
					for (int x = 0; x < cp.parameters.Length; x++) {
						if (cp.parameters [x].IsMatching (input) !=null) {
							cp.Process (input);
							return;
						}
					}
				}
				//Fehlermeldung
			} 
			else
				Console.WriteLine ("In der Konsole eine Fehlermeldung ausgeben");
		}
	}
}

