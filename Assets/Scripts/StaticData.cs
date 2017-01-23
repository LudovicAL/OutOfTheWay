//This static class stores information that is relevant application-wide.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData {

	public enum AvailableGameStates {
		Menu,	//Consulting the menu
		Starting,	//Game is starting
		Playing,	//Game is playing
		Paused	//Game is paused
	};

	public enum AvailableIntelligences {
		Human,	//Character is controlled by a human being
		ComputerAI,	//Character is controlled by a computer AI
		None	//Character isn't in the game
	};

	public static List<GameObject> characterList = new List<GameObject>();	//List of all characters
	public static List<GameObject> aliveList = new List<GameObject>();	//List of all alive characters
	public static List<GameObject> deadList = new List<GameObject>();	//List of all dead characters
	public static List<GameObject> playerList = new List<GameObject>();	//List of all human players
	public static List<GameObject> computerAIList = new List<GameObject>();	//List of all computer AI players
	public static List<GameObject> rejectedList = new List<GameObject>();	//List of all characters that are not currently in use
}
