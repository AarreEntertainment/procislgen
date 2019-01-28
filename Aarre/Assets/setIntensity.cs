using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setIntensity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public float speed;
	// Update is called once per frame
	void Update () {
			transform.Rotate(-Time.deltaTime * speed, 0, 0);
		if(transform.GetChild(0).position.y<transform.position.y && transform.GetChild(0).GetComponent<Light> ().intensity>0)
			transform.GetChild(0).GetComponent<Light> ().intensity -= Time.deltaTime;
		else if(transform.GetChild(0).position.y>transform.position.y && transform.GetChild(0).GetComponent<Light> ().intensity<1)
			transform.GetChild(0).GetComponent<Light> ().intensity += Time.deltaTime;


	}
}
