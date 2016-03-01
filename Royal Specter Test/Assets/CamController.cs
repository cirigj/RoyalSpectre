using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	public GameObject myTarget;
	private Transform tgtTF;
	private Transform myTF;
	private float speed;
	public float lockY = 2.25f;
	public float lockZ = 7;
	public float deadRange = 1;

	// Use this for initialization
	void Start () {
		tgtTF = myTarget.GetComponent<Transform> ();
		myTF = GetComponent<Transform> ();
		speed = myTarget.GetComponent<SpriteController> ().speed;

		Vector3 newPos = myTF.position;
		newPos.x = tgtTF.position.x;
		myTF.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	void Movement(){

		float diffX = 0;
		diffX = tgtTF.position.x - myTF.position.x;

		Vector3 newPos = myTF.position;

		if (diffX > deadRange) {
			newPos.x += speed * Time.deltaTime;
		} else if (diffX < -deadRange) {
			newPos.x -= speed * Time.deltaTime;
		}

		myTF.position = newPos;

	}
}
