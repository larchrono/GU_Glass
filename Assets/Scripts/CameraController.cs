using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController current;

	public Camera mainCamera;
	public InputField textOrien;
	public InputField textTrimming;

	public Slider sliderCameraHeight;
	public Slider sliderFieldofView;
	public Slider sliderRollCorrection;

	public Text cameraRotateData;
	public Text devieceRawOrientation;

	//Should Camera Move to
	private float nowX  = 0;
	private float nowY  = -5;
	private float viewHeight = 0;

	//Camera parameter
	float rotateRate = 1.0f;

	//校正正視角度
	public float trimming = 1;
	//校正歪頭角度
	public float rollCorrection = 0;

	//detect the north direction compass in real world
	public float initOrientDegree = 0;

	public float nowAzimuth;

	ScreenOrientation originOrientation;
	enum Mode
	{
		Phone,
		Glass
	}
	Mode appMode;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {
		//when orientation raw data recieve
		CallJava.recieveOrientation += OnViewChanged;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		originOrientation = Screen.orientation;
		if (originOrientation == ScreenOrientation.Portrait) {
			appMode = Mode.Phone;
		} else {
			appMode = Mode.Glass;
		}

		/*
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		*/

		initOrientDegree = PlayerPrefs.GetFloat ("Orien", 0f);
		trimming = PlayerPrefs.GetFloat ("Trimming", 1f);
		textOrien.text = initOrientDegree.ToString ();
		textTrimming.text = trimming.ToString ();
		sliderCameraHeight.value = PlayerPrefs.GetFloat ("CameraHeight", 6);
		sliderFieldofView.value = PlayerPrefs.GetFloat ("FieldofView", 60);
		sliderRollCorrection.value = PlayerPrefs.GetFloat ("RollCorrection", 0);
	}

	// Update is called once per frame
	void Update () {
		//this.transform.position = new Vector3 (nowX, 0, nowY);
		//this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (nowX, viewHeight, nowY), 0.1f);

		cameraRotateData.text = mainCamera.transform.rotation.eulerAngles.ToString ();
	}

	void OnViewChanged(object sender, OrienArgs args){

		//new verson, only for azimuth
		devieceRawOrientation.text = "( " + args.O_X + " , " + args.O_Z + " , " + args.O_Y + " )";

		nowAzimuth = args.O_Z;

		return;

		/*
		if (appMode == Mode.Glass || (appMode == Mode.Phone && originOrientation == ScreenOrientation.Portrait)) {
			float filter_Y = args.O_Y;
			if (filter_Y < trimming && filter_Y > -trimming) {
				filter_Y = 0;
			}
			this.transform.rotation = Quaternion.Euler ((args.O_X + 90) * rotateRate, args.O_Z - initOrientDegree, filter_Y);
		} else if (appMode == Mode.Phone &&  originOrientation == ScreenOrientation.Landscape) {
			float filter_Y = args.O_X;
			if (filter_Y < trimming && filter_Y > -trimming) {
				filter_Y = 0;
			}
			this.transform.rotation = Quaternion.Euler ((-args.O_Y + 90) * rotateRate, args.O_Z - initOrientDegree + 90 , filter_Y);
		}
		*/
	}

	public void UseNowAzimuth(){
		textOrien.text = "" + nowAzimuth;
	}

	public void SetInitOrient(string src){
		float temp = 0;
		float.TryParse (src, out temp);

		initOrientDegree = temp;
		transform.rotation = Quaternion.Euler (new Vector3 (0 , -initOrientDegree, rollCorrection));
		PlayerPrefs.SetFloat("Orien", initOrientDegree);
	}

	public void SetTrimming(string src){
		float temp = 0;
		float.TryParse (src, out temp);

		trimming = temp;
		PlayerPrefs.SetFloat("Trimming", trimming);
	}

	public void SetCameraHeight(float src){
		transform.position = new Vector3 (transform.position.x, src, transform.position.z);
		PlayerPrefs.SetFloat ("CameraHeight", src);
	}

	public void SetFieldOfView(float src){
		mainCamera.fieldOfView = src;
		PlayerPrefs.SetFloat ("FieldofView", src);
	}

	public void SetRollCorrection(float src){
		
		rollCorrection = src;
		transform.rotation = Quaternion.Euler (new Vector3 (0 , -initOrientDegree, rollCorrection));
		PlayerPrefs.SetFloat ("RollCorrection", src);
	}

	public float Now_X { get { return nowX; } }
	public float Now_Y { get { return nowY; } }
	public float Now_Z { get { return viewHeight; } }
}
