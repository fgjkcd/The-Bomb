using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHub : MonoBehaviour {

	public GameObject linePrefab;
	public PlayerHub playerHub;

	public int MaxLines { get { return maxLines;} }
	private int maxLines;

	// Use this for initialization
	void Awake () {
		if (playerHub.maxPlayers % 2 != 0)
			throw new InternalFuckupException();

		maxLines = playerHub.maxPlayers / 2;
		for (int i = 0; i < maxLines; i++) {
			GameObject go = Instantiate(linePrefab, transform);
			go.SetActive(false);
		}
//        print(maxLines);
	}
		
	// coordinates are given in pairs of start and end
	public void updateLineCoordinates(List<Vector3> coords) {
		if (coords.Count % 2 != 0)
			throw new InternalFuckupException();
		
		//for (int i = 0; i < coords.Count; i += 2) {
		for (int i = 0; i < maxLines; i++) {
			Transform line = transform.GetChild(i);

            if (i >= coords.Count / 2) {
//                print("line " + i + ": nope");
				line.gameObject.SetActive(false);
				continue;
			}
 //           print("line " + i + ": yep");
			
			Vector3 start = coords[2*i];
			Vector3 end = coords[2*i + 1];
			Vector3 delta = start - end;

            // position: between the two end points
            line.position = start - delta / 2;

			// rotation: along the line from start to end (prefab oriented towards (1, 0, 0))
			float angle = Vector3.Angle(Vector3.right, delta) + 180;
			if (start.z > end.z)
				angle *= -1;
			line.rotation = Quaternion.AngleAxis(angle, Vector3.up);

			// scale: reaching from start to end
			Vector3 scale = line.localScale;
			scale.x = delta.magnitude;
			line.localScale = scale;

            line.gameObject.SetActive(true);
		}
	}

			// TODO
			/*
			for (int i = 0; i < lEntities.Count; i++) {
				Line line = lEntities[i];
				
				Vector3 delta = line.pFrom.position - line.pTo.transform.position;
				if (delta.magnitude > maxLineLength) {
					Destroy(line.gameObject);
					continue;
				}
				
				// position: between the two colliding players
				line.transform.position = line.pFrom.position - delta / 2;
				
				// rotation: along the line from master to partner (prefab oriented towards (1, 0, 0))
				float angle = Vector3.Angle(Vector3.right, delta) + 180;
				if (line.pFrom.position.z > line.pTo.position.z)
					angle *= -1;
				line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
				
				// scale: reaching from the center of one player to the center of the other
				Vector3 scale = line.transform.localScale;
				scale.x = delta.magnitude;
				line.transform.localScale = scale;
			}
			*/


}
