using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour {

	public int score = 100;
	public char brickType;

	private BreakerBehaviour gameInstance;

	// Use this for initialization
	void Start () {
		gameInstance = GetComponentInParent<BreakerBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null && transform.localPosition.y + transform.parent.position.y < -1 * gameInstance.visibleRows / 2) {
			GameObject.Destroy (gameObject);
		}
	}
}
