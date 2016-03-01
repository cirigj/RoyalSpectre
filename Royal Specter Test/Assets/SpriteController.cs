using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {

	Transform myTF;
	SpriteRenderer mySR;

	public float speed = 1;
	public float backBound = 0;
	public float frontBound = 4;

	// Use this for initialization
	void Start () {
		myTF = GetComponent<Transform>();
		//mySR = GetComponent<SpriteRenderer>;
	}
	
	// Update is called once per frame
	void Update () {

		movement ();

	}

	void movement(){
		Vector3 newPos = myTF.position;

		if (Input.GetKey (KeyCode.A)) {
			newPos.x += speed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.D)) {
			newPos.x -= speed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.W)) {
			newPos.z -= speed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.S)) {
			newPos.z += speed * Time.deltaTime;
		}

		if (newPos.z > frontBound) {
			newPos.z = frontBound;
		}
		if (newPos.z < backBound) {
			newPos.z = backBound;
		}

		myTF.position = newPos;
	}
}
