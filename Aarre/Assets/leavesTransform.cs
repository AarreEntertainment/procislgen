using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class leavesTransform : MonoBehaviour {
	Vector3 treeCenter;

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
			
		if (transform.parent.GetComponent<MeshFilter> ().sharedMesh != null) {
			if (transform.parent.GetComponent<MeshFilter> ().sharedMesh.bounds.size.x > 1 && transform.parent.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z > 1)
				GetComponent<MeshRenderer> ().enabled = true;
			else
				GetComponent<MeshRenderer> ().enabled = false;

			Vector3 sizeCalculated = transform.parent.GetComponent<MeshFilter> ().sharedMesh.bounds.size * 1.5f;
			
			sizeCalculated.y = sizeCalculated.y / 4;
			
			treeCenter = transform.parent.GetComponent<MeshFilter> ().sharedMesh.bounds.center;
			transform.position = transform.parent.position + new Vector3 (treeCenter.x / 3, treeCenter.y * 2, treeCenter.z / 3);
			transform.localScale = sizeCalculated;
		}

	}
}
