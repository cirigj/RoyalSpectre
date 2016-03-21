using UnityEngine;
using System.Collections;

public class RoomTransitions : MonoBehaviour {

	public GameObject thisRoom;
	public GameObject nextRoom;
	public GameObject ghostHunter;
	public Camera mainCam;

	//bool for if the player is going into a room away from the camera
	public bool upTransition;

	void Start(){
		mainCam = Camera.main;
	}

	void OnTriggerEnter(Collider col){
		if(!col.isTrigger){
			if (ghostHunter.GetComponent<GhostHunter> ().canSeePlayer) {
				ghostHunter.GetComponent<GhostHunter> ().playerTransition = true;
				//ghostHunter.GetComponent<GhostHunter> ().canSeePlayer = false;
			}
			if (col.gameObject == ghostHunter) {
				ghostHunter.GetComponent<GhostHunter> ().playerTransition = false;
			}
			nextRoom.gameObject.SetActive (true);
			//set current room of person to next room, used to turn off sprite renderer
			if (col.GetComponent<Person> () != null) {
				col.GetComponent<Person> ().currentRoom = nextRoom;
			}
			if (col.GetComponent<GhostHunter> () != null) {
				col.GetComponent<GhostHunter> ().currentRoom = nextRoom;
			}
			//8 because i put the rooms 8m away, 4.5 so player is in correct spot
			if (upTransition) {
				col.gameObject.transform.position += (4.5f * Vector3.forward);
				if (col.gameObject != ghostHunter) {
					mainCam.transform.position += (8 * Vector3.forward);
				}
			} else {
				col.gameObject.transform.position -= (4.5f * Vector3.forward);
				if (col.gameObject != ghostHunter) {
					mainCam.transform.position -= (8 * Vector3.forward);
					thisRoom.gameObject.SetActive (false);
				}
			}
		}
			
	}
}
