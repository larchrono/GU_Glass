using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!DebugSystem.current.IsDebug) {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
