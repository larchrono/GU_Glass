using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextForSliderValue : MonoBehaviour {

	UnityEngine.UI.Text myText;

	// Use this for initialization
	void Awake () {
		myText = GetComponent<UnityEngine.UI.Text> ();
	}

	public void SetTextFromFloat(float src){
		myText.text = src.ToString ("F1");
	}
}
