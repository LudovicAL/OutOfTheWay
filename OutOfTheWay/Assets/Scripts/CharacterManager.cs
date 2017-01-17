using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour {

	private StaticData.AvailableGameStates gameState;
	public GameObject scriptsBucket;
	private Rigidbody2D selfRigidbody;
	private TimeSpan flightDuration;
	public float movementSpeed;
	public float climbingSpeed;
	public float flyingVelocity;
	public float jumpForce;
	public TimeSpan maximumflightDuration;
	public bool grounded { get; private set; }
	public bool hasControl { get; private set; }
	public Vector3 initialPosition { get; private set; }

	// Use this for initialization
	void Start () {
		this.GetComponent<TrailRenderer> ().sortingOrder = -2;
		flightDuration = TimeSpan.Zero;
		maximumflightDuration = TimeSpan.FromSeconds (1);
		grounded = true;
		hasControl = true;
		selfRigidbody = this.GetComponent<Rigidbody2D> ();
		initialPosition = this.transform.localPosition;
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		SetGameState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}

	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (Input.GetButton("Horizontal") && hasControl) {	//Move left or right
				selfRigidbody.velocity = new Vector2(movementSpeed * Mathf.Sign(Input.GetAxisRaw("Horizontal")), selfRigidbody.velocity.y);
			}
			if (grounded) {
				if (Input.GetButton("Vertical")) {	//Jump
					if (Input.GetButton("Fire3")) {
						selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, jumpForce * 1.5f);	//High jump
					} else {
						selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, jumpForce);	//Low jump
					}
				}
			} else {
				if (Input.GetButton("Vertical")) {	//Fly
					if (selfRigidbody.velocity.y <= flyingVelocity && flightDuration < maximumflightDuration) {
						selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, flyingVelocity);
						flightDuration += TimeSpan.FromSeconds(Time.deltaTime);
					}
				}
			}
		}
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.transform.tag == "Wall") {
			selfRigidbody.velocity = new Vector2 (selfRigidbody.velocity.x, climbingSpeed);
		}
	}

	public void OnHeadTriggerEnter(Collider2D col) {
		if (col.transform.tag == "UpperWall") {
			selfRigidbody.velocity = new Vector2 (movementSpeed * 2 * Mathf.Sign(Camera.main.transform.position.x - this.transform.position.x), selfRigidbody.velocity.y);
			hasControl = false;
		} else if (col.transform.tag == "Feet") {
			Die ();
		}
	}

	public void OnFeetTriggerEnter(Collider2D col) {
		if (col.transform.tag == "Floor") {
			grounded = true;
			hasControl = true;
			flightDuration = TimeSpan.Zero;
		}
	}

	public void OnFeetTriggerExit(Collider2D col) {
		if (col.transform.tag == "Floor") {
			grounded = false;
		}
	}

	public void Reset() {
		flightDuration = TimeSpan.Zero;
		grounded = true;
		hasControl = true;
		this.transform.localPosition = initialPosition;
		this.gameObject.SetActive (true);
	}

	public void Die() {
		this.gameObject.SetActive (false);
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
