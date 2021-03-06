﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostHunter : InteractableObject {

	//moving speed
	float speed = .6f;
	public float FOVAngle = 110f;
	public Animator anim;
	public GameObject player;
	public bool lookingLeft, lookingRight, lookingUp, lookingDown;
	//ai logic
	public bool canSeePlayer;
	public Vector3 lastSeenPosition;
	public Vector3 direction; //of player
	public bool sawBodySwitch;
	public bool bodySwitch;
	public bool playerTransition; //bool if saw player tranistion rooms, is reset upon enering room player entered
	public GameObject currentRoom;
	//spin logic
	float spinTimer;
	//waypoint logic
	public List<Transform> waypointList;
	Vector3 closestWaypointPosition;
	public bool knowPlayerRoom;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		name = "Ghost Hunter";
		objType = type.GhostHunter;
		anim = GetComponent<Animator> ();
		canSeePlayer = false;
		Look ("right"); //default direction
		spinTimer = Time.time + 2f;
		bodySwitch = false;
		sawBodySwitch = false;
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (playerTransition) { //if player transitioned rooms //!CanSeePlayer
			//find closest waypoint
			Vector3 point = getClosestWaypoint ();
			transform.position = Vector3.MoveTowards (transform.position, point, speed * Time.deltaTime);
			lastSeenPosition = transform.position; //reset last seen position to current position so below if is false
		}
		//if player is in sight or ghost hunter is not currently close to the spot the ghost was last seen
		else if (canSeePlayer || (!Mathf.Approximately (transform.position.x, lastSeenPosition.x) && !Mathf.Approximately (transform.position.z, lastSeenPosition.z))) {
			ChasePlayer ();
			spinTimer = Time.time + 2f; //if you can see player, keep resetting spintimer
		} 
		else { //spin mode
			if (spinTimer <= Time.time) {
				Look ("rand");
				spinTimer = Time.time + 2f;
			}
		}
		//if the player switched bodies and the ghost hunter saw it, set bool to true
		if (canSeePlayer && bodySwitch) {
			sawBodySwitch = true;
			if(player.GetComponent<Person> () != null)
				player.GetComponent<Person> ().suspicionLevel = 100;
		}
		if (!canSeePlayer) {
			bodySwitch = false; //if you cant see the player, reset bodySwitch bools
			sawBodySwitch = false;
		}
		if (currentRoom.activeSelf == false && gameObject.GetComponentInChildren<SpriteRenderer> ().enabled) {
			gameObject.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		} else if (currentRoom.activeSelf == true && gameObject.GetComponentInChildren<SpriteRenderer> ().enabled == false) {
			gameObject.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		}
	}

	Vector3 getClosestWaypoint(){
		Transform bestTarget = null;
		float closestDist = Mathf.Infinity;
		foreach (Transform t in waypointList) {
			float distance = (t.position - lastSeenPosition).sqrMagnitude;
			if (distance < closestDist) {
				closestDist = distance;
				bestTarget = t;
			}
		}
		return bestTarget.position;
	}

	void Look(string str){
		if (str == "right") {
			lookingRight = true;
			lookingUp = false;
			lookingLeft = false;
			lookingDown = false;
		}
		if (str == "left") {
			lookingRight = false;
			lookingUp = false;
			lookingLeft = true;
			lookingDown = false;
		}
		if (str == "up") {
			lookingRight = false;
			lookingUp = true;
			lookingLeft = false;
			lookingDown = false;
		}
		if (str == "down") {
			lookingRight = false;
			lookingUp = false;
			lookingLeft = false;
			lookingDown = true;
		}
		if (str == "rand") {
			int rand = Random.Range (1, 5);
			switch (rand) {
			case 1:
				Look ("right");
				break;
			case 2:
				Look ("down");
				break;
			case 3:
				Look ("up");
				break;
			case 4:
				Look ("left");
				break;
			}
		}
	}

	void ChasePlayer(){
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, lastSeenPosition, step);
		//stay looking at player
		if (Mathf.Abs(direction.normalized.x) > Mathf.Abs(direction.normalized.z) && direction.normalized.x > 0)
			Look ("right");
		if (Mathf.Abs(direction.normalized.x) > Mathf.Abs(direction.normalized.z) && direction.normalized.x < 0)
			Look ("left");
		if (Mathf.Abs(direction.normalized.x) < Mathf.Abs(direction.normalized.z) && direction.normalized.z > 0)
			Look ("up");
		if (Mathf.Abs(direction.normalized.x) < Mathf.Abs(direction.normalized.z) && direction.normalized.x < 0)
			Look ("down");
	}

	void OnTriggerStay(Collider col){
		//if player is a ghost or if he saw the player switch bodies
		if (col.gameObject.GetComponent<Ghost>() != null || (col.gameObject == player && sawBodySwitch)) {
			canSeePlayer = false; //just becuase player is in radius doesn't mean he can be seen
			//get direction and angle of player
			direction = col.transform.position - transform.position;
			float angle = 0f;
			if(lookingRight)
				angle = Vector3.Angle (direction, Vector3.right);
			if(lookingLeft)
				angle = Vector3.Angle (direction, Vector3.left);
			if(lookingUp)
				angle = Vector3.Angle (direction, Vector3.forward);
			if(lookingDown)
				angle = Vector3.Angle (direction, Vector3.back);
			//and within the field of view
			if (angle < FOVAngle / 2) {
				//note Edit -> proj settings -> physics 2d -> queries start in collider must be OFF, otherwise raycast will hit itself
				RaycastHit hit;
				Physics.Raycast (transform.position, direction.normalized, out hit);
				//and there is nothing in the way
				if (hit.collider != null) {
					if (hit.collider.gameObject.GetComponent<Ghost>() != null || (col.gameObject == player && sawBodySwitch)) {
						canSeePlayer = true;
						lastSeenPosition = player.transform.position;
					}
				}
			}
		}
	}

	//if player is not in radius of the ghost hunter
	void OnTriggerExit(Collider col){
		if (col.gameObject == player) {
			canSeePlayer = false;
		}
	}
}
