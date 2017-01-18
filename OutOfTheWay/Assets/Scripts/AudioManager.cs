using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip menuMusic;
	public AudioClip gameMusic;
	public AudioClip deathSound;
	public AudioClip jumpSound;
	public AudioClip buttonSound;
	public GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	private AudioSource mainMusicAS;
	private AudioSource deathSoundAS;
	private AudioSource jumpSoundAS;
	private AudioSource buttonSoundAS;

	void Awake() {
		mainMusicAS = Camera.main.gameObject.AddComponent<AudioSource>();
		deathSoundAS = Camera.main.gameObject.AddComponent<AudioSource>();
		jumpSoundAS = Camera.main.gameObject.AddComponent<AudioSource>();
		buttonSoundAS = Camera.main.gameObject.AddComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		ConfigureAudioSource (mainMusicAS, menuMusic, true);
		ConfigureAudioSource (deathSoundAS, deathSound, false);
		ConfigureAudioSource (jumpSoundAS, jumpSound, false);
		ConfigureAudioSource (buttonSoundAS, buttonSound, false);
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Configures an AudioSource component
	private void ConfigureAudioSource (AudioSource thisAudioSource, AudioClip clip, bool loop) {
		thisAudioSource.playOnAwake = false;
		thisAudioSource.clip = clip;
		thisAudioSource.loop = loop;
	}

	//Updates the clip currently playing
	public void UpdateMusic() {
		switch(gameState) {
			case StaticData.AvailableGameStates.Menu:
				if (!mainMusicAS.clip.Equals(menuMusic)) {
					mainMusicAS.clip = menuMusic;
				}
				if (!mainMusicAS.isPlaying) {
					mainMusicAS.Play();
				}
				break;
			case StaticData.AvailableGameStates.Paused:
				mainMusicAS.Pause();
				break;
			case StaticData.AvailableGameStates.Playing:
			case StaticData.AvailableGameStates.Starting:
				if (!mainMusicAS.clip.Equals(gameMusic)) {
					mainMusicAS.clip = gameMusic;
				}
				if (!mainMusicAS.isPlaying) {
					mainMusicAS.Play();
				}
				break;
		}
	}

	//Plays the button sound
	public void PlayButtonSound() {
		buttonSoundAS.Play ();
	}

	//Plays the jump sound
	public void PlayJumpSound() {
		jumpSoundAS.Play ();
	}

	//Plays the death sound
	public void PlayDeathSound() {
		deathSoundAS.Play ();
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
		UpdateMusic ();
	}
}
