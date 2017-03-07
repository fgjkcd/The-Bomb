using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterObjectCollisionHandler : MonoBehaviour {

	public Cube cube;

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Projectile"))
			return;

		Destroy(other.gameObject);
		cube.LoseLife();
	}

/* DEBUG
	public int hitPoints = 10;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Line")) {
			Destroy(other.gameObject);
			return;
		}

		if (!other.CompareTag("Projectile"))
			return;

		hitPoints--;
		print("Center object hit: " + hitPoints);
		Destroy(other.gameObject);

		if (hitPoints <= 0)
			Destroy(gameObject);
	}
*/
}
