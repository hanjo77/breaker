using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour {

	public float speed = 1;

	private Vector3 motion;
	private List<bool> canMove = new List<bool> { true, true };

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0, -4, 0);
	}
	
	// Update is called once per frame
	void Update () {
		float horizontalSpeed = Input.GetAxis ("Horizontal");
		if ((canMove[0] || horizontalSpeed > 0) && (canMove[1] || horizontalSpeed < 0)) {
			motion = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			transform.position += motion * speed * Time.deltaTime;
			canMove = new List<bool> { true, true };
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject otherObject = collision.collider.gameObject;

		if (otherObject.tag == "Wall") {
			if (otherObject.transform.position.x < transform.position.x) {
				canMove = new List<bool> { false, true };
			} else {
				canMove = new List<bool> { true, false };
			}
		}
	}
}
