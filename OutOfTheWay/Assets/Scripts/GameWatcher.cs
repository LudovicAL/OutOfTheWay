using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWatcher : MonoBehaviour {

	public GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	private bool ending;

	void Awake() {
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		GameObject[] charTempo = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject c in charTempo) {
			StaticData.characterList.Add (c);
		}
	}
	// Use this for initialization
	void Start () {
		ending = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (!ending) {
				if (StaticData.aliveList.Count < 2) {
					StartCoroutine(EndGame());
				}
			}
		}
	}

	IEnumerator EndGame() {
		ending = true;
		yield return new WaitForSeconds(1.5f);
		if (gameState == StaticData.AvailableGameStates.Playing) {
			scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Paused);
		}
		ending = false;
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
	}
}
