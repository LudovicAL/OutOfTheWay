using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public Text textCountDown;
	public GameObject panelMenu;
	public GameObject panelGame;
	public GameObject panelStarting;
	public GameObject panelMiniMenu;
	public GameObject scriptsBucket;
	public Dropdown[] ddPlayer;
	private StaticData.AvailableGameStates gameState;

	// Use this for initialization
	void Start () {
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		OnDropDownValueChanged ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Updates the canvas content
	public void UpdateCanvas() {
		panelMenu.SetActive (false);
		panelGame.SetActive (false);
		panelStarting.SetActive (false);
		panelMiniMenu.SetActive (false);
		switch (gameState){
			case StaticData.AvailableGameStates.Menu:
				panelMenu.SetActive (true);
				break;
			case StaticData.AvailableGameStates.Starting:
				panelGame.SetActive (true);
				panelStarting.SetActive (true);
				foreach (GameObject go in StaticData.characterList) {
					go.GetComponent<CharacterManager> ().Reset ();
				}
				StartCoroutine(ShowCountDown());
				break;
			case StaticData.AvailableGameStates.Paused:
				panelGame.SetActive (true);
				panelMiniMenu.SetActive (true);
				break;
			case StaticData.AvailableGameStates.Playing:
				panelGame.SetActive (true);
				break;
		}
	}

	//Shows a countdown before the game actually starts
	IEnumerator ShowCountDown() {
		textCountDown.text = "3";    
		yield return new WaitForSeconds(0.8f);
		textCountDown.text = "2";    
		yield return new WaitForSeconds(0.7f);
		textCountDown.text = "1";    
		yield return new WaitForSeconds(0.7f);
		textCountDown.text = "";
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Playing);
	}

	public void OnPlayButtonClick() {
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Starting);
	}

	public void OnMenuButtonClick() {
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Menu);
	}

	public void OnHowToButtonClick() {
		
	}

	public void OnExitButtonClick() {
		Application.Quit ();
	}

	//Determines what intelligence will control every player
	public void OnDropDownValueChanged() {
		StaticData.playerList.Clear ();
		StaticData.computerAIList.Clear ();
		StaticData.rejectedList.Clear ();
		StaticData.aliveList.Clear ();
		StaticData.deadList.Clear ();
		for (int i = 0, maxA = ddPlayer.Length, maxB = StaticData.characterList.Count; i < maxA && i < maxB; i++) {
			//Debug.Log ("Player " + i + " is " + ddPlayer [i].GetComponentInChildren<Text> ().text);
			switch (ddPlayer[i].GetComponentInChildren<Text>().text) {
				case "Human":
					StaticData.playerList.Add (StaticData.characterList [i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.Human;
					break;
				case "Computer AI":
					StaticData.computerAIList.Add (StaticData.characterList[i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.ComputerAI;
					break;
				case "None":
					StaticData.rejectedList.Add (StaticData.characterList[i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.None;
					break;
			}
		}
	}

	protected void OnMenu() {
		SetGameState (StaticData.AvailableGameStates.Menu);
	}

	protected void OnStarting() {
		SetGameState (StaticData.AvailableGameStates.Starting);
	}

	protected void OnPausing() {
		SetGameState (StaticData.AvailableGameStates.Paused);
	}

	protected void OnPlaying() {
		SetGameState (StaticData.AvailableGameStates.Playing);
	}

	public void SetGameState(StaticData.AvailableGameStates state) {
		gameState = state;
		UpdateCanvas ();
	}
}
