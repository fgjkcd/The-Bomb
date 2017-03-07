using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	public float initialTime = 404;
	protected float timeLeft;
	public bool active = false;
	public bool blink = false;
	protected Renderer rend;

	// Use this for initialization
	void Start () {
		timeLeft = initialTime;
		//rend = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!active)
			return;
		
		timeLeft -= Time.deltaTime;
		GetComponent<TextMesh> ().text = timeLeft.ToString("F2");
		Blink ();

		if (timeLeft <= 0) {
			//Game Over Callback could go here
		}
	}

	public void StartTimer() {
		active = true;
	}

	public void StopTimer() {
		active = false;
	}

	public void ResetTimer() {
		timeLeft = initialTime;
		active = false;
	}

	public void ToggleBlink() {
		blink = !blink;
	}

	void Blink() {
		if (!blink)
			return;
	
		bool oddeven = Mathf.FloorToInt (Time.time) % 2 == 0;
		GetComponent<MeshRenderer> ().enabled = oddeven;
	}
}
