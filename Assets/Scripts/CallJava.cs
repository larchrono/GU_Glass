using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CallJava : MonoBehaviour {
	//com.unity3d.player.UnityPlayerActivity

	public static event EventHandler<OrienArgs> recieveOrientation;

	public static CallJava current;

	public float Orien_Z { get; set;}
	public float Orien_X { get; set;}
	public float Orien_Y { get; set;}

	AndroidJavaObject mainActivity;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {

		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass jc = new AndroidJavaClass("sensor.plugin.chrono.bt200plugin.Sensors" );
		mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		#endif
	}

	// Update is called once per frame
	void Update () {

		#if UNITY_ANDROID && !UNITY_EDITOR
		Orien_Z = mainActivity.Call<float> ("GetOrientationZ");
		Orien_X = mainActivity.Call<float> ("GetOrientationX");
		Orien_Y = mainActivity.Call<float> ("GetOrientationY");

		if(recieveOrientation != null){
			recieveOrientation.Invoke(this, new OrienArgs(){O_X = Orien_X , O_Y = Orien_Y , O_Z = Orien_Z});
		}

		#endif
	}

}
