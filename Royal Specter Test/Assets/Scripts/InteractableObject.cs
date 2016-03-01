using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

	public string name;
	public type objType;
	//bool for triggers
	//public bool canInteract;
	public enum type{
		Person,
		GhostHunter,
		Door,
		Ghost,
		Item,
		none
	}

	//default constructor
	public virtual void Start(){
		name = "interactable object";
		objType = type.none;
		//canInteract = true;
	}

	public virtual void Use(){
		Debug.Log ("using some object");
	}
}
