using UnityEngine;

public class FeetManager : MonoBehaviour {
	private CharacterManager selfCharacter;

	// Use this for initialization
	void Start () {
		selfCharacter = this.gameObject.GetComponentInParent<CharacterManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		selfCharacter.OnFeetTriggerEnter (col);
	}

	void OnTriggerExit2D(Collider2D col) {
		selfCharacter.OnFeetTriggerExit (col);
	}
}
