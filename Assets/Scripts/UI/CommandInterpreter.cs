using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInterpreter : MonoBehaviour
{
    private delegate void CommandAction(Console console, string[] args);

    struct Command
    {
        public string text;
        public CommandAction action;
    };

    private Command[] commands;
    private GameObject player = null;

	void Start ()
    {
        Console.AddListener(OnSubmit);

        commands = new Command[]
        {
            new Command { text = "/loc", action = OnLoc },
            new Command { text = "/goto", action = OnGoTo },
            new Command { text = "/quit", action = OnQuit }
        };

        player = GameObject.FindGameObjectWithTag("Player");
	}

    public void OnSubmit(object source, ConsoleArguments _args)
    {
        Console console = source as Console;

        foreach(Command cmd in commands)
        {
            if(cmd.text.CompareTo(_args.args[0]) == 0)
            {
                cmd.action(console, _args.args);
            }
        }
    }

    // /loc -> position of player
    void OnLoc(Console console, params string[] args)
    {
        string output;

        if(player)
        {
            output = "You are at " + player.transform.position.x + ", "
                + player.transform.position.y + ", "
                + player.transform.position.z;
        }
    }

    void OnGoTo(Console console, params string[] args)
    {
        string output;

        if(args.Length != 3)
        {
            console.AddString("Error: /goto x,y,z");
            return;
        }

        if(player)
        {
            output = "Teleport to " + args[1] + ", " + args[2];
            console.AddString(output);

            float newX = float.Parse(args[1]);
            float newZ = float.Parse(args[2]);

            Vector3 position = new Vector3(newX, 0f, newZ);
            position.y = Terrain.activeTerrain.SampleHeight(position) + Terrain.activeTerrain.GetPosition().y + 1.0f;
            player.transform.position = position;
        }
    }

    void OnQuit(Console console, params string[] args)
    {

    }
}
