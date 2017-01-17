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
	public GameObject[] characters;
	private StaticData.AvailableGameStates gameState;

	// Use this for initialization
	void Start () {
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCanvas() {
		panelMenu.SetActive (false);
		panelGame.SetActive (false);
		panelStarting.SetActive (false);
		panelMiniMenu.SetActive (false);
		switch (scriptsBucket.GetComponent<GameStatesManager>().gameState){
			case StaticData.AvailableGameStates.Menu:
				panelMenu.SetActive (true);
				break;
			case StaticData.AvailableGameStates.Starting:
				panelGame.SetActive (true);
				panelStarting.SetActive (true);
				foreach (GameObject go in characters) {
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
