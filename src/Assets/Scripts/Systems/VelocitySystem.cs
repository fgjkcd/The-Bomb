using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocitySystem : SystemBase {

	private View<Velocity> vEntities;

	// Use this for initialization
	void Start () {
		vEntities = GetEntities<Velocity>();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < vEntities.Count; i++) {
			Velocity v = vEntities[i];
			//v.transform.Translate(v.direction * v.speed * Time.deltaTime);
			Vector3 position = v.transform.position;
			position += v.direction * v.speed * Time.deltaTime;
			v.transform.position = position;
		}
	}

}
