using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenBehaviour : MonoBehaviour {

	public Button startButton;
	public 

	// Use this for initialization
	void Start () {
		startButton.onClick.AddListener(() => StartButtonClicked());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartButtonClicked() {
		SceneManager.LoadScene("game");
	}
}
