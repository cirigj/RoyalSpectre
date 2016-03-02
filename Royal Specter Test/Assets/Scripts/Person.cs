﻿using UnityEngine;
using System.Collections;

public class Person : InteractableObject {

	//if this person is currently the player
	public bool player;
	//bools for animations
	public bool walkingLeft, walkingRight, walkingUp, walkingDown, canSwitch;
	//moving speed
	public float speed = 1f;
	float cooldown;
	public Camera mainCam;
	public GhostHunter ghostHunter;

	//public Animator anim;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		name = "person";
		objType = type.Person;
		canSwitch = false;
		//anim = GetComponent<Animator> ();
		mainCam = Camera.main;
		try{
			ghostHunter.GetComponent<GhostHunter> ();
		} catch {
			Debug.Log ("Need reference to ghost hunter assigned in the inspector");
		}
	}

	//if player tries to talk
	public void Talk(){
		Debug.Log ("talking");
	}

	//if player wants to take control
	public void Control(Person otherPerson){
		//Debug.Log ("Switching " + gameObject.name + " with " + otherPerson.gameObject.name);
		//switch player var
		player = false;
		otherPerson.player = true;
		//switch up the trigger colliders
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		otherPerson.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		//set cooldown of switch
		cooldown = Time.time + 5f;
		//update camera
		mainCam.GetComponent<CamController>().myTarget = otherPerson.gameObject;
		//update ghost hunter
		ghostHunter.player = otherPerson.gameObject;
	}


	// Update is called once per frame
	void FixedUpdate () {
		//if player is controlling person
		if (player) {
			//moving and animation logic ----------------------------------------------
			if (Input.GetKey (KeyCode.W)) {
				walkingUp = true;
				//anim.SetBool ("walkingUp",true);
				//anim.SetBool ("idle", false);
				transform.position += Vector3.forward * speed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.A)) {
				walkingLeft = true;
				//anim.SetBool ("walkingLeft",true);
				//anim.SetBool ("idle", false);
				transform.position += Vector3.left * speed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.S)) {
				walkingDown = true;
				//anim.SetBool ("walkingDown",true);
				//anim.SetBool ("idle", false);
				transform.position += Vector3.back * speed * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.D)) {
				walkingRight = true;
				//anim.SetBool ("walkingRight",true);
				//anim.SetBool ("idle", false);
				transform.position += Vector3.right * speed * Time.deltaTime;
			}
			if (Input.GetKeyUp (KeyCode.W)) {
				//anim.SetBool ("walkingUp", false);
				//anim.SetBool ("idle", true);
				walkingUp = false;
			}
			if (Input.GetKeyUp (KeyCode.A)) {
				//anim.SetBool ("walkingLeft", false);
				//anim.SetBool ("idle", true);
				walkingLeft = false;
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				//anim.SetBool ("walkingDown", false);
				//anim.SetBool ("idle", true);
				walkingDown = false;
			}
			if (Input.GetKeyUp (KeyCode.D)) {
				//anim.SetBool ("walkingRight", false);
				//anim.SetBool ("idle", true);
				walkingRight = false;
			} // -----------------------------------------------------
		} else {
			//do npc idle stuff
		}
	}

	// trigger logic for talking and controlling -----------------------------------
	void OnTriggerEnter2D(Collider2D col){
		//Debug.Log ("Entered trigger");
	}

	void OnTriggerStay2D(Collider2D col){
		//for controlling other people
		if (Input.GetKey (KeyCode.Space) && col.gameObject.GetComponent<Person>() != null && cooldown <= Time.time) {
			Control (col.gameObject.GetComponent<Person> ());
		}
		//for talking to other people
		if (Input.GetKey (KeyCode.T) && col.gameObject.GetComponent<Person>() != null) {
			Talk ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		//Debug.Log ("Left Trigger");
	}
}
