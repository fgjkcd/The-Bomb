using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedientheaterCalibrationExtension : MonoBehaviour {

	public GameObject[] medientheaters;

	private GameObject[] pAreas;

	// Use this for initialization
	void Start () {
 		pAreas = GetComponent<MedientheaterCalibration>().projectionAreas;
		pAreas = pAreas.OrderBy(go => go.GetComponentInChildren<Camera>().targetDisplay).ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			foreach (GameObject mt in medientheaters)
				mt.SetActive(!mt.activeInHierarchy);
		}
	}

}
