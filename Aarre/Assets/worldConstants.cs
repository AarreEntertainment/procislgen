using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldConstants : MonoBehaviour {
	public Texture2D noiseTex;
	public float[,] noiseMatrix;
	// Use this for initialization
	void Start () {
		generateNoise ();
		if (transform.GetChild (0).GetComponent<terrainGenerator> ().root)
			transform.GetChild (0).GetComponent<terrainGenerator> ().generate ();	
		if (transform.GetChild (1).GetComponent<terrainGenerator> ().root)
			transform.GetChild (1).GetComponent<terrainGenerator> ().generate ();	
	}

	void generateNoise(){
		noiseMatrix = new float[noiseTex.width,noiseTex.height]; 
		Color[] cols = noiseTex.GetPixels ();

		int y = 0;
		while (y < noiseTex.height) {
			int x = 0;
			while (x < noiseTex.width) {
				noiseMatrix [x, y] = cols [x * (y+1)].r;
				x++;
			}
			y++;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*if (noiseMatrix.GetLength (0) < 1) {
			generateNoise ();
		}*/
	}
}
