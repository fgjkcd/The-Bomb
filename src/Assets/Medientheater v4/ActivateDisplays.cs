using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This script activates as many displays as specified (as long as they are available).
 * 
 * See: https://docs.unity3d.com/Manual/MultiDisplay.html
 */
public class ActivateDisplays : MonoBehaviour {

	public int maxDisplays = 8;

	void Awake() {
		if (maxDisplays > Display.displays.Length)
			print("Warning: More displays requested than available (" + Display.displays.Length + ")");
		
		for (int i = 1; i < Display.displays.Length && i < maxDisplays; i++)
			Display.displays[i].Activate();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
