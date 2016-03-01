using UnityEngine;
using System.Collections;

public class GhostHunter : InteractableObject {

	//moving speed
	float speed = .25f;
	public float FOVAngle = 110f;
	public Animator anim;
	public GameObject player;
	public bool lookingLeft, lookingRight, lookingUp, lookingDown;
	//ai logic
	public bool canSeePlayer;
	public Vector3 lastSeenPosition;
	public Vector2 direction; //of player

	// Use this for initialization
	public override void Start () {
		base.Start ();
		name = "Ghost Hunter";
		objType = type.GhostHunter;
		anim = GetComponent<Animator> ();
		canSeePlayer = false;
		lookingRight = true; //default true
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		if (canSeePlayer) {
			Debug.Log ("I can see you");
			ChasePlayer ();
		} else { //wonder mode
			
		}
	}

	void ChasePlayer(){
		transform.position += lastSeenPosition * speed * Time.deltaTime;
	}

	void OnTriggerStay2D(Collider2D col){
		//if player is within collider
		if (col.gameObject == player) {
			canSeePlayer = false; //just becuase player is in radius doesn't mean he can be seen
			//get direction and angle of player
			direction = col.transform.position - transform.position;
			float angle = 0f;
			if(lookingRight)
				angle = Vector2.Angle (direction, Vector2.right);
			if(lookingLeft)
				angle = Vector2.Angle (direction, Vector2.left);
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
