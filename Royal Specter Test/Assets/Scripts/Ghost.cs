using UnityEngine;
using System.Collections;

public class Ghost : InteractableObject {

	//if player if in ghost form
	public bool ghostForm;
	//bools for animations
	public bool walkingLeft, walkingRight, walkingUp, walkingDown, canSwitch;
	//moving speed
	float speed = 1.5f;
	public float cooldown;
	public Camera mainCam;
	public GhostHunter ghostHunter;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		name = "ghost";
		objType = type.Ghost;
		//canSwitch = false;
		//anim = GetComponent<Animator> ();
		mainCam = Camera.main;
		try{
			ghostHunter.GetComponent<GhostHunter> ();
		} catch {
			Debug.Log ("Need reference to ghost hunter assigned in the inspector");
		}
		ghostForm = true;
	}

	void OnTriggerStay2D(Collider2D col){
		//for controlling other people
		if (ghostForm && Input.GetKey (KeyCode.Space) && col.gameObject.GetComponent<Person>() != null && cooldown <= Time.time) {
			Control (col.gameObject.GetComponent<Person> ());
		}
	}

	//if player wants to take control
	public void Control(Person otherPerson){
		Debug.Log (otherPerson.gameObject.name + " is now the player");
		//switch player var
		ghostForm = false;
		otherPerson.player = true;
		//switch up the trigger colliders
		otherPerson.gameObject.GetComponent<CircleCollider2D> ().enabled = true;
		//set cooldown of switch
		otherPerson.cooldown = Time.time + 2f;
		//update camera
		mainCam.GetComponent<CamController>().myTarget = otherPerson.gameObject;
		//update ghost hunter
		ghostHunter.player = otherPerson.gameObject;
		//set this game object false
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		//moving and animation logic ----------------------------------------------
		if (Input.GetKey (KeyCode.W)) {
			walkingUp = true;
			//anim.SetBool ("walkingUp",true);
			//anim.SetBool ("idle", false);
			//upperbounds check
			if((transform.position + Vector3.forward * speed * Time.deltaTime).z < 2)
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
			//lowerbounds check
			if((transform.position + Vector3.forward * speed * Time.deltaTime).z > -2)
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
		}
	}
}
