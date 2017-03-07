using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineTrackingSystem : SystemBase {

	private struct PlayerPair {
		public Transform p1, p2;
		public float distance;
	}

	public float maxLineLength = 2f;
	public LineHub lineHub;
	public PlayerHub playerHub;

	void Update() {
//        print("LTS Update");
		List<PlayerPair> pairedPlayers = calculatePlayerDistances(playerHub);
//        print(pairedPlayers.Count + "pairedPlayers: " + pairedPlayers);

		IOrderedEnumerable<PlayerPair> ppByDistance = pairedPlayers.OrderBy(pp => pp.distance);

		List<Vector3> lineCoords = determineLineCoords(ppByDistance);
//        print("PlayerTrackingSystem: Tracked " + lineCoords.Count/2 + " lines");
        lineHub.updateLineCoordinates(lineCoords);
	}

	private List<PlayerPair> calculatePlayerDistances(PlayerHub pHub) {
		List<PlayerPair> result = new List<PlayerPair>();

		int players = pHub.activePlayers();
//        print(players + " active players in scene");
		for (int i = 0; i < players; i++)
			for (int j = i+1; j < players; j++) {
//                print("Pairing players " + i + " and " + j);
				PlayerPair pp = new PlayerPair();
				pp.p1 = pHub.transform.GetChild(i);
				pp.p2 = pHub.transform.GetChild(j);
				pp.distance = Vector3.Distance(pp.p1.position, pp.p2.position);
                result.Add(pp);
			}

		return result;
	}

	private List<Vector3> determineLineCoords(IOrderedEnumerable<PlayerPair> ppByDistance) {
		List<Vector3> result = new List<Vector3>();
		List<Transform> matchedPlayers = new List<Transform>();

 //       print("---");
        int i = 0;
        foreach (PlayerPair pp in ppByDistance) {
            //            print(pp.distance + " / " + pp.p1 + " / " + pp.p2);
            if (pp.distance > maxLineLength || matchedPlayers.Contains(pp.p1) || matchedPlayers.Contains(pp.p2)) {
       //         print(i++ + ": no line between " + pp.p1.name + " and " + pp.p2.name + " - " + pp.distance);
                continue;
            }

        //    print(i++ + ": line between " + pp.p1.name + " and " + pp.p2.name + " - " + pp.distance);
			result.Add(pp.p1.position);
			result.Add(pp.p2.position);

			matchedPlayers.Add(pp.p1);
			matchedPlayers.Add(pp.p2);
		}

        //print("lines: " + result.Count/2);
		return result;
	}
		
}
