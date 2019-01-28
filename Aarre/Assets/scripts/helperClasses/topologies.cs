using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fractals;

namespace topologies{

	public class topologyOptions{
		public int seed;
		public float elevationHigh;
		public float elevationLow;
		public float centerPoint;
		public float coarseNoise;
		public float midNoise;
		public float flatNoise;
		public float seaLevel;
		public bool island;
	}
	
	public static class topologyCreator {

		static Texture2D blacktexture(int size)
		{
			Texture2D ret = new Texture2D (size, size);
			Color32[] pixels = ret.GetPixels32 ();

			for (int i = 0; i<pixels.Length; i++) {
				pixels[i] = new Color32 (0, 0, 0, 255);
			}
			ret.SetPixels32 (pixels);
			ret.Apply ();
			return ret;
		}

		public static List<float[,]> makeTerrain (
			Texture2D elevation, 
			float[,] noise,
			topologyOptions options
			)
			{
			if (elevation == null)
				elevation = blacktexture (2049);

			float[,] first = DiamondSquare.getFractal (elevation.width, options.seed);

			float[,] second = fadeEdges (2049, 1);

			float[,] biomes = SeamlessNoise.Generate (2048, 3);
			 

			List<float[,]> ret = new List<float[,]>();
			float[,] map = new float[first.GetLength (0), first.GetLength (1)];
			float[,] splat = new float[first.GetLength (0), first.GetLength (1)];
			float[,] foliage = new float[first.GetLength (0), first.GetLength (1)];
			float[,] trees = new float[first.GetLength (0), first.GetLength (1)];

			int size = first.GetLength (1);
			int y = 0;
			while (y < size) {
				int x = 0;
				while (x < size) {
					
					if (elevation.GetPixel (x, y).r > 0 && elevation.GetPixel (x, y).r < 1) {
						if (options.seaLevel < options.centerPoint + options.elevationHigh) {
							map [x, y] = options.elevationHigh - elevation.GetPixel (x, y).r / 20f;
							if(x>0&&y>0&&x<size-1&&y<size-1)
								map [x, y] += noise [(int)x, (int)y] / options.flatNoise;
							
							splat [x, y] = 3;
						} else {
							
							map [x, y] = options.elevationHigh - elevation.GetPixel (x, y).r / 20f ;
							if(x>0&&y>0&&x<size-1&&y<size-1)
								map [x, y] += noise [(int)x, (int)y] / options.flatNoise;
							foliage [x, y] = noise [x, y]/3f;
							splat [x, y] = 6;
						}
					}
					else if (elevation.GetPixel (x, y).r >= 1.0f) {
						if (options.seaLevel < options.centerPoint + options.elevationHigh) {
							map [x, y] = options.elevationLow ;
							if(x>0&&y>0&&x<size-1&&y<size-1)
								map [x, y] += noise [(int)x, (int)y] / options.coarseNoise;
							splat [x, y] = 0;
						} else {
							map [x, y] = options.elevationLow ;
							if(x>0&&y>0&&x<size-1&&y<size-1)
								map [x, y] += noise [(int)x, (int)y] / options.flatNoise;
							splat [x, y] = 6;
							foliage [x, y] = noise [x, y] / 3f;

						}
					}
					else if (elevation.GetPixel (x, y).r <= 0.0f) {
						if (options.island) {
							second [x, y] = Mathf.Pow (second [x, y], 3);
							if (second [x, y] > 0.0 && second [x, y] < 1f) {
								map [x, y] = options.elevationHigh - second [x, y] / 20f ;
								if(x>0&&y>0&&x<size-1&&y<size-1)
									map [x, y] += noise [(int)x, (int)y] / options.flatNoise;
								splat [x, y] = 3;
							}
							else if (second [x, y] >=1f) {
								map [x, y] = options.elevationLow ;
								if(x>0&&y>0&&x<size-1&&y<size-1)
									map [x, y] += noise [(int)x, (int)y] / options.coarseNoise;
								splat [x, y] = 0;
							}
							else{
								map [x, y] = options.elevationHigh ;
								if(x>0&&y>0&&x<size-1&&y<size-1)
									map [x, y] += noise [(int)x, (int)y] / options.flatNoise;	
								splat [x, y] = 6;
								foliage [x, y] = noise [x, y] / 3f;
							}

						} else {
							map [x, y] = options.elevationHigh ;
							if(x>0&&y>0&&x<size-1&&y<size-1)
								map [x, y] += noise [(int)x, (int)y] / options.flatNoise;	
							splat [x, y] = 6;
							foliage [x, y] = noise [x, y] / 3f;
						}
				
					}

					float landscape = first[x,y]- second [x, y];

					if (landscape < 0)
						landscape = 0;
					if (landscape > 0) {
						if (landscape > 0.2f) {
							map [x, y] += landscape / 2 * (biomes [x, y] / 2) + noise [(int)x, (int)y] / options.coarseNoise;
							foliage [x, y] = 2f / 3f + noise [x, y] / 3f;
							splat [x, y] += 2;
						} else {
							foliage [x, y] = 1f / 3f + noise [x, y] / 3f;
							splat [x, y] += 1;
							map [x, y] += landscape / 2 * (biomes [x, y] / 2) + noise [(int)x, (int)y] / options.flatNoise;
						
						}
					} 
					if (splat [x, y] == 6 &&noise [x, y] > 0.5) {
						if(noise [x, y] > 0.7)
							trees [x, y] = 1;
						else
							splat [x, y] = 9;
					}

					x++;
				}
				y++;
			}

			ret.Add (map);
			ret.Add (splat);
			ret.Add (foliage);
			ret.Add (trees);
			return ret;
		}



		 static float[,] fadeEdges(int size, float multiplier = 1){
			float[,] ret = new float[size,size];
			float y = 0;
			while (y < size) {
				float x = 0;
				while (x < size) {
					float multiplierX = (size / 2 - Mathf.Abs ((size / 2) - x)) / (size / 2);
					float multiplierY = (size / 2 - Mathf.Abs ((size / 2) - y)) / (size / 2);
					ret[(int)x,(int)y] = 1- Mathf.Sqrt( 1*multiplierX*multiplierY)*multiplier;

					if (ret [(int)x, (int)y] < 0)
						ret [(int)x, (int)y] = 0;
					x++;
				}
				y++;
			}
			return ret;
		}

	}
}