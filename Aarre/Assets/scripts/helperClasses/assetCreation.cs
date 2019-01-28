using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using topologies;

namespace assetCreation {
	public class palette{
		Color stone1;
		Color stone2;

		Color vegetationDry;
		Color vegetationHealthy;
		
		Color sand;

		Color soil;
	}

public static class terrains {
		
		private static TerrainData createPrototypes(TerrainData terr, GameObject[] trees, GameObject[] meshDetails, Texture2D[] textureDetails, Texture2D[]textures, Texture2D[]normals){

			SplatPrototype[] terrText = new SplatPrototype[textures.Length];
			for (int i = 0; i < textures.Length; i++) {
				terrText[i] = new SplatPrototype (){ 
					texture = textures [i], 
					normalMap = normals [i], 
					tileSize= new Vector2(10f, 10f), 
					metallic=0.6f,   };
			}

			DetailPrototype[] detprot = new DetailPrototype[textureDetails.Length+meshDetails.Length];
			for (int i = 0; i < textureDetails.Length; i++) {
				detprot [i] = new DetailPrototype () {
					prototype = new GameObject(),
					prototypeTexture = textureDetails [i],
					dryColor = new Color (0.02f, 0.06f, 0f),
					healthyColor = new Color (0.01f, 0.1f, 0f),
					noiseSpread = 0.1f,
					minWidth=0.2f,
					minHeight=0.2f,
					maxWidth=0.8f,
					maxHeight=0.8f
				};	
			}
			for (int i = textureDetails.Length; i < textureDetails.Length+meshDetails.Length; i++) {
				detprot [i] = new DetailPrototype () {
					prototype = meshDetails[i-textureDetails.Length],
					usePrototypeMesh = true,
					renderMode = DetailRenderMode.VertexLit,
					noiseSpread=0.1f,
					minWidth=0.1f,
					minHeight=0.1f,
					maxWidth=0.5f,
					maxHeight=0.5f,
					dryColor=Color.grey,
					healthyColor=Color.grey

				};	
			}
			TreePrototype[] treeprot = new TreePrototype[trees.Length];
			for (int i = 0; i < trees.Length; i++) {
				treeprot [i] = new TreePrototype (){ prefab = trees [i]};
			}

			terr.splatPrototypes = terrText;
			terr.detailPrototypes = detprot;
			terr.treePrototypes = treeprot;

			return terr;
		}


	
	public static GameObject getTerrain(
		GameObject[] trees, 
		GameObject[] foliage, 
		Texture2D[] grass, 

		Texture2D[] textures,  
		Texture2D[] normals, 
		float[,] noise, 
		GameObject origin,
		topologyOptions options,
		Texture2D elevation = null

		){

			GameObject ret = origin;

			List<int[,]> detailmap = new List<int[,]> ();	


			TerrainData terr = new TerrainData () { size = new Vector3 (50, 3200, 50), heightmapResolution = 2048, alphamapResolution=2048 };
			terr.SetDetailResolution (2048, 8);

			terr = createPrototypes (terr, trees, foliage, grass, textures, normals);
			for (int i = 0; i < terr.detailPrototypes.Length; i++) {
				detailmap.Add(terr.GetDetailLayer (0, 0, terr.detailWidth, terr.detailHeight, i));
			}
				
			List<float[,]> mapData = topologyCreator.makeTerrain (elevation, noise, options);
			terr.SetHeights (0, 0, mapData[0]);

			terr.SetAlphamaps (0, 0, drawSplatMaps(mapData[1], 2048, textures.Length));

			detailmap = drawDetails (mapData[2], detailmap);
			for ( int i = 0; i< detailmap.Count;i++) {
				terr.SetDetailLayer (0, 0, i, detailmap [i]);
			}

			ret.GetComponent<Terrain>().terrainData = terr;
			ret.GetComponent<TerrainCollider> ().terrainData = terr;

			int y = 0;
			while (y < mapData [3].GetLength (0)) {
				int x = 0;
				while (x<mapData [3].GetLength(1)){
					if (mapData [3] [x, y] > 0) {
						Vector3 treepos = new Vector3 ((float)x / 2048.0f,  mapData [3][x,y] , (float)y / 2048.0f);  
						float randomize = (float)Random.Range (1, 18) / 10;
						if (terr.GetHeight (x, y) > (terr.size.y*options.elevationHigh) && terr.GetHeight(x,y)<(terr.size.y*options.elevationHigh + terr.size.y*0.0025))
							ret.GetComponent<Terrain>().AddTreeInstance (new TreeInstance () {heightScale=0.2f + randomize ,widthScale=0.2f+randomize,
								position = treepos,
								color = Color.white,
								lightmapColor = new Color(0.5f,0.5f,0.5f),
								prototypeIndex = Random.Range(0,terr.treePrototypes.Length)
							});
					}
					x++;
				}
				y++;
			}
			return ret;
		}



		static List<int[,]>drawDetails(float[,] foliage, List<int[,]> ret){
			int y = 0;
			while (y < ret[0].GetLength(0)) {
				int x = 0;
				while (x < ret[0].GetLength(0)) {
					ret[(int) Mathf.Floor(foliage[x,y]*ret.Count)][x,y]=1;
					x++;
				}
				y++;
			}
			return ret;
		}

		static float[,,] drawSplatMaps(float[,] splat, int splatsize, int splatcount)
		{
			float[,,] ret = new float[splatsize, splatsize, splatcount];
			int y = 0;
			while (y < splatsize) {
				int x = 0;
				while (x < splatsize) {
					for (var i = 0; i < splatcount; i++) {

						if (i == splat [x, y]) {
							ret [x, y, i] = 1f;
						}
						else
							ret [x, y, i] = 0f;
					}
					x++;
				}
				y++;
			}
			return ret;
		}
	}
	}