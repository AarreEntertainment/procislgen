using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environmentGenerator : MonoBehaviour {
	public GameObject [] envobjs;
	// Use this for initialization
	void Start () {
		int[] soilVertexList = GetComponent<MeshFilter> ().sharedMesh.GetTriangles(1) ;
		int treePoints = soilVertexList.Length / 2;
		List<int> soilVertices = new List<int> ();
		List<int> growVertices = new List<int> ();
		foreach (int i in soilVertexList) {
			soilVertices.Add (i);
		}

		for(int i = 0; i<treePoints;i++){
			int indx = Random.Range (0, soilVertices.Count);
			growVertices.Add (soilVertices [indx]);
			soilVertices.RemoveAt(indx);
		
		}
		Debug.Log (soilVertices.Count);
		Debug.Log (growVertices.Count);
		foreach (int point in growVertices) {
			GameObject envobj = Instantiate (envobjs[Random.Range(0,envobjs.Length)]);
			envobj.transform.position = transform.TransformPoint(GetComponent<MeshFilter> ().sharedMesh.vertices [point]);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
