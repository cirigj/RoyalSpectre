using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	public GameObject myTarget;
	private float speed = 1f;
	public float lockY = 2.25f;
	public float lockZ = 7;
	public float deadRange = 1;

	// Use this for initialization
	void Start () {
		Vector3 newPos = transform.position;
		newPos.x = myTarget.transform.position.x;
		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	void Movement(){

		float diffX = 0;
		diffX = myTarget.transform.position.x - transform.position.x;

		Vector3 newPos = transform.position;

		if (diffX > deadRange) {
			newPos.x += speed * Time.deltaTime;
		} else if (diffX < -deadRange) {
			newPos.x -= speed * Time.deltaTime;
		}

		transform.position = newPos;

	}
}
