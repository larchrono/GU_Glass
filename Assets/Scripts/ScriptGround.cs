using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptGround : MonoBehaviour {

	public Slider sliderTrans;

	// Use this for initialization
	void Start () {
		sliderTrans.value = PlayerPrefs.GetInt ("BGTrans", 255);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetGroundTrans(float src){
		byte a = System.Convert.ToByte (src);
		GetComponent<Renderer> ().material.color = new Color32 (0, 0, 0, a);
		PlayerPrefs.SetInt ("BGTrans", a);
	}
}
