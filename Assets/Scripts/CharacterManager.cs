//This script provides everything required to have the characters move.

using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour {

	private StaticData.AvailableGameStates gameState;
	private AudioManager am;
	public StaticData.AvailableIntelligences intelligence;
	public GameObject scriptsBucket;
	public GameObject textMesh;
	public Rigidbody2D selfRigidbody { get; private set; }
	private TimeSpan flightDuration;
	public float movementSpeed;
	public float climbingSpeed;
	public float flyingVelocity;
	public float jumpForce;
	public int numberOfParticles;
	public TimeSpan maximumflightDuration;
	public bool grounded { get; private set; }
	public bool hasControl { get; private set; }
	public Vector3 initialPosition { get; private set; }
	public string verticalButton;
	public string horizontalButton;
	public string higherJumpButton;

	// Use this for initialization
	void Start () {
		am = scriptsBucket.GetComponent<AudioManager> ();
		this.GetComponent<TrailRenderer> ().sortingOrder = -2;
		maximumflightDuration = TimeSpan.FromSeconds (1);
		selfRigidbody = this.GetComponent<Rigidbody2D> ();
		initialPosition = this.transform.localPosition;
		Reset ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}

	// Update is called once per frame
	void Update () {
		if (intelligence == StaticData.AvailableIntelligences.Human && gameState == StaticData.AvailableGameStates.Playing) {
			if (Input.GetButton(horizontalButton)) {	//Move left or right
				MoveLeftRight(Mathf.Sign(Input.GetAxisRaw(horizontalButton)));
			}
			if (Input.GetButton(verticalButton)) {	//Jumps or fly
				if (Input.GetButton(higherJumpButton)) {
					JumpFly (1.5f);	//High jump or fly
				} else {
					JumpFly (1.0f);	//Low jump or fly
				}
			}
		}
	}

	//Makes the character jump or fly
	public void JumpFly(float modifier) {
		if (grounded) {	//Will jump
			selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, jumpForce * modifier);
			am.PlayJumpSound ();
		} else {	//Will fly
			if (selfRigidbody.velocity.y <= flyingVelocity && flightDuration < maximumflightDuration) {
				selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, flyingVelocity);
				flightDuration += TimeSpan.FromSeconds(Time.deltaTime);
			}
		}
	}

	//Makes the character move left or right
	public void MoveLeftRight(float direction) {
		if (hasControl) {
			selfRigidbody.velocity = new Vector2(movementSpeed * direction, selfRigidbody.velocity.y);
		}
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.transform.tag == "Wall") {	//Character collided with a wall
			selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, climbingSpeed);
		}
	}

	public void OnHeadTriggerEnter(Collider2D col) {
		if (col.transform.tag == "UpperWall") {	//Character's head touched the upper wall
			selfRigidbody.velocity = new Vector2 (movementSpeed * Mathf.Sign(Camera.main.transform.position.x - this.transform.position.x), selfRigidbody.velocity.y);
			hasControl = false;
		} else if (col.transform.tag == "Feet") {	//Character's head was touched by another character's feet
			Die ();
		}
	}

	public void OnFeetTriggerEnter(Collider2D col) {
		if (col.transform.tag == "Floor") {	//Character's feet touched the floor
			grounded = true;
			hasControl = true;
			flightDuration = TimeSpan.Zero;
		}
	}

	public void OnFeetTriggerExit(Collider2D col) {
		if (col.transform.tag == "Floor") {	//Character's feet left the floor
			grounded = false;
		}
	}

	//Resets the character to its initial state and position
	public void Reset() {
		flightDuration = TimeSpan.Zero;
		grounded = true;
		hasControl = true;
		selfRigidbody.velocity = Vector2.zero;
		this.transform.localPosition = initialPosition;
		if (!StaticData.rejectedList.Contains(this.gameObject)) {
			this.gameObject.SetActive (true);
			StaticData.deadList.Remove (this.gameObject);
			if (!StaticData.aliveList.Contains(this.gameObject)) {
				StaticData.aliveList.Add (this.gameObject);
			}
		} else {
			this.gameObject.SetActive (false);
		}
	}

	//Kills the character
	public void Die() {
		am.PlayDeathSound ();
		scriptsBucket.transform.position = this.transform.position;
		scriptsBucket.GetComponent<ParticleSystem> ().Emit (numberOfParticles);
		this.gameObject.SetActive (false);
		StaticData.aliveList.Remove (this.gameObject);
		StaticData.deadList.Add (this.gameObject);
	}

	protected void OnMenu() {
		SetGameState (StaticData.AvailableGameStates.Menu);
		textMesh.SetActive (false);
	}

	protected void OnStarting() {
		SetGameState (StaticData.AvailableGameStates.Starting);
		textMesh.SetActive (true);
	}

	protected void OnPausing() {
		SetGameState (StaticData.AvailableGameStates.Paused);
		textMesh.SetActive (true);
	}

	protected void OnPlaying() {
		SetGameState (StaticData.AvailableGameStates.Playing);
		textMesh.SetActive (false);
	}

	public void SetGameState(StaticData.AvailableGameStates state) {
		gameState = state;
	}
}
