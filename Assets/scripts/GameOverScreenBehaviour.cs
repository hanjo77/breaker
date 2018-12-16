using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenBehaviour : MonoBehaviour {

	public Button retryButton;
	public Button endButton;

	// Use this for initialization
	void Start () {
		retryButton.onClick.AddListener(() => StartButtonClicked("game"));
		endButton.onClick.AddListener(() => StartButtonClicked("start"));
	}

	// Update is called once per frame
	void Update () {

	}

	void StartButtonClicked(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}