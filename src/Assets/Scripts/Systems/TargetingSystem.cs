using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : SystemBase {

	//[System.Serializable]
	public enum SpeedUnit {
		UnitsPerSecond,
		SecondsToImpact
	}

	public Transform target;
	public SpeedUnit speedMode;
	public float unitsPerSecond = .25f;
	public float secondsToImpact = 3f;
	//public bool scaleProjectileSpeed = false;
	//public Transform wandHinten;
	// TODO code skalierung
	public float yPosition = .1f;

	private View<Targeting> tEntities;

	// Use this for initialization
	void Start () {
		tEntities = GetEntities<Targeting>();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < tEntities.Count; i++) {
			Targeting targeting = tEntities[i];

			// place projectile on the floor
			Vector3 pos = targeting.transform.position;
			pos.y = yPosition;
			targeting.transform.position = pos;

			// and adjust local rotation
			targeting.transform.GetChild(0).localRotation = Quaternion.Euler(90, 0, 0);

			Velocity v = targeting.gameObject.AddComponent<Velocity>();
			v.direction = (target.position - targeting.transform.position).normalized;
			//print("target: " + target.position);
			//print("proj:   " + targeting.transform.position);
			//print("delta:  " + (target.position - targeting.transform.position));
			//print("dnorm:  " + (target.position - targeting.transform.position).normalized);
			//print("dir:    " + v.direction);

			switch (speedMode) {
			case SpeedUnit.UnitsPerSecond:
				v.speed = unitsPerSecond;
				break;
			case SpeedUnit.SecondsToImpact:
				v.speed = v.transform.position.magnitude / secondsToImpact;
				break;
			default:
				throw new InternalFuckupException();
			}

			Destroy(targeting);
		}
	}

}
