using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCountdownSystem : SystemBase {

	private View<TeleportCountdown> cEntities;

	void Start () {
		cEntities = GetEntities<TeleportCountdown>();
	}
	
	void Update () {
		for (int i = 0; i < cEntities.Count; i++) {
			TeleportCountdown c = cEntities[i];
			c.secondsLeft -= Time.deltaTime;

			if (c.secondsLeft <= 0f) {
				c.gameObject.AddComponent<Targeting>();
				Destroy(c);
			}
		}
	}

}
