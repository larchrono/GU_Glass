using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController current;
	public GameObject targetObject;

	float trimming = 1;

	//set rect and position in Unity
	//in unity (0,0) is left button , but kinect is left top
	float x_r;
	float y_r;
	float d_r;
	int unity_rect_width;
	int unity_rect_height;
	float defalutX;
	float defalutY;

	//For user height
	int unity_camera_height;
	float defalutZ;
	//int depthGround = 30;
	//float depthMoveRate = 1.0f;


	//Kinect Depth Image Size
	float maxX = 512f;
	float maxY = 424f;
	float maxDepth = 255f;

	//Should Camera Move to
	private float nowX  = 0;
	private float nowY  = -5;
	private float viewHeight = 0;

	//Camera parameter
	float rotateRate = 1.0f;
	//detect the north direction compass in real world
	float initOrientDegree;

	//For background create
	//private WebCamTexture mCamera;
	//public GameObject backGround;
	//public Texture mask;

	//thread Async Invoke message


	void Awake(){
		//Input.location.Start();
		//Input.compass.enabled = true;

		current = this;
	}

	// Use this for initialization
	void Start () {

		//When camera raw data receive
		TCPClient.recievePosition += OnPositionChanged;
		//when orientation raw data recieve
		CallJava.recieveOrientation += OnViewChanged;

		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;

		//mCamera = new WebCamTexture ();
		//backGround.GetComponent<Renderer>().material.mainTexture = mCamera;
		//mCamera.Play ();

		//To do this let two camera merged
		//this.gameObject.SetActive (false);
		//this.gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		//this.transform.position = new Vector3 (nowX, 0, nowY);
		//this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (nowX, viewHeight, nowY), 0.1f);
	}

	public void SetPosition(float x, float y, float z){
		nowX = defalutX + (x / maxX) * unity_rect_width;
		nowY = defalutY - (y / maxY) * unity_rect_height;
		viewHeight = defalutZ + (z / maxDepth) * unity_camera_height;
	}

	void OnPositionChanged(object sender, OrienArgs args){
		SetPosition (args.O_X, args.O_Y, args.O_Z);
	}

	void OnViewChanged(object sender, OrienArgs args){
		
		float filter_Y = args.O_Y;
		if (filter_Y < trimming && filter_Y > -trimming) {
			filter_Y = 0;
		}
		this.transform.rotation = Quaternion.Euler ((args.O_X + 90) * rotateRate , args.O_Z - initOrientDegree, filter_Y);
	}


	public void SetInitOrient(string src){
		float temp;
		float.TryParse (src, out temp);
		initOrientDegree = temp;
	}

	public void SetObjectHeight(float height){
		float final = defalutZ + (height / maxDepth) * unity_camera_height;
		targetObject.transform.position = new Vector3 (0, final, 0);
	}

	public float Now_X { get { return nowX; } }
	public float Now_Y { get { return nowY; } }
	public float Now_Z { get { return viewHeight; } }
}
