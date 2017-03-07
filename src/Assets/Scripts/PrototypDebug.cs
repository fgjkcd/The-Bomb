using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypDebug : MonoBehaviour {

	public GameObject[] activateOnStart;
	public GameObject[] deactivateOnStart;

	void OnEnable() {
		foreach (GameObject go in activateOnStart)
			go.SetActive(true);
		foreach (GameObject go in deactivateOnStart)
			go.SetActive(false);		
	}

	void OnDisable() {
		foreach (GameObject go in activateOnStart)
			go.SetActive(false);
		foreach (GameObject go in deactivateOnStart)
			go.SetActive(true);				
	}

}
