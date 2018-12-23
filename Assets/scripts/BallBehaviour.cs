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
	private BreakerBehaviour gameInstance;
	private BatBehaviour bat;
	private bool isWaiting;
	private Renderer myRenderer;

	private bool _isVisible = true;
	public bool isVisible {
		get {
			return _isVisible;
		}
		set {
			_isVisible = value;
			myRenderer.enabled = value;
			GetComponent<Collider> ().isTrigger = !_isVisible;
		}
	}

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<Renderer> ();
		gameInstance = GetComponentInParent<BreakerBehaviour> ();
		bat = GameObject.FindGameObjectsWithTag("Bat")[0].GetComponent<BatBehaviour>();
		lastUpdate = Time.fixedTime;
		Reset ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!isWaiting && isVisible) {
			Vector3 newPos = transform.position + direction * speed * Time.fixedDeltaTime;
            newPos.z = 0;
            transform.position = newPos;
            if (transform.position.y < -1 * gameInstance.visibleRows / 2) {
				gameInstance.lives--;
				isWaiting = true;
				StartCoroutine (WaitAfterDeath ());
				direction *= -1;
			}

			transform.RotateAround (transform.position, Vector3.Cross (direction, new Vector3 (0, 0, 1)), 4);

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
	}

	public void Reset() {
		speed = initialSpeed;
		direction = new Vector3(0, -1, 0);
	}

	IEnumerator WaitAfterDeath()
	{
		yield return new WaitForSeconds(2);
		isWaiting = false;
	}

	void OnTriggerEnter (Collider col) {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject otherObject = collision.collider.gameObject;
		bool isVerticalCollision = Mathf.Abs (transform.position.x - otherObject.transform.position.x) < Mathf.Abs (transform.position.y - otherObject.transform.position.y);

		if (otherObject.tag != "Bat" || otherObject.transform.position.y < transform.position.y) {
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
				BrickBehaviour brick = otherObject.GetComponent<BrickBehaviour> ();
				if (brick.brickType == '<') {
					bat.Shrink ();
				} else if (brick.brickType == '>') {
					bat.Grow ();
				} else if (brick.brickType == 'x') {
					bat.DeleteOtherBat ();
				} else if (brick.brickType == 'o') {
					gameInstance.score += gameInstance.optionScore;
				} else if ("RBGY".IndexOf(brick.brickType.ToString()) > -1) {
					if (brick.brickType == 'R') {
						gameInstance.UpdateBonus(BonusPanelBehaviour.ButtonColor.red);
					} else if (brick.brickType == 'G') {
						gameInstance.UpdateBonus(BonusPanelBehaviour.ButtonColor.green);
					} else if (brick.brickType == 'B') {
						gameInstance.UpdateBonus(BonusPanelBehaviour.ButtonColor.blue);
					} else if (brick.brickType == 'Y') {
						gameInstance.UpdateBonus(BonusPanelBehaviour.ButtonColor.yellow);
					}
				}
				gameInstance.score += brick.score;
				GameObject.Destroy (otherObject);
			}
		}
	}
}
