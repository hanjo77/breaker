using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour {

	public int score = 100;
	public char brickType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null && transform.localPosition.y + transform.parent.position.y < -6) {
			GameObject.Destroy (gameObject);
		}
	}
}
