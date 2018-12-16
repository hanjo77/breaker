using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour {

	public float speed = 1;
	public float minimalBatGap = 2;
	public GameObject otherBat;

	private Vector3 horizontalMotion;
	private Vector3 verticalMotion;
	private List<bool> canMove = new List<bool> { true, true };

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0, -4, 0);
		otherBat = GameObject.Instantiate (otherBat);
		otherBat.transform.SetParent (transform);
		otherBat.transform.localPosition = new Vector3 (0, this.minimalBatGap, 0);
	}
	
	// Update is called once per frame
	void Update () {
		float horizontalSpeed = Input.GetAxis ("Horizontal");
		if ((canMove[0] || horizontalSpeed > 0) && (canMove[1] || horizontalSpeed < 0)) {
			horizontalMotion = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			transform.position += horizontalMotion * speed * Time.deltaTime;
			canMove = new List<bool> { true, true };
		}
		verticalMotion = new Vector3 (0, Input.GetAxis ("Vertical"), 0);
		Vector3 newPos = otherBat.transform.localPosition + verticalMotion * speed * Time.deltaTime;
		if (newPos.y > minimalBatGap && newPos.y < 10) {
			otherBat.transform.localPosition = newPos;
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
