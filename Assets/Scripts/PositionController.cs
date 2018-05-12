using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionController : MonoBehaviour {

	public GameObject ColliderCapsule;
	public Slider sliderCapsuleSize;
	public Slider sliderRealSpace;

	float capsuleSize = 3;
	float capsuleDelay = 0.8f;
	float realSpaceHeight = 30;

	//To run Method in Unity main Thread
	delegate void NetworkThreadWork();
	NetworkThreadWork functionCallback = null;

	// Use this for initialization
	void Start () {
		MyTcpClient.recievePositionEvent += OnRecievePosition;
		sliderCapsuleSize.value = PlayerPrefs.GetFloat ("CapsuleSize", 3);
		sliderRealSpace.value = PlayerPrefs.GetFloat ("RealSpaceHeight", 30);
	}
	
	// Update is called once per frame
	void Update () {
		if (functionCallback != null) {
			functionCallback.Invoke ();
			functionCallback = null;
		}
	}

	void OnRecievePosition(ArgsPosition[] args){
		functionCallback += delegate {
			for (int i = 0; i < args.Length; i++) {
				GameObject temp = Instantiate (ColliderCapsule , new Vector3 (RectX (args [i].x), RectY(args[i].z) , RectZ (args [i].y)), Quaternion.identity);
				temp.transform.localScale = new Vector3(capsuleSize,capsuleSize,capsuleSize);
				Destroy(temp, capsuleDelay);
			}
		};
	}

	float RectX(int src){
		return ((src * 1.0f) / GlobalVars.Depth_Width) * GlobalVars.GoundWidth;
	}
	float RectZ(int src){
		return ((src * 1.0f) / GlobalVars.Depth_Height) *  GlobalVars.GoundHeight - GlobalVars.GoundHeight;
	}
	float RectY(int src){
		return ((src * 1.0f) / GlobalVars.Depth_Color) *  realSpaceHeight;
	}

	public void SetCapsultSize(float value){
		capsuleSize = value;
		PlayerPrefs.SetFloat ("CapsuleSize", value);
	}

	public void SetRealSpaceHeight(float value){
		realSpaceHeight = value;
		PlayerPrefs.SetFloat ("RealSpaceHeight", value);
	}
}
