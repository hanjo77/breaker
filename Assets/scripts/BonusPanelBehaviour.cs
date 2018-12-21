using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPanelBehaviour : MonoBehaviour {

	public GameObject redButton;
	public GameObject greenButton;
	public GameObject blueButton;
	public GameObject yellowButton;

	public enum ButtonColor {red, green, blue, yellow};
	private List<GameObject> buttons;

	// Use this for initialization
	void Start () {
		buttons = new List<GameObject> () {
			redButton,
			greenButton,
			blueButton,
			yellowButton
		};

		Reset ();
	}

	public void SwitchButton(ButtonColor color) {
		if (buttons[(int)color] != null) {
			buttons[(int)color].GetComponent<Image>().enabled = !buttons[(int)color].GetComponent<Image>().enabled;
		}
	}

	public void Reset() {
		buttons.ForEach (button => {
			button.GetComponent<Image>().enabled = false;
		});
	}

	public bool IsBonusActive() {
		return !buttons.Any (button => !button.GetComponent<Image>().enabled);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
