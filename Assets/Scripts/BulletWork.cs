using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWork : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		string message = "1";
		float _x = Mathf.Clamp (transform.position.x, 0, GlobalVars.GoundWidth);
		float _y = Mathf.Clamp (transform.position.z, 0 - GlobalVars.Depth_Height, 0);

		message += "," + ReverseRectX (_x) + "," + ReverseRectZ (_y);
		//Debug.Log ("pos:" + message);
		MyTcpClient.curent.writeSocket (message);
		Destroy(gameObject,0.5f);
	}

	//In Unity , (0,0) is at left bottom
	int ReverseRectX(float src){
		return Mathf.FloorToInt( src / GlobalVars.GoundWidth * ResolutioController.current.touchScreenWidth);
	}
	int ReverseRectZ(float src){
		return Mathf.FloorToInt ((src + GlobalVars.GoundHeight) / GlobalVars.GoundHeight * ResolutioController.current.touchScreenHeight);
	}
}
