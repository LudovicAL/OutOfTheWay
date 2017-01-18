using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour {

	private StaticData.AvailableGameStates gameState;
	public StaticData.AvailableIntelligences intelligence;
	public GameObject scriptsBucket;
	private Rigidbody2D selfRigidbody;
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

	// Use this for initialization
	void Start () {
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
		intelligence = StaticData.AvailableIntelligences.Human;
	}

	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (intelligence == StaticData.AvailableIntelligences.Human) {
				if (Input.GetButton("Horizontal") && hasControl) {	//Move left or right
					MoveLeftRight(Mathf.Sign(Input.GetAxisRaw("Horizontal")));
				}
				if (grounded) {
					if (Input.GetButton("Vertical")) {	//Jump
						if (Input.GetButton("Fire3")) {
							Jump (1.5f);	//High jump
						} else {
							Jump (1.0f);	//Low jump
						}
					}
				} else {
					if (Input.GetButton("Vertical")) {	//Fly
						if (selfRigidbody.velocity.y <= flyingVelocity && flightDuration < maximumflightDuration) {
							Fly ();
						}
					}
				}
			}
		}
	}

	public void Fly() {
		selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, flyingVelocity);
		flightDuration += TimeSpan.FromSeconds(Time.deltaTime);
	}

	public void Jump(float modifier) {
		selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, jumpForce * modifier);
	}

	public void MoveLeftRight(float direction) {
		selfRigidbody.velocity = new Vector2(movementSpeed * direction, selfRigidbody.velocity.y);
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.transform.tag == "Wall") {	//Character collided with a wall
			selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, climbingSpeed);
		}
	}

	public void OnHeadTriggerEnter(Collider2D col) {
		if (col.transform.tag == "UpperWall") {	//Character's head touched the upper wall
			selfRigidbody.velocity = new Vector2 (movementSpeed * 2 * Mathf.Sign(Camera.main.transform.position.x - this.transform.position.x), selfRigidbody.velocity.y);
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
		scriptsBucket.transform.position = this.transform.position;
		scriptsBucket.GetComponent<ParticleSystem> ().Emit (numberOfParticles);
		this.gameObject.SetActive (false);
		StaticData.aliveList.Remove (this.gameObject);
		StaticData.deadList.Add (this.gameObject);
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
