using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public GameObject panelMenu;
	public GameObject panelGame;
	public GameObject panelStarting;
	public GameObject panelMiniMenu;
	public GameObject scriptsBucket;
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
				break;
			case StaticData.AvailableGameStates.Starting:
				break;
			case StaticData.AvailableGameStates.Paused:
				break;
			case StaticData.AvailableGameStates.Playing:
				break;
		}
	}

	protected void OnMenu() {
		SetGameState (StaticData.AvailableGameStates.Playing);
	}

	protected void OnStarting() {
		SetGameState (StaticData.AvailableGameStates.Playing);
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
