using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainDrawer : MonoBehaviour {
	public Texture2D heightmap;
	public Texture2D perlin;

	public float heightstep;
	public float widthstep;
	public float depthstep;

	public float rockThreshold;

	List<int>[] checkTriangles(List<int> triangles, List <Vector3> vertices)
	{
		Color32[] permap = perlin.GetPixels32 ();
		List<int>[] ret = new List<int>[4];
		List<int> submesh1 = new List <int> ();
		List<int> submesh2 = new List <int> ();
		List<int> submesh3 = new List <int> ();
		List<int> submesh4 = new List <int> ();
		for (var i = 0; i < triangles.Count; i+=3) {
			int point1 = triangles [i];
			int point2 = triangles [i + 1];
			int point3 = triangles [i + 2];
		
			if (Mathf.Round (Mathf.Abs (vertices [point1].y - vertices [point2].y)) > rockThreshold || Mathf.Round (Mathf.Abs (vertices [point1].y - vertices [point3].y)) > rockThreshold) {
				submesh2.Add (triangles [i]);
				submesh2.Add (triangles [i + 1]);
				submesh2.Add (triangles [i + 2]);
			} else {
				float ka = (permap [triangles [i]].r+permap [triangles [i+1]].r+permap [triangles [i+2]].r)/3;
				if (ka < 40) {
					submesh3.Add (triangles [i]);
					submesh3.Add (triangles [i + 1]);
					submesh3.Add (triangles [i + 2]);
				} else if (ka > 180) {
					submesh4.Add (triangles [i]);
					submesh4.Add (triangles [i + 1]);
					submesh4.Add (triangles [i + 2]);
				} else {
					submesh1.Add (triangles [i]);
					submesh1.Add (triangles [i + 1]);
					submesh1.Add (triangles [i + 2]);
				}

			}
			ret [0] = submesh1;
			ret [1] = submesh2;
			ret [2] = submesh3;
			ret [3] = submesh4;

		}
		return ret;
	}


	public float xOrg;
	public float yOrg;
	public float scale = 1.0F;
	private Texture2D noiseTex;
	private Color[] pix;
	private Renderer rend;

	void CalcNoise() {
		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				sample = Mathf.Round (sample*3) / 3;
				pix[(int)y * noiseTex.width + (int)x] = new Color(sample,sample,sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();
	}

	void makeNoiseTex()
	{
		rend = GetComponent<Renderer>();
		noiseTex = new Texture2D(heightmap.width , heightmap.height);
		pix = new Color[noiseTex.width * noiseTex.height];
		rend.material.mainTexture = noiseTex;
		CalcNoise ();
		perlin = noiseTex;
	}


	void Start () {
		makeNoiseTex ();

		Color32[] map = heightmap.GetPixels32();
		int x = heightmap.width;
		int y = heightmap.height;

		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();

		for (int i = 0, r = 0, c=0; i < map.Length;i++) {
			
			vertices.Add( new Vector3(c*widthstep, map [i].r*depthstep, r*heightstep));

			if (r > 0) {
				if(c >0) {
					triangles.Add(i);
					triangles.Add(i - x);
					triangles.Add(i - x - 1);

				}
				if (c < (x-1)) {
					triangles.Add(i);
					triangles.Add(i + 1);
					triangles.Add(i - x);

				}
			}

			if (c == x - 1) {
				c = 0;
				r++;
			} else
				c++;
		}
		List <int> [] submeshes = checkTriangles (triangles, vertices);

		MeshFilter filter = GetComponent <MeshFilter>();
		Mesh mesh = filter.sharedMesh;
		if (mesh == null) {
			filter.mesh = new Mesh ();
			mesh = filter.sharedMesh;
		}
		mesh.Clear ();
		int smc = 0;
			
		mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray ();
		if (submeshes[0].Count>0) mesh.SetTriangles(submeshes[0].ToArray(), 0);
		if (submeshes[1].Count>0) mesh.SetTriangles(submeshes[1].ToArray(), 1);
		if (submeshes[2].Count>0) mesh.SetTriangles(submeshes[2].ToArray(), 2);
		if (submeshes[3].Count>0) mesh.SetTriangles(submeshes[3].ToArray(), 3);

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		//mesh.uv = UnityEditor.Unwrapping.GeneratePerTriangleUV (mesh);
		GetComponent<MeshCollider> ().sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
