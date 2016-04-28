using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerEnterHandler {

	// Use this for initialization
	void Start () {
		GameObject skills = transform.GetChild (1).gameObject;
		GameObject portrait = transform.GetChild (2).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerEnter(PointerEventData dataName)
	{
		//Debug.Log (gameObject.name);
	}
}
