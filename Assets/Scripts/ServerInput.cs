using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInput : MonoBehaviour {

	public GameObject target;
	public GameObject bullet;

	float max_left = -10;
	float max_right = 10;
	float max_near = 0;
	float max_far = 8;

	float speed = 10.0f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.A)) {
			if(target.transform.position.x >= max_left)
				target.transform.Translate (-speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)) {
			if(target.transform.position.x <= max_right)
				target.transform.Translate (speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey (KeyCode.W)) {
			if(target.transform.position.z <= max_far)
				target.transform.Translate (0, 0, speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			if(target.transform.position.z >= max_near)
				target.transform.Translate (0, 0, -speed * Time.deltaTime);
		}
		if (Input.GetButtonDown ("Fire1")) {
			GameObject temp = Instantiate (bullet);
			temp.transform.position = target.transform.position;

		}
	}


}
