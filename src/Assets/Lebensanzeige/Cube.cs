using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Methods
// LoseLife() - Removes one life and shuts down one face of the cube.
// AddLife() - Adds one life and lights up the face previously shut down

// Callbacks
// GameOver() - Gets called when all lives are removed


public class Cube : MonoBehaviour {

    public GameManager gm;

	//faces contains all Quads and stores whether they are lit (true) or not (false)
	public GameObject[] children;
	public Dictionary<GameObject, bool> faces = new Dictionary<GameObject, bool>();
	protected int lifes = 45;
	public bool gameover = false;

	//if true, cube hovers up and down
	public bool hover = true;
	//amplitude of the hover effect
	public float amplitude = 1.0f;
	//frequency of the hover effect (frames per full rotation)
	public int frequency = 60;
	//Current position on the graph
	protected float cur = 0;

	// Get all children, mix 'em good, and put them in the dictionary
	void Start () {

		children = new GameObject[transform.childCount];

		//Get child objects
		for(int i = 0; i < transform.childCount; i++) {
			children[i] = transform.GetChild(i).gameObject;
		}

		//Do the shuffle so the order in which they get shut off is randomized
		ShuffleArray<GameObject>(children);

		//Add them to the dictionary
//		foreach(GameObject child in children) {
//			faces.Add(child, true);
//			print(child.gameObject);
//		}
//		print(faces);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			LoseLife();
		}

		if(Input.GetKeyDown(KeyCode.KeypadPlus)) {
			AddLife();
		}

		//Hover effect
		if(hover) {
		//Substract previous number from the position
		transform.position = new Vector3(transform.position.x, transform.position.y - cur * amplitude, transform.position.z);
		//Calculate current position on the eternal up-and-down graph
		cur = Mathf.Sin(Map(Time.frameCount, 0, frequency, 0, 2*Mathf.PI));
		//Add current position on the graph multiplied with the amplitude to the overall position
		transform.position = new Vector3(transform.position.x, transform.position.y + cur * amplitude, transform.position.z);
		}
		
	}

	//Shuts off one cube-face, removes one life
	public int LoseLife() {
		//Game over? Don't do anything.
		if(gameover) { return 0; }
		lifes--;

		//Turn child off
		children[lifes].GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 0);
		DynamicGI.UpdateMaterials(children[lifes].GetComponent<Renderer>());

		if(lifes == 0) {
			GameOver();
			return lifes;
		}

//		print(lifes);
		return lifes;
	}

	public int AddLife() {
		//Game over? Don't do anything.
		if(gameover) { return 0; }

		lifes = Mathf.Min(++lifes, 45);

		children[lifes-1].GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 1);
		DynamicGI.UpdateMaterials(children[lifes-1].GetComponent<Renderer>());

		print(lifes);
		return lifes;
	}

	//Gets called when you lose all 45 lifes
	public void GameOver() {
		gameover = true;
        //print ("Blame the tracking.");
        //Do stuff
        gm.notifyGameOver(false);
	}

	// Fisher-Yates
	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}

	public float Map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

}