using UnityEngine;
using System.Collections;

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
	//spin logic
	float spinTimer;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		name = "Ghost Hunter";
		objType = type.GhostHunter;
		anim = GetComponent<Animator> ();
		canSeePlayer = false;
		Look ("right"); //default true
		spinTimer = Time.time + 2f;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		//if player is in sight or ghost hunter is not currently close to the spot the ghost was last seen
		if (canSeePlayer || (!Mathf.Approximately(transform.position.x,lastSeenPosition.x) && !Mathf.Approximately(transform.position.z,lastSeenPosition.z))) {
			ChasePlayer ();
			spinTimer = Time.time + 2f; //if you can see player, keep resetting spintimer
		} else { //wonder mode
			if (spinTimer <= Time.time) {
				Look ("rand");
				spinTimer = Time.time + 2f;
			}
		}
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
			Debug.Log (rand);
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
	}

	void OnTriggerStay2D(Collider2D col){
		//if player is within collider
		if (col.gameObject == player) {
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
				RaycastHit2D hit = Physics2D.Raycast (transform.position, direction.normalized);
				//and there is nothing in the way
				if (hit.collider != null) {
					if (hit.collider.gameObject == player) {
						canSeePlayer = true;
						lastSeenPosition = player.transform.position;
					}
				}
			}
					
		}
	}

	//if player is not in radius of the ghost hunter
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject == player) {
			canSeePlayer = false;
		}
	}
}
