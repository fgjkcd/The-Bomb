using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackingSystem : SystemBase {

    /*
        kinect			unity
        0, 0			1.75, -1.15
        1, 0			-1.8, -1.14
        0, 1			1.6, 1.24
        1, 1			-1.45, 1.16     
     */

    public float playerYCoordinate = .1f;

    private Vector2 kinect00 = new Vector2(2.3f, 3.2f);
    private Vector2 floorDim = new Vector2(2 * 2.35f , 2 * 3.4f); // 3.2
    //    private Vector2 kinect00 = new Vector2(2.2345f, 3.073f);
    //    private Vector2 floorDim = new Vector2(2 * 2.2345f, 2 * 3.073f);
    public PlayerHub playerHub;

    //public TextMesh text;

    void Start() {
        //floorDim = new Vector2(Mathf.Abs(kinect00.x) + Mathf.Abs(kinect11.x), Mathf.Abs(kinect00.y) + Mathf.Abs(kinect11.y));
        //floorDim = new Vector2(floorWidth, floorDepth);
    }

	void Update() {
		Blob[] blobs = Blobs.getBlobs();
		if (blobs.Length == 0)
			return;

		List<Vector3> playerCoords = new List<Vector3>();
		foreach (Blob blob in blobs) {
            float x = kinect00.x - blob.center.x * floorDim.x;
            float z = kinect00.y - blob.center.y * floorDim.y;
			//float x = floorWidth * (1-blob.center.x) - floorWidth;
			//float z = floorDepth * (1-blob.center.y) - floorDepth;
			//print(blob.center.x + " / " + blob.center.y + " / " + x + " / " + z + " / " + floorWidth + " / " + floorWidth / 2 + " / " + floorDepth + " / " + floorDepth / 2);
			playerCoords.Add(new Vector3(x, playerYCoordinate, z));
		}

        /*
		string str = "";
		foreach (Vector3 v in playerCoords)
			str += v.x + ", " + v.z + " // ";
		print("Player coords: " + str);
		//        print("PlayerTrackingSystem: Tracked " + playerCoords.Count + " players");

		str = "bx = " + blobs[0].center.x + "\nby = " + blobs[0].center.y + "\n\npx = " + playerCoords[0].x + "\npy = " + playerCoords[0].y;
        */
		playerHub.updatePlayerCoordinates(playerCoords);
	}

/*
	void Update() {
        Blob[] blobs = Blobs.getBlobs();
        if (blobs.Length == 0)
            return;

		List<Vector3> playerCoords = new List<Vector3>();
        foreach (Blob blob in blobs) {
            float x = floorWidth * (1-blob.center.x) - floorWidth;
            float z = floorDepth * (1-blob.center.y) - floorDepth;
            //print(blob.center.x + " / " + blob.center.y + " / " + x + " / " + z + " / " + floorWidth + " / " + floorWidth / 2 + " / " + floorDepth + " / " + floorDepth / 2);
            playerCoords.Add(new Vector3(x, playerYCoordinate, z));
        }

        string str = "";
        foreach (Vector3 v in playerCoords)
            str += v.x + ", " + v.z + " // ";
        print("Player coords: " + str);
//        print("PlayerTrackingSystem: Tracked " + playerCoords.Count + " players");

		str = "bx = " + blobs[0].center.x + "\nby = " + blobs[0].center.y + "\n\npx = " + playerCoords[0].x + "\npy = " + playerCoords[0].y;

		playerHub.updatePlayerCoordinates(playerCoords);
	}
*/

		/* 	1. foreach blob:
		 * 		try to find existing player withing matching range
		 * 			if found: log match
		 * 			else: create new player and log match
		 * 	2. foreach player:
		 * 		if matching blob exists: update position
		 * 		else: delete player
		 * 
		 * TODO optimize
		 * - matching is O(n^2)
		 * - cache previous match results?
		 * - use heuristic?
		 * - be genius in another way?
		 * - trajectories?
		 */

		/*
	void Update () {

		if (Blobs.badData)
			return;
		if (Blobs.blobs.Count == 0)
			return;
		
		Dictionary<Transform, Blob> matched = new Dictionary<Transform, Blob>();

		while (Blobs.blobs.Count > 0) {
			Blob blob = Blobs.blobs[0];
			Blobs.blobs.RemoveAt(0);

            print("Blob found @ " + blob.center);

			Transform match = matchWithPlayer(blob);
			if (match == null)
				match = newPlayer();

			Debug.Assert(!matched.ContainsKey(match));
			Debug.Assert(!matched.ContainsValue(blob));
			matched.Add(match, blob);
		}

		for (int i = 0; i < playerHub.childCount; i++) {
			Transform child = playerHub.GetChild(i);
			try {
				Blob blob = matched[child];
				child.position = new Vector3(blob.center.x, child.position.y, blob.center.y);
			} catch (KeyNotFoundException) {
				Destroy(child.gameObject);
			}
		}
	}
	*/

	/*
	private Transform matchWithPlayer(Blob blob) {
		Transform result = null;
		float closestDistance = Mathf.Infinity;
		Vector3 blobPos = new Vector3(blob.center.x, 0, blob.center.y);

		for (int i = 0; i < playerHub.childCount; i++) {
			float distance = Vector3.Distance(blobPos, playerHub.GetChild(i).position);
			if (distance < closestDistance && distance <= maxMatchingDistance) {
				result = playerHub.GetChild(i);
				closestDistance = distance;
			}
		}

		return result;
	}

	private Transform newPlayer() {
		int colorIndex = playerHub.childCount % playerColors.Length;
		GameObject go = Instantiate(playerPrefab, playerHub);
		go.name = "Player " + go.GetInstanceID();
		go.GetComponentInChildren<SpriteRenderer>().color = playerColors[colorIndex];
		return go.transform;
	}
	*/
}
