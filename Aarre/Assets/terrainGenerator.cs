using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assetCreation;
using topologies;

public class terrainGenerator : MonoBehaviour {
	public Texture2D noiseTex;
	public int xpos;
	public int ypos;
	public int xfromStart;
	public int yfromStart;
	public int planetSizeX;
	public int planetSizeY;
	public bool root;
	public GameObject baseTerrain;
	public GameObject creator;
	public Texture2D[] textures;
	public Texture2D[] normals; 
	public Texture2D[] grass;
	public GameObject[] foliages; 
	float[,,] splatmap;
	float[,] treemap;
	List<int[,]> detailmap;
	public GameObject[] trees;
	public int seed;

	TerrainData terr;




	public void generate(bool surroundings=true){
		if (surroundings) {/*
			checkandGenerate (new Vector3 (-10000, 0, 10000), -1,1);
			checkandGenerate (new Vector3 (-10000, 0, 0),-1,0);
			checkandGenerate (new Vector3 (-1000, 0, -10000),-1,-1);
			checkandGenerate (new Vector3 (0, 0, 10000),0,1);

			checkandGenerate (new Vector3 (0, 0, -10000),0,-1);
			checkandGenerate (new Vector3 (10000, 0, 10000),1,1);
			checkandGenerate (new Vector3 (10000, 0, 0),1,0);
			checkandGenerate (new Vector3 (10000, 0, -10000),1,-1);*/
		}
		//if (transform.childCount == 0)
		topologyOptions options = new topologyOptions(){
			seed=seed,
			elevationHigh=0.05f,
			elevationLow=0.00f,
			centerPoint=0.5f,
			coarseNoise=800,
			midNoise=1500,
			flatNoise=8000,
			seaLevel=0.549f,
			island=false
		};

		Instantiate (
			terrains.getTerrain (trees, foliages, grass, textures, normals, transform.parent.GetComponent<worldConstants> ().noiseMatrix, baseTerrain, options, null)
		).transform.SetParent (transform, false);

	}


	// Use this for initialization
	void OnTriggerEnter (Collider col) {
		if (col.tag=="Player"&&!root) {
			generate ();
		}
	}
	void OnTriggerExit(){
		
	}
	public GameObject imager;
	void Start(){
		
	/*	if (root) {
			generate ();
		}*/
	}

	// Update is called once per frame
	void FixedUpdate () {

	}
}
