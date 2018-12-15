using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerBehaviour : MonoBehaviour {

	public List<TextAsset> levels;
	public GameObject bat;
	public GameObject ball;
	public GameObject wall;
	public GameObject brick;
	public Material[] materials;
	public GUIText scoreTextField;
	public GUIText livesTextField;
	public int initialLives = 5;

	private int currentLevel = 0;
	private int score = 0;
	private float topY;
	private GameObject playBat;
	private GameObject playBall;

	private void DrawLevel(string levelString) {
		string[] levelLines = levelString.Split('\n');
		for (int lineNr = 0; lineNr < levelLines.Length; lineNr++) {
			for (int charNr = 0; charNr < levelLines [lineNr].Length; charNr++) {
				char c = levelLines [lineNr] [charNr];
				GameObject tile = null;
				Renderer rend;
				if (c == 'w') {
					tile = GameObject.Instantiate (wall);
				}
				int j;
				if (int.TryParse(c.ToString(), out j)) {
					tile = GameObject.Instantiate (brick);
					rend = tile.GetComponent<Renderer> ();
					rend.material = materials[j];
				}
				if (tile != null) {
					tile.transform.position = new Vector3 (charNr - 7.5f, levelLines.Length - (lineNr + 5), 0);
					tile.transform.SetParent (this.transform);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		playBat = GameObject.Instantiate (bat);
		playBall = GameObject.Instantiate (ball);
		playBall.transform.position = playBat.transform.position + new Vector3(0, 2, 0);
		playBat.transform.SetParent(transform.parent);
		playBall.transform.SetParent(transform);
		DrawLevel (levels [currentLevel].text);
	}
	
	// Update is called once per frame
	void Update () {
		if (playBall.transform.position.y - transform.position.y > 2 - transform.position.y) {
			topY = playBall.transform.position.y - transform.position.y;
			float dist = (2 - transform.position.y) - topY;
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + dist, transform.position.z), 5 * Time.fixedDeltaTime);
		}
	}
}
