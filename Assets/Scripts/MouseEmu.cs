using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEmu : MonoBehaviour {

	public GameObject myUser;

	float nowX;
	float nowY;

	Vector3 target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		nowX = Input.GetAxis ("Mouse X");
		nowY = Input.GetAxis ("Mouse Y");

		//Camera.main.ScreenToWorldPoint

		//Debug.Log (Input.mousePosition);
		//Debug.Log (nowX + " , " + nowY );


		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if ( Physics.Raycast (ray,　out hit,　100.0f)) {
			//StartCoroutine(ScaleMe(hit.transform));
			if(hit.collider.gameObject.tag == "Ground"){
				
				SmoothMove (new Vector3 (hit.point.x, 0.5f, hit.point.z));
				//Debug.Log ("position" + hit.point);
			}
		}
	}
	void OnCollisionEnter(Collision c)
	{
		// force is how forcefully we will push the player away from the enemy.
		float force = 3;

		Debug.Log ("hit enter");

		// If the object we hit is the enemy
		//if (c.gameObject.tag == "enemy")
		{
			// Calculate Angle Between the collision point and the player
			Vector3 dir = c.contacts[0].point - transform.position;
			// We then get the opposite (-Vector3) and normalize it
			dir = -dir.normalized;
			// And finally we add force in the direction of dir and multiply it by force. 
			// This will push back the player
			GetComponent<Rigidbody>().AddForce(dir*force);
		}
	}


	/*
	var pushDir = Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
	// If you know how fast your character is trying to move,
	// then you can also multiply the push velocity by that.
	// Apply the push
	body.velocity = pushDir * pushPower;
	*/


	void SmoothMove(Vector3 vect){
		myUser.transform.position = Vector3.Lerp(myUser.transform.position, vect, 0.25f);
	}
}
