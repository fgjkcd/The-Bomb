using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovieManager : MonoBehaviour {

	public Renderer wallLeftFront, wallLeftBack, wallBack, wallRightBack, wallRightFront, wallFront, floorFront, floorBack;

	private Renderer[] renderers;
	private Dictionary<Renderer, Texture> blindTextures = new Dictionary<Renderer, Texture>();
	private Dictionary<string, MovieAnimation> animations = new Dictionary<string, MovieAnimation>();
    private AudioSource audioPlayer;

	private MovieAnimation currentlyPlaying = null;
	private float timeLeft = 0;

	public delegate void OnPlaybackStopped(bool aborted);
	private event OnPlaybackStopped callback;
	//private UnityEvent<string, bool> onPlaybackStopped; // <id of the MovieAnimation, playback interrupted?>

    void Start() {
		renderers = new Renderer[] { wallLeftFront, wallLeftBack, wallBack, wallRightBack, wallRightFront, wallFront, floorFront, floorBack };
		foreach (Renderer r in renderers)
			blindTextures.Add(r, r.material.mainTexture);

		MovieAnimation[] movies = GetComponentsInChildren<MovieAnimation>();
		foreach (MovieAnimation ma in movies)
			animations.Add(ma.id, ma);

		audioPlayer = GetComponent<AudioSource>();
    }

	public void playAnimation(string id, OnPlaybackStopped callback) {//, UnityEvent<string, bool> callback) {
		if (currentlyPlaying != null) {
			Debug.LogWarningFormat("Movie playback interrupted ({0})", currentlyPlaying.id);
			playbackStopped(true);
		}

        if (!animations.ContainsKey(id))
            throw new InternalFuckupException(id + " not in " + animations.Keys.ToString());

		MovieAnimation ma = animations[id];
		for (int i = 0; i < renderers.Length; i++) {
            if (ma.Textures[i] != null)
            {
                renderers[i].material.mainTexture = ma.Textures[i];
                if (!ma.Textures[i].isPlaying)
                    ma.Textures[i].Play();
            }
		}

		if (ma.audioClip != null) {
			audioPlayer.clip = ma.audioClip;
			audioPlayer.Play();
		}

		currentlyPlaying = ma;
		timeLeft = ma.Duration;

		this.callback = callback;
	}

	private void playbackStopped(bool aborted) {
		foreach (MovieTexture mt in currentlyPlaying.Textures)
            if (mt != null)
			    mt.Stop();		

		foreach (Renderer r in renderers)
			r.material.mainTexture = blindTextures[r];
		
		//string id = currentlyPlaying.id;

		currentlyPlaying = null;

	//	if (onPlaybackStopped != null)
	//		onPlaybackStopped.Invoke(id, aborted);
		if (callback != null)
			callback(aborted);
	}

	// Update is called once per frame
	void Update () {
		if (currentlyPlaying != null) {
			timeLeft -= Time.deltaTime;
			if (timeLeft <= 0)
				playbackStopped(false);
		}

/*
		if (Input.GetKeyDown(KeyCode.Q)) {
            for (int i = 0; i < textures.Length; i++)
//                for (int i = 0; i < 3; i++)
                    playMovie(textures[i], renderers[i]);

            duration = 0;
            foreach (MovieTexture mt in textures)
                duration = Mathf.Max(duration, mt.duration);
		}
//		else if (Input.GetKeyDown(KeyCode.W))
*/
	}
		
}
