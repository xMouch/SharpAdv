using System;
using System.Collections.Generic;
using System.Collections;

namespace AEngineTest
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
				string[] parameter = new string[cp.parameters.Length];
				for (int x = 0; x < cp.parameters.Length; x++) 
				{
					for (int y = 1; y < input.Length; y++) 
					{
						if (input [y] != null && cp.parameters [x].matching (input [y])) 
						{
							parameter [x] = input [y];
							input [y] = null;
						}
					}
					if (parameter [x] == null) 
					{
						cp.Error (x);
						return;
					}
				}
				cp.Process (parameter);
			} 
			else
				Console.WriteLine ("In der Konsole eine Fehlermeldung ausgeben");
		}
	}
}

