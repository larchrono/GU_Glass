using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutioController : MonoBehaviour {

	public static ResolutioController current;

	public int touchScreenWidth = 1920;
	public int touchScreenHeight = 1080;

	void Awake(){
		current = this;
	}

	void Start(){
		touchScreenWidth = 1920;
		touchScreenHeight = 1080;
		MyTcpClient.recieveResolution += OnResolutionChange;
	}
	
	void OnResolutionChange(int w,int h){
		if(w != 0 && h != 0){
			touchScreenWidth = w;
			touchScreenHeight = h;
		}
	}
}
