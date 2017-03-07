using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallBorderCollisionHandler : MonoBehaviour {

	public float countdownSeconds;

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Projectile"))
			return;
		
		//print("Collision with " + other);
		GameObject go = other.gameObject;

		Destroy(go.GetComponent<Velocity>());

		TeleportCountdown c = go.AddComponent<TeleportCountdown>();
		c.secondsLeft = countdownSeconds;
	}

}
