using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour {

	public void GoToLevel(int lvl){
		Application.LoadLevel (lvl);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.M))
			Application.LoadLevel (0);
	}
}
