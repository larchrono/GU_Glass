using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboardInput : MonoBehaviour {

	public GameObject baseShooter ;
	public GameObject baseBullet;

	GameObject shooter;

	float max_left = 0;
	float max_right = 40;
	float max_near = -20;
	float max_far = 0;

	float speed = 20.0f;
	Vector3 shhoterInitLoc = new Vector3(0, 15.5f,0);

	// Use this for initialization
	void Start () {
		shooter = Instantiate (baseShooter, shhoterInitLoc, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.A)) {
			if(shooter.transform.position.x >= max_left)
				shooter.transform.Translate (-speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)) {
			if(shooter.transform.position.x <= max_right)
				shooter.transform.Translate (speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey (KeyCode.W)) {
			if(shooter.transform.position.z <= max_far)
				shooter.transform.Translate (0, 0, speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			if(shooter.transform.position.z >= max_near)
				shooter.transform.Translate (0, 0, -speed * Time.deltaTime);
		}
		if (Input.GetButtonDown ("Fire1")) {
			Instantiate (baseBullet, shooter.transform.position, Quaternion.identity);
		}
	}


}
