using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnSystem : MonoBehaviour {

	/*
	public class GroupSpawnSystem {
		public int minProjectiles, maxProjectiles;
		public bool homogeneous;
		public void spawn(GameObject[] prefabs) {
		}
	}
*/



	public GameObject[] projectilePrefabs;
	public float projectileSpeed;
	public Transform projectileHub;
	public Transform[] walls;
	public float yOffset = -.1f;
	public float spawnDelay = 2f;

	private float countdown;

	// Use this for initialization
	void Start () {
		countdown = spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		countdown -= Time.deltaTime;

		if (countdown <= 0f) {
			Transform wall = walls[Random.Range(0, walls.Length)];
			Vector3 position;
			float yRot = 0f;

			position.y = wall.position.y + wall.localScale.y / 2f + yOffset; // 1.75

			// TODO add gap to prevent overlapping with wall border

			if (Mathf.Approximately(wall.position.x, 0f)) { // front or back wall
				position.x = wall.position.x + Random.Range(0, wall.localScale.x) - wall.localScale.x / 2f;
				position.z = wall.position.z - Mathf.Sign(wall.position.z) * .05f;
				if (wall.position.z > 0) // back wall
					yRot = 180f;

			} else { // left or right wall
				position.x = wall.position.x - Mathf.Sign(wall.position.x) * .05f;
				position.z = wall.position.z + Random.Range(0, wall.localScale.z) - wall.localScale.z / 2f;
				if (wall.position.x < 0) // left wall
					yRot = -90f;
				else
					yRot = 90f;
			}

			GameObject prefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
			Quaternion rotation = Quaternion.Euler(0, yRot, 0);

			GameObject projectile = Instantiate(prefab, position, Quaternion.identity, projectileHub);
			projectile.GetComponent<Velocity>().speed = projectileSpeed;
			projectile.transform.GetChild(0).rotation = rotation;
			// TODO use physics (and acceleration) for falling down along the wall, add/enabled velocity later

			countdown = spawnDelay;
		}
	}

}
