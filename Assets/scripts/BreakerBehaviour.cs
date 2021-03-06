﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BreakerBehaviour : MonoBehaviour {

	[System.Serializable]
	public class Tile
	{
		public char key;
		public GameObject tileObject;
		public float rotation;
		public Material material;
	}
 
	public List<TextAsset> levels;
	public GameObject bat;
	public GameObject ball;
	public Tile[] tiles;

	public GameObject brick;
	public BonusPanelBehaviour bonusPanel;
	public Material[] materials;
	public Text scoreTextField;
	public Text livesTextField;
	public float scrollSpeed = 2;
	public int columns = 16;
	public int initialLives = 5;
	public int optionScore = 50;
	public int visibleRows = 20;

	private int currentLevel = 0;
	private int currentRow = 0;
	private int lastRow = 0;
	private float topY;
	private GameObject playBat;
	private GameObject playBall;
	private string[] levelLines;

	private int _score;
	public int score {
		get {
			return _score;
		}
		set {
			_score = value;
			scoreTextField.text = _score.ToString();
		}
	}

	private int _lives;
	public int lives {
		get {
			return _lives;
		}
		set {
			_lives = value;
			if (_lives < 0) {
				SceneManager.LoadScene ("game-over");
			} else {
				livesTextField.text = _lives.ToString();
			}
		}
	}

	private bool _isWarping = false;
	public bool isWarping {
		get {
			return _isWarping;
		}
		set {
			_isWarping = value;
			playBall.GetComponent<BallBehaviour>().isVisible = !value;
		}
	}

	public void CheckWarp(WarpBehaviour warp) {
		if (isWarping && Math.Abs(warp.transform.position.x - playBall.transform.position.x) < 1) {
			playBall.transform.position = new Vector3(warp.transform.position.x, warp.transform.position.y, 0);
			isWarping = false;
		}
	}

	public void UpdateBonus(BonusPanelBehaviour.ButtonColor buttonColor) {
		bonusPanel.SwitchButton (buttonColor);
		if (bonusPanel.IsBonusActive ()) {
			bonusPanel.Reset ();
			lives++;
		}
	}

	private void DrawLevel() {
		int startRow = levelLines.Length - visibleRows + currentRow;
		for (int lineNr = startRow; lineNr < lastRow; lineNr++) {
			for (int charNr = 0; charNr < levelLines [lineNr].Length; charNr++) {
				char c = levelLines [lineNr] [charNr];
				GameObject tile = null;
				Renderer rend;
				var tileEntries = tiles.Where<Tile> (entry => entry.key == c);
				if (tileEntries.Any ()) {
					Tile tileEntry = tileEntries.First ();
					tile = GameObject.Instantiate (tileEntry.tileObject);
					tile.transform.rotation = Quaternion.Euler(0, 0, tileEntry.rotation);
					tile.GetComponent<BrickBehaviour> ().brickType = tileEntry.key;
					rend = tile.GetComponent<Renderer> ();
					rend.material = tileEntry.material;
				}
				if (tile != null) {
					tile.transform.SetParent (transform);
					tile.transform.localPosition = new Vector3 (charNr - 7.5f, levelLines.Length - (lineNr + (visibleRows / 2)), tile.transform.position.z);
				}
			}
		}
		lastRow = startRow;
	}

	// Use this for initialization
	void Start () {
		Camera cam = GameObject.Find("cam").GetComponent<Camera> ();
		visibleRows = (int)Math.Ceiling((double)(Screen.height / (Screen.width / columns))) + 2;
		float currentAspect = (float) Screen.width / (float) Screen.height;
		cam.orthographicSize = columns / 2 / currentAspect;
		Debug.Log (cam.orthographicSize);
		Debug.Log (Screen.width + " x " + Screen.height);
		levelLines = levels [currentLevel].text.Split('\n');
		lastRow = levelLines.Length;
		lives = initialLives;
		score = 0;
		playBat = GameObject.Instantiate (bat);
		playBall = GameObject.Instantiate (ball);
		playBat.transform.SetParent(transform.parent);
		playBat.transform.position = new Vector3 (playBat.transform.position.x, -1 * ((visibleRows / 2) - 2), playBat.transform.position.z);
		playBall.transform.SetParent(transform);
		playBall.transform.position = playBat.transform.position + new Vector3(0, 2, 0);
		DrawLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isWarping) {
			topY += scrollSpeed;
			float dist = (scrollSpeed - transform.position.y) - topY;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + dist, transform.position.z), 10 * Time.fixedDeltaTime);
		} else if (playBall.transform.position.y - transform.position.y > 2 - transform.position.y) {
			topY = playBall.transform.position.y - transform.position.y;
			float dist = (scrollSpeed - transform.position.y) - topY;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + dist, transform.position.z), 10 * Time.fixedDeltaTime);
		}
		if ((int)transform.position.y != currentRow) {
			currentRow = (int)transform.position.y;
			DrawLevel ();
		}
	}
}
