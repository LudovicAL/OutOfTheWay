using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompAI : MonoBehaviour {

	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	private GameObject target;
	public CharacterManager cm;
	public List<GameObject> enemyList;

	void Awake () {
		enemyList = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {
		scriptsBucket = GameObject.FindGameObjectWithTag ("ScriptsBucket");
		cm = this.GetComponent<CharacterManager> ();
		enemyList = new List<GameObject>();
		target = null;
		UpdateEnemiesList ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (target != null) {
				if (target.activeSelf) {
					cm.MoveLeftRight (Mathf.Sign(target.transform.position.x - this.transform.position.x));
					if (cm.grounded) {
						cm.JumpFly(Random.Range(1.0f, 1.5f));
					}
				} else {
					UpdateEnemiesList ();
					UpdateTarget ();
				}
			} else {
				UpdateTarget ();
			}
		}
	}

	//Picks a new target that the AI will aim for
	public void UpdateTarget() {
		if (enemyList.Count > 0) {
			target = enemyList [Random.Range (0, enemyList.Count - 1)];
		}
	}

	//Updates the list of current enemies
	public void UpdateEnemiesList() {
		enemyList.Clear ();
		foreach (GameObject go in StaticData.aliveList) {
			if (!go.Equals(this.gameObject)) {
				enemyList.Add (go);
			}
		}
	}

	protected void OnMenu() {
		SetGameState (StaticData.AvailableGameStates.Menu);
	}

	protected void OnStarting() {
		SetGameState (StaticData.AvailableGameStates.Starting);
		UpdateEnemiesList ();
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
