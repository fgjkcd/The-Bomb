using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieAnimation : MonoBehaviour {

	public string id;
	public AudioClip audioClip;
	public MovieTexture wallLeftFront, wallLeftBack, wallBack, wallRightBack, wallRightFront, wallFront, floorFront, floorBack;

	public MovieTexture[] Textures { get { return textures; } }
	private MovieTexture[] textures = null;

	public float Duration { get { return duration; } }
	private float duration = 0;

	void Start () {
		textures = new MovieTexture[] { wallLeftFront, wallLeftBack, wallBack, wallRightBack, wallRightFront, wallFront, floorFront, floorBack };

		foreach (MovieTexture mt in textures)
			if (mt != null) {
				duration = Mathf.Max(duration, mt.duration);
			}

		if (audioClip != null && !audioClip.preloadAudioData) {
			audioClip.LoadAudioData();
			duration = Mathf.Max(duration, audioClip.length);
		}
	}

}
