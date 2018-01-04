using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour {
	public float speed = 5;
	private Vector3 moveTowardsPoint;

	// Use this for initialization
	void Start () {
		GetComponent<Animator> ().SetInteger ("state", 1);
	}
	
	// Update is called once per frame
	void Update () {
		var pos = new Vector3 (Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
		moveTowardsPoint = pos;	
		MoveTowards ();
	}
	public void MoveTowards() {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, moveTowardsPoint, step/10);
		if(transform.position.magnitude == moveTowardsPoint.magnitude) {
			GetComponent<Animator> ().SetInteger ("state", 2);
			Invoke ("Idle", 0.35f);
		}
	}

	private void Idle() {
		GetComponent<Animator> ().SetInteger ("state", 0);
	}
		
}
