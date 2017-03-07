using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MedientheaterCalibrationInput : MonoBehaviour {

	private const int SCALE = -1;
	private const int VIEWPORT = -2;

	private const int LR = 0;
	private const int FB = 1;
	private const int FLOOR = 2;

	public float scalingStep = .01f;
	public float vpScalingStep = .01f;
	public float vpPositionStep = .01f;

	private MedientheaterCalibration mtCal;
	private CameraCalibration camCal;
	private int mode = SCALE;
	private int selection = LR;

	// Use this for initialization
	void Start () {
		camCal = GetComponentInParent<CameraCalibration>();
		mtCal = GetComponent<MedientheaterCalibration>();
		char mc = mode == SCALE ? 'S' : 'V';
		mtCal.texts[selection].text = mc + mtCal.texts[selection].text;
	}

	private void setSelection(int sel) {
		mtCal.texts[selection].text = mtCal.texts[selection].text.Substring(1);
		selection = sel;
		char mc = mode == SCALE ? 'S' : 'V';
		mtCal.texts[selection].text = mc + mtCal.texts[selection].text;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {
			mode = SCALE;
			print("SCALE mode");
		} else if (Input.GetKeyDown(KeyCode.V)) {
			mode = VIEWPORT;
			print("VIEWPORT mode");

		} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
			setSelection(LR);
			print("LR cams / Cam 0 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			setSelection(FB);
			print("FB cams / Cam 1 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			setSelection(FLOOR);
			print("FLOOR cams / Cam 2 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			setSelection(3);
			print("Cam 3 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			setSelection(4);
			print("Cam 4 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			setSelection(5);
			print("Cam 5 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			setSelection(6);
			print("Cam 6 selected");
		} else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			setSelection(7);
			print("Cam 7 selected");
		
		} else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
			if (mode == SCALE) {
				adjustScaleX(selection, Input.GetKeyDown(KeyCode.RightArrow) ? 1 : -1);
			} else if (mode == VIEWPORT) {
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					adjustViewportWidth(selection, Input.GetKeyDown(KeyCode.RightArrow) ? 1 : -1);
				} else {
					adjustViewportX(selection, Input.GetKeyDown(KeyCode.RightArrow) ? 1 : -1);
				}
			}
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
			if (mode == SCALE) {
				adjustScaleY(selection, Input.GetKeyDown(KeyCode.UpArrow) ? 1 : -1);
			} else if (mode == VIEWPORT) {
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					adjustViewportHeight(selection, Input.GetKeyDown(KeyCode.UpArrow) ? 1 : -1);
				else
					adjustViewportY(selection, Input.GetKeyDown(KeyCode.UpArrow) ? 1 : -1);

			}
		}
		else if (Input.GetKeyDown(KeyCode.Return))
			saveToFile();
		else if (Input.GetKeyDown(KeyCode.Escape))
			saveAndQuit();
	}

	private void adjustViewportX(int selection, int direction) {
		print(string.Format("Adjusting viewport x of {0} by {1}; now {2}", selection, direction * vpPositionStep, camCal.viewports[selection]));
		camCal.viewports[selection].x += direction * vpPositionStep;
		camCal.apply();
	}

	private void adjustViewportY(int selection, int direction) {
		print("Adjusting viewport y of " + selection + " by " + direction * vpPositionStep);
		camCal.viewports[selection].y += direction * vpPositionStep;
		camCal.apply();
	}

	private void adjustViewportWidth(int selection, int direction) {
		print("Adjusting viewport width of " + selection + " by " + direction * vpScalingStep);
		camCal.viewports[selection].width += direction * vpScalingStep;
		camCal.apply();
	}

	private void adjustViewportHeight(int selection, int direction) {
		print("Adjusting viewport height of " + selection + " by " + direction * vpScalingStep);
		camCal.viewports[selection].height += direction * vpScalingStep;
		camCal.apply();
	}

	private void adjustScaleX(int selection, int direction) {
		print("Adjusting scale x of " + selection + " by " + direction * scalingStep);
		switch (selection) {
		case LR:
			camCal.scaleFactors.wallsLR.x += direction * scalingStep;
			break;
		case FB:
			camCal.scaleFactors.wallsFB.x += direction * scalingStep;
			break;
		case FLOOR:
			camCal.scaleFactors.floor.x += direction * scalingStep;
			break;
		}
		camCal.apply();
	}

	private void adjustScaleY(int selection, int direction) {
		print("Adjusting scale y of " + selection + " by " + direction * scalingStep);
		switch (selection) {
		case LR:
			camCal.scaleFactors.wallsLR.y += direction * scalingStep;
			break;
		case FB:
			camCal.scaleFactors.wallsFB.y += direction * scalingStep;
			break;
		case FLOOR:
			camCal.scaleFactors.floor.y += direction * scalingStep;
			break;
		}
		camCal.apply();
	}

	private void saveToFile() {
		int increment = 0;
		string filename = "./camcal" + increment++ + ".txt";
		while (File.Exists(filename))
			filename = "./camcal" + increment++ + ".txt";

		StreamWriter fs = File.CreateText(filename);
		fs.WriteLine("Scale factors:");
		fs.WriteLine("WallsLR: " + camCal.scaleFactors.wallsLR);
		fs.WriteLine("WallsFB: " + camCal.scaleFactors.wallsFB);
		fs.WriteLine("Floor:   " + camCal.scaleFactors.floor);
		fs.WriteLine();
		fs.WriteLine("Viewports:");
		for (int i = 0; i < camCal.viewports.Length; i++)
			fs.WriteLine("Camera " + i + ": " + camCal.viewports[i]);
		fs.Close();
		print("Camera calibration settings written to file.");
	}

	private void saveAndQuit() {
		saveToFile();
		Application.Quit();
	}

}
