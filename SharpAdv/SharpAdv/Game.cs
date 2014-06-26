using System;
using System.Collections.Generic;
using System.Collections;
using Artemis;

namespace SharpAdv
{
	public class Game :InputListener
	{
		private static Game game;
		public static Game Get()
		{
			if (game == null)
				game = new Game ();
			return game;
		}

		public EntityWorld World{ get; private set; }
		public EventProcessorManager EPManager{ get; private set; }
		private Dictionary<string,CommandProcessor> commands;
		public InputListener InputListener{ get; set;}
		public DatabaseManager DBManager{ get; private set; }

		private Game ()
		{
			commands = new Dictionary<string, CommandProcessor> (30);
			World = new EntityWorld ();
			EPManager = new EventProcessorManager (World);
			DBManager = new DatabaseManager (World);
		}

		public void Init ()
		{
			ProcessorExtension.EPManager = EPManager;
			World.InitializeAll ();
			DBManager.Init ();
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
					cp.Process (null,null);
				} else {
					input [0] = null;
					for (int x = 0; x < cp.parameters.Length; x++) {
						string[] output = cp.parameters [x].IsMatching (input);
						if (output !=null) {
							cp.Process (output,cp.parameters[x]);
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

