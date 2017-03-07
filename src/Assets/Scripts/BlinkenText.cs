using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkenText : MonoBehaviour {

	public GameObject text;
	public float frequency = 1f;

	private float countdown;

	void Start () {
		countdown = frequency;
	}
	
	void Update () {
		countdown -= Time.deltaTime;
		if (countdown <= 0) {
			text.SetActive(!text.activeInHierarchy);
			countdown = frequency;
		}
	}

}
