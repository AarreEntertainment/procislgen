using UnityEngine;
using System.Collections;



namespace fractals{

	public static class SeamlessNoise {

		static float transform(float sample, int textureType)
		{
			switch (textureType) {
			case 0:
				return sample;
			case 1:
				if (0.5f - sample <= -0.3)
					sample = Mathf.Round (sample * 10) / 10;
				else if (0.5f - sample >= 0.1)
					sample = Mathf.Round ((1 - sample) * 10) / 10;
				else if (sample > 0.5f)
					sample = Mathf.Round (1 - sample * 5) / 10;
				else if (sample < 0.5f)
					sample = Mathf.Round (sample * 5) / 10;
				return sample;
			default:
				return sample;

			}
		}

		public static float[,] Generate(int size, int scale, int textureType = 0){
	
				float[,] ret = new float[size,size];
				float y = 0.0f;
				while (y < size/2) {
					float x = 0.0f;
					while(x<size/2){
						float sample = transform(Mathf.PerlinNoise (x/size/2* scale, y/size/2* scale), textureType);
						ret [(int)x, (int)y] = sample;
						ret [size - 1 - (int)x, (int)y] = sample;
						ret [size - 1 - (int)x, size - 1 - (int)y] = sample;
						ret [(int)x, size - 1 - (int)y] = sample;
						x++;
					}
					y++;
				}
				return ret;
			
		}
		public static Texture2D getTexture(int size, int scale, int textureType=0,bool normal = false, float r = 0.5f, float g= 0.5f, float b= 0.5f){

				Texture2D res;
				if (normal)
					res = new Texture2D (size, size, TextureFormat.RGBA32, true);
				else
					res = new Texture2D (size, size);

				Color[] cols = new Color[size * size];
				float[,] noise = Generate (size, scale, textureType);
				int i = 0;
				int y = 0;
				while (y < size) {
					int x = 0;
					while (x < size) {
						if (normal) {
							float sample = 0.5f + noise [x, y] * 0.5f;
							cols [i] = new Color (sample, sample, sample);
						}
						else
						cols [i] = new Color (r/2+noise [x, y]*r,g/2+ noise [x, y]*g,b/2+ noise [x, y]*b);
						i++;
						x++;
					}
					y++;
				}
				res.SetPixels (cols);
				res.Apply ();
				return res;

		}
	}

public static class DiamondSquare {
		public static float [,]getFractal(int size, int randomState = 420666, int grain = 2){
			_DiamondSquare diam = new _DiamondSquare ();
			return diam.getFractal (size, randomState, grain);
		}
	}

	class _DiamondSquare {
		
	int pwidth=32;
	int pheight=32;
	float pwidthpheight;
	int GRAIN=2;

	float[,] grid;

	public float[,] getFractal(int size, int randomState=420666, int grain = 2){
		GRAIN = grain;
			Random.InitState(randomState);
		pwidthpheight = pwidth * pheight;
		grid = new float[size,size];
		drawPlasma ((float)size,(float)size);
		return grid;
	}

	float displace(float num)
	{
		float max = num / pwidthpheight * GRAIN;
		return Random.Range(-0.5f, 0.5f)* max;
	}

	void drawPlasma(float w, float h)
	{
	   float c1, c2, c3, c4;
			   
	   c1 = .5f;
		c2 = .5f;
		c3 = .5f;;
		c4 = .5f;
		
	   divideGrid(0.0f, 0.0f, w , h , c1, c2, c3, c4);
	}

	void divideGrid(float x, float y, float w, float h, float c1, float c2, float c3, float c4)
	{
	 
	   float newWidth = w * 0.5f;
	   float newHeight = h * 0.5f;
	 
	   if (w < 1.0f || h < 1.0f)
	   {
		 //The four corners of the grid piece will be averaged and drawn as a single pixel.
		 float c = (c1 + c2 + c3 + c4) * 0.25f;
			grid [(int)x,(int) y] = c;}
	   else
	   {
		 float middle =(c1 + c2 + c3 + c4) * 0.25f + displace(newWidth + newHeight);      //Randomly displace the midpoint!
		 float edge1 = (c1 + c2) * 0.5f; //Calculate the edges by averaging the two corners of each edge.
		 float edge2 = (c2 + c3) * 0.5f;
		 float edge3 = (c3 + c4) * 0.5f;
		 float edge4 = (c4 + c1) * 0.5f;
	 
		 //Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!
		 if (middle <= 0)
		 {
		   middle = 0;
		 }
		 else if (middle > 1.0f)
		 {
		   middle = 1.0f;
		 }
	 
		 //Do the operation over again for each of the four new grids.                 
		 divideGrid(x, y, newWidth, newHeight, c1, edge1, middle, edge4);
		 divideGrid(x + newWidth, y, newWidth, newHeight, edge1, c2, edge2, middle);
		 divideGrid(x + newWidth, y + newHeight, newWidth, newHeight, middle, edge2, c3, edge3);
		 divideGrid(x, y + newHeight, newWidth, newHeight, edge4, middle, edge3, c4);
	   }
	}

	
}
}