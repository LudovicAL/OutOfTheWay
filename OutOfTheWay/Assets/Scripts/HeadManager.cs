using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : MonoBehaviour {
	private CharacterManager selfCharacter;

	// Use this for initialization
	void Start () {
		selfCharacter = this.gameObject.GetComponentInParent<CharacterManager> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		selfCharacter.OnHeadTriggerEnter (col);
	}
}
