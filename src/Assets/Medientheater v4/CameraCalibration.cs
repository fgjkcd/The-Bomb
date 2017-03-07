using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCalibration : MonoBehaviour {

	public Camera[] lrCameras;
	public Camera[] fbCameras;
	public Camera[] floorCameras;

	public Camera[] allCameras;

	public ScaleFactors scaleFactors = new ScaleFactors();
	public Rect[] viewports;

	[System.Serializable]
	public class ScaleFactors {		
		public Vector2 wallsLR = new Vector2(1, 1);
		public Vector2 wallsFB = new Vector2(1, 1);
		public Vector2 floor = new Vector2(1, 1);
	}


	// Use this for initialization
	void Start() {
		allCameras = new Camera[lrCameras.Length + fbCameras.Length + floorCameras.Length];
		System.Array.Copy(lrCameras, 0, allCameras, 0, lrCameras.Length);
		System.Array.Copy(fbCameras, 0, allCameras, lrCameras.Length, fbCameras.Length);
		System.Array.Copy(floorCameras, 0, allCameras, lrCameras.Length + fbCameras.Length, floorCameras.Length);

		viewports = new Rect[allCameras.Length];
		for (int i = 0; i < allCameras.Length; i++)
			viewports[i] = allCameras[i].rect;
	
		apply();
	}

	public void apply() {
		positionCameras(allCameras, viewports);
		scaleCameras(lrCameras, scaleFactors.wallsLR);
		scaleCameras(fbCameras, scaleFactors.wallsFB);
		scaleCameras(floorCameras, scaleFactors.floor);
	}

	private void scaleCameras(Camera[] cameras, Vector2 scaleFactors) {
		foreach (Camera cam in lrCameras) {
			cam.ResetProjectionMatrix();
			Matrix4x4 pm = cam.projectionMatrix;
			pm.m00 *= scaleFactors.x;
			pm.m11 *= scaleFactors.y;
			cam.projectionMatrix = pm;
		}
	}

	private void positionCameras(Camera[] cameras, Rect[] viewports) {
		for (int i = 0; i < cameras.Length; i++) {
			cameras[i].rect = viewports[i];
		}
	}

}
