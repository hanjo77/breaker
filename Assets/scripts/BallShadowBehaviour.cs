using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShadowBehaviour : MonoBehaviour {

	public float shadowFadeSpeed = 3;

	private Color color;

	// Use this for initialization
	void Start () {
		color = gameObject.GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		color.a -= Time.deltaTime * shadowFadeSpeed;
		gameObject.GetComponent<Renderer> ().material.color = color;
		transform.localScale -= Vector3.one * Time.deltaTime * shadowFadeSpeed;
		if (color.a <= 0) {
			GameObject.Destroy (gameObject);
		}
	}
}
