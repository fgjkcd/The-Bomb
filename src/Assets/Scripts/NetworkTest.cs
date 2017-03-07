using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Displays blobs as received by the network with raw position / 100 and center.y * -1
// For testing purposes only

public class NetworkTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//print(Blobs.blobs.Count);

		foreach(GameObject delete in GameObject.FindGameObjectsWithTag("EditorOnly")) {
			Destroy(delete);
		}

//		print("Blobs:");
		int i = 0;
        Blob[] blobs = Blobs.getBlobs();
		foreach(Blob blob in blobs) {
			GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylinder.transform.parent = gameObject.transform;
			cylinder.transform.position = new Vector3(blob.center.x / 100, -blob.center.y / 100, 0);
			cylinder.tag = "EditorOnly";
//			print(" " + i + ": " + blob.center);
			i++;
		}
	}
}
