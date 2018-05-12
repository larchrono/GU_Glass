using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSystem : MonoBehaviour {
	public static DebugSystem current;

	public delegate void ThreadWorkStore();
	public ThreadWorkStore threadWorkStores;

	public Text debugMsg;

	bool isDebug = true;

	void Awake(){
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (threadWorkStores != null) {
			threadWorkStores.Invoke ();
			threadWorkStores = null;
		}
	}

	public void ShowMessage(string str){
		if (debugMsg != null)
			debugMsg.text = str;
	}

	public bool IsDebug {
		set { isDebug = value ; }
		get { return isDebug ;}
	}
}
