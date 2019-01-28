using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {
	public bool created = false;
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"&& !created) {
			created = true;
			Instantiate (transform.parent.gameObject).transform.position=transform.parent.position + new Vector3(-500,0,0);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
