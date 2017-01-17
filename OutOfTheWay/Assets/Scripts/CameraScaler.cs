using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour {

	public float widthToBeSeen;
	public GameObject floor;

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = widthToBeSeen * (float)Screen.height / (float)Screen.width * 0.5f;
		int e = 0;
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		while (!GeometryUtility.TestPlanesAABB (planes, floor.GetComponent<SpriteRenderer> ().bounds) && e < 10) {
			Camera.main.transform.Translate(Vector3.down);
			planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
			e++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
