using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

	public Text _debugOrien;

	private WebCamTexture backCamera;
	public RawImage rawimage;
	public ScreenOrientation nowOrientation;
	string backCamName;

	private bool useWebcam = true;

	float _imageRotate = 90;

	// Use this for initialization
	void Start () {
		nowOrientation = Screen.orientation;

		WebCamDevice[] devices = WebCamTexture.devices;
		backCamName = devices [0].name;
		Debug.Log (backCamName);
		backCamera = new WebCamTexture (backCamName, Screen.width, Screen.height, 30);
		rawimage.texture = backCamera;
		rawimage.material.mainTexture = backCamera;
		MyCameraPlay (backCamera);
		//backCamera.Play ();
		rawimage.rectTransform.sizeDelta = new Vector2 (Screen.height, Screen.width);
		rawimage.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, -90));

	}
	
	// Update is called once per frame
	void Update () {
		_debugOrien.text = Screen.orientation.ToString ();
		if (Screen.orientation != nowOrientation) {
			nowOrientation = Screen.orientation;
			ResolutionReSet ();
		}
	}

	void ResolutionReSet(){
		
		backCamera.Stop ();
		backCamera = new WebCamTexture (backCamName, Screen.width, Screen.height, 30);
		rawimage.texture = backCamera;
		rawimage.material.mainTexture = backCamera;
		MyCameraPlay (backCamera);
		//backCamera.Play ();
		RotationSet ();
	}

	void RotationSet(){
		if (Screen.orientation == ScreenOrientation.Portrait) {
			_imageRotate = -90;
			rawimage.rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
		} else if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			_imageRotate = 0;
			rawimage.rectTransform.sizeDelta = new Vector2 (Screen.height, Screen.width);
		} 
		rawimage.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, _imageRotate));
	}

	public void Initialize(){
		Debug.Log("Initialize");

		GUITexture BackgroundTexture = gameObject.AddComponent<GUITexture>();
		BackgroundTexture.pixelInset = new Rect(0,0,Screen.width,Screen.height);
		WebCamDevice[] devices = WebCamTexture.devices;
		string backCamName = devices[0].name;
		WebCamTexture CameraTexture = new WebCamTexture(backCamName,10000,10000,30);
		MyCameraPlay (backCamera);
		//CameraTexture.Play();
		BackgroundTexture.texture = CameraTexture;

	}

	void MyCameraPlay(WebCamTexture src){
		if (useWebcam) {
			src.Play ();
		}
	}

	public void WebcamTurn(bool val){
		useWebcam = val;
		gameObject.SetActive (useWebcam);
		if (useWebcam) {
			backCamera.Play ();
		} else {
			backCamera.Stop ();
		}
	}
}