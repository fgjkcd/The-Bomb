using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHub : MonoBehaviour {

	public GameObject playerPrefab;
	public int maxPlayers = 8;
	private Renderer[] renderers;

	// Use this for initialization
	void Awake() {
        renderers = new Renderer[maxPlayers];
		for (int i = 0; i < maxPlayers; i++) {
			GameObject go = Instantiate(playerPrefab, transform);
            go.name = "Player " + i;
			go.SetActive(false);
            renderers[i] = go.GetComponent<Renderer>();
		}

		//renderers = GetComponentsInChildren<SpriteRenderer>();

		if (renderers.Length != maxPlayers)
			throw new InternalFuckupException(renderers.Length + " != " + maxPlayers);
	}

	public int activePlayers() {
		int result = 0;
		for (int i = 0; i < transform.childCount; i++)
			if (transform.GetChild(i).gameObject.activeInHierarchy)
				result++;
		return result;
	}

	public void setPlayersVisible(bool flag) {
		foreach (Renderer r in renderers)
			r.enabled = flag;
	}

	public void updatePlayerCoordinates(List<Vector3> coords) {
		for (int i = 0; i < transform.childCount; i++) {
			Transform player = transform.GetChild(i);		
            	
			if (i >= coords.Count) {
				player.gameObject.SetActive(false);				
				continue;
			}
		
			player.position = coords[i];
			player.gameObject.SetActive(true);				
		}
	}

}
