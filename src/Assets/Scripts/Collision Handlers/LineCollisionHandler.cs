using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionHandler : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Projectile"))
			return;

		Destroy(other.gameObject);
	}

}
