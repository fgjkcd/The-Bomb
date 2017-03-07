using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * When enabled, this script activates calibration mode by 
 *  1. overlaying wall and floor captions to facilitate mapping of the scene's cameras to the corresponding displays,
 *  2. coloring all of the scene's walls and floors in different bright colors, and 
 *  3. changing the camera's backgrounds from black to white, both (2. and 3.) to facilitate aligning the scene's 
 * 	   cameras to the scene's walls and floors.
 * 
 * Disabling the script will undo the changes.
 * 
 * BEWARE: This script will likely break if any incautious changes are made on children of the Medientheater->Room 
 *         game object. This includes adding new game objects or components. Best avoid this if you don't know what 
 *         you are doing, unless you are not interested in (or you have finished) calibration (then ignore this warning).
 */
public class MedientheaterCalibration : MonoBehaviour {

	public GameObject[] projectionAreas;

	public List<TextMesh> texts = new List<TextMesh>();
	private List<Material> materials = new List<Material>();
	private List<Camera> cameras = new List<Camera>();

	private Color[] colors = { Color.red, Color.green, Color.blue, Color.red, Color.green, Color.blue, Color.magenta, new Color(1f, .5f, 0f) };

	void Awake() {
		foreach (GameObject pa in projectionAreas) {
			TextMesh t = pa.GetComponentInChildren<TextMesh>(true);
			if (t != null)
				texts.Add(t);

			Material m = pa.GetComponent<MeshRenderer>().material;
			if (m != null && !materials.Contains(m))
				materials.Add(m);

			Camera c = pa.GetComponentInChildren<Camera>(true);
			if (c != null)
				cameras.Add(c);
		}
	}

	void OnEnable() {
		foreach (TextMesh t in texts)
			t.gameObject.SetActive(true);

		for (int i = 0; i < materials.Count; i++)
			materials[i].SetColor("_EmissionColor", colors[i]);
		
		foreach (Camera c in cameras)
			c.backgroundColor = Color.white;
	}

	void OnDisable() {
		foreach (TextMesh t in texts)
			if (t != null)
				t.gameObject.SetActive(false);

		foreach (Material m in materials)
			if (m != null)
				m.SetColor("_EmissionColor", Color.black);

		foreach (Camera c in cameras)
			if (c != null)
				c.backgroundColor = Color.black;
	}

}
