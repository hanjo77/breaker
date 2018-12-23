using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour {

	public float speed = 1;
	public float minimalBatGap = 2;
	public GameObject otherBat;
	public int scaleDuration = 15;
	public int initialScale = 2;
	public float deleteDuration = 10;

	private float scaledTime;
	private bool isScaled;
    private FloatingJoystick joyStick;
	private Vector3 horizontalMotion;
	private Vector3 verticalMotion;
	private List<bool> canMove = new List<bool> { true, true };
	private float? otherBatDeletedTime;
    private BreakerBehaviour gameInstance;

	// Use this for initialization
	void Start () {
		ResetScale ();

        joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystick>();
        joyStick.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        joyStick.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);

        gameInstance = GameObject.Find("playground").GetComponent<BreakerBehaviour>();

		otherBat = GameObject.Instantiate (otherBat);
		otherBat.transform.SetParent (transform);
		otherBat.transform.localPosition = new Vector3 (0, this.minimalBatGap, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x <= -7) {
			transform.position = new Vector3 (-7, transform.position.y, transform.position.z);
			canMove = new List<bool> { false, true };
		} else if (transform.position.x >= 7) {
			transform.position = new Vector3 (7, transform.position.y, transform.position.z);
			canMove = new List<bool> { true, false };
		}

		float horizontalSpeed = Input.GetAxis("Horizontal") + Input.GetAxis("Horizontal") + joyStick.Horizontal;
		if ((canMove[0] || horizontalSpeed > 0) && (canMove[1] || horizontalSpeed < 0)) {
			horizontalMotion = new Vector3(Input.GetAxis("Horizontal") + joyStick.Horizontal, 0, 0);
			transform.position += horizontalMotion * speed * Time.deltaTime;
			canMove = new List<bool> { true, true };
		}
		verticalMotion = new Vector3 (0, Input.GetAxis ("Vertical") + joyStick.Vertical, 0);
		Vector3 newPos = otherBat.transform.localPosition + verticalMotion * speed * Time.deltaTime;
		if (newPos.y > minimalBatGap && newPos.y < gameInstance.visibleRows - 6) {
			otherBat.transform.localPosition = newPos;
		}

		if (isScaled && Time.fixedTime - scaledTime > scaleDuration) {
			ResetScale ();
		}

		if (otherBatDeletedTime != null && Time.fixedTime - otherBatDeletedTime > deleteDuration) {
			AddOtherBat ();
		}
	}

	void AddOtherBat() {
		otherBatDeletedTime = null;
		otherBat.GetComponent<Renderer> ().enabled = true;
		otherBat.GetComponent<Collider> ().isTrigger = false;
	}
		
	public void DeleteOtherBat() {
		otherBat.GetComponent<Renderer> ().enabled = false;
		otherBat.GetComponent<Collider> ().isTrigger = true;
		otherBatDeletedTime = Time.fixedTime;
	}

	public void ResetScale() {
		isScaled = false;
		Vector3 scale = transform.localScale;
		scale.x = initialScale;
		transform.localScale = scale;
	}

	public void Grow() {
		isScaled = true;
		Vector3 scale = transform.localScale;
		if (scale.x < 3) {
			scale.x += 1;
		}
		scaledTime = Time.fixedTime;
		transform.localScale = scale;
	}

	public void Shrink() {
		isScaled = true;
		Vector3 scale = transform.localScale;
		if (scale.x > 1) {
			scale.x -= 1;
		}
		scaledTime = Time.fixedTime;
		transform.localScale = scale;
	}
}
