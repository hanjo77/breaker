using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour {

	public float initialSpeed = 8;
	public float acceleration = 0;
	public GameObject ballShadowObject;
	public float shadowInterval = 10;

	private float lastUpdate;
	private float speed;
	private Vector3 direction;

	// Use this for initialization
	void Start () {
		lastUpdate = Time.fixedTime;
		Reset ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.position += direction * speed * Time.fixedDeltaTime;
		if (transform.position.y < -6) {
			direction *= -1;
		}

		transform.RotateAround (transform.position, Vector3.Cross (direction, new Vector3 (0, 0, 1)), 4);

		Debug.Log (Time.fixedTime);
		Debug.Log (Time.fixedTime - lastUpdate);
		if (Time.fixedTime - lastUpdate > shadowInterval) {
			lastUpdate = Time.fixedTime;
			GameObject shadow = GameObject.Instantiate (ballShadowObject);
			shadow.transform.SetParent (transform.parent);
			shadow.transform.localPosition = transform.localPosition;
			shadow.transform.localRotation = transform.localRotation;
			Color c = shadow.GetComponent<Renderer> ().material.color;
			c.a = .5f;
			shadow.GetComponent<Renderer> ().material.color = c;
		}
	}

	public void Reset() {
		speed = initialSpeed;
		direction = new Vector3(0, -1, 0);
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject otherObject = collision.collider.gameObject;
		bool isVerticalCollision = Mathf.Abs (transform.position.x - otherObject.transform.position.x) < Mathf.Abs (transform.position.y - otherObject.transform.position.y);

		if (isVerticalCollision) {
			if (transform.position.y < otherObject.transform.position.y) {
				direction = new Vector3 (direction.x, Mathf.Abs (direction.y) * -1, direction.z).normalized;
			} else {
				direction = new Vector3 (direction.x, Mathf.Abs (direction.y), direction.z).normalized;
			}
		} else {
			if (transform.position.x < otherObject.transform.position.x) {
				direction = new Vector3(Mathf.Abs(direction.x) * -1, direction.y, direction.z).normalized;
			} else {
				direction = new Vector3(Mathf.Abs(direction.x), direction.y, direction.z).normalized;
			}
		}

		if (otherObject.tag == "Bat") {
			direction += (transform.position - otherObject.transform.position).normalized;
			direction.Normalize ();
			direction += new Vector3(0, .8f, 0);
			direction.Normalize ();
			speed += acceleration;
		}
			
		if (otherObject.tag == "Brick") {
			GameObject.Destroy (otherObject);
		}
	}
}
