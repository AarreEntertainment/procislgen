using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vegetation : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		GetComponent<Wasabimole.ProceduralTree.ProceduralTree> ().Seed = Random.Range (0, 94432);
		GetComponent<Wasabimole.ProceduralTree.ProceduralTree> ().GenerateTree ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
