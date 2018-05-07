using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

	public GameObject iconPrefab;

	int touchScreenWidth = 640;
	int touchScreeHeight = 480;

	int goundWidth = 40;
	int goundHeight = 20;
	float iconHeight = 15.5f;

	//To run Method in Unity main Thread
	delegate void NetworkThreadWork();
	NetworkThreadWork functionCallback = null;

	// Use this for initialization
	void Start () {
		MyTcpClient.recieveTouchEvent += OnRecieveTouch;

	}
	
	// Update is called once per frame
	void Update () {
		if (functionCallback != null) {
			functionCallback.Invoke ();
			functionCallback = null;
		}
	}

	void OnRecieveTouch(ArgsPosition[] args){
		Debug.Log ("Recieve data and Trigger Event");

		functionCallback += delegate {
			for (int i = 0; i < args.Length; i++) {
				GameObject temp = Instantiate (iconPrefab , new Vector3 (RectX (args [i].x), iconHeight , RectZ (args [i].y)), Quaternion.identity);

				Debug.Log("x:" + RectX (args [i].x) + " , y:" + RectZ (args [i].y));
			}
		};
	}

	//In Unity , (0,0) is at left bottom
	float RectX(int src){
		return ((src * 1.0f) / touchScreenWidth) * goundWidth;
	}
	float RectZ(int src){
		//In openCV Y is negative
		return ((src * 1.0f) / touchScreeHeight) * goundHeight - goundHeight;
	}
}
