//This script make sure the right canvas-panel is shown at the right time.
//It also sets the number of players that will be involved in the next game
//and decides which of them are controled by human players or AI.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	private AudioManager am;
	public Text textCountDown;
	public GameObject panelMenu;
	public GameObject panelPrimaryMenu;
	public GameObject panelHowToPlay;
	public GameObject panelGame;
	public GameObject panelStarting;
	public GameObject panelMiniMenu;
	public GameObject scriptsBucket;
	public Dropdown[] ddPlayer;
	private StaticData.AvailableGameStates gameState;

	// Use this for initialization
	void Start () {
		am = scriptsBucket.GetComponent<AudioManager> ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		for (int i = 1, max = ddPlayer.Length; i < max; i++) {
			ddPlayer [i].value = 1;
		}
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
		Button[] buttons;
		switch (gameState){
			case StaticData.AvailableGameStates.Menu:
				panelMenu.SetActive (true);
				panelPrimaryMenu.SetActive (true);
				panelHowToPlay.SetActive (false);
				buttons = panelMenu.GetComponentsInChildren<Button> ();
				if (buttons.Length > 0) {
					buttons [0].Select();
				}
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
				buttons = panelMiniMenu.GetComponentsInChildren<Button> ();
				if (buttons.Length > 0) {
					buttons [0].Select();
				}
				break;
			case StaticData.AvailableGameStates.Playing:
				panelGame.SetActive (true);
				break;
		}
	}

	//Shows a countdown before the game actually starts
	IEnumerator ShowCountDown() {
		for (int i = 3; i > 0; i--) {
			textCountDown.text = i.ToString();    
			yield return new WaitForSeconds(0.7f);
		}
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Playing);
	}

	public void OnPlayButtonClick() {
		am.PlayButtonSound ();
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Starting);
	}

	public void OnMenuButtonClick() {
		am.PlayButtonSound ();
		scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Menu);
	}

	public void OnHowToPlayButtonClick() {
		am.PlayButtonSound ();
		panelPrimaryMenu.SetActive (false);
		panelHowToPlay.SetActive (true);
		Button[] buttons = panelHowToPlay.GetComponentsInChildren<Button> ();
		if (buttons.Length > 0) {
			buttons [0].Select();
		}
	}

	public void OnExitButtonClick() {
		am.PlayButtonSound ();
		Application.Quit ();
	}

	//Determines what intelligence will control every player
	public void OnDropDownValueChanged() {
		am.PlayButtonSound ();
		StaticData.playerList.Clear ();
		StaticData.computerAIList.Clear ();
		StaticData.rejectedList.Clear ();
		StaticData.aliveList.Clear ();
		StaticData.deadList.Clear ();
		for (int i = 0, maxA = ddPlayer.Length, maxB = StaticData.characterList.Count; i < maxA && i < maxB; i++) {
			switch (ddPlayer[i].GetComponentInChildren<Text>().text) {
				case "Human":
					StaticData.playerList.Add (StaticData.characterList [i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.Human;
					RemoveComputerAI (StaticData.characterList [i]);
					break;
				case "Computer AI":
					StaticData.computerAIList.Add (StaticData.characterList [i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.ComputerAI;
					AddComputerAI (StaticData.characterList [i]);
					break;
				case "None":
					StaticData.rejectedList.Add (StaticData.characterList[i]);
					StaticData.characterList [i].GetComponent<CharacterManager> ().intelligence = StaticData.AvailableIntelligences.None;
					RemoveComputerAI (StaticData.characterList [i]);
					break;
			}
		}
	}

	//Adds a CompAI component to a GameObject
	private void AddComputerAI(GameObject go) {
		if (go.GetComponent<CompAI> () == null) {
			go.AddComponent<CompAI> ();
		}
	}

	//Removes a CompAI component from a GameObject
	private void RemoveComputerAI(GameObject go) {
		Destroy (go.GetComponent<CompAI> ());
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
