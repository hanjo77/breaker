using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBehaviour : MonoBehaviour {

	public bool isWarpEntry;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isWarpEntry && transform.position.y < -1) {
			GetComponentInParent<BreakerBehaviour> ().CheckWarp (this);
		}
	}

	void OnTriggerEnter (Collider col) {
		BallBehaviour ball = col.gameObject.GetComponent<BallBehaviour> ();
		BreakerBehaviour game = GetComponentInParent<BreakerBehaviour> ();
		if (ball != null && game != null) {
			game.isWarping = isWarpEntry;
		}
	}
}
