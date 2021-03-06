//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
namespace Armage
{
	public class HeightmapProperties
	{
		public static int[] subHeightmapSizes = new int[] { 16, 16, 32, 32, 32, 48, 48, 48, 64, 64, 96, 96, 192, 384, 514 };
		public static string[] heightBlendModes = new string[] { "add", "subtract" };

		public int size = 512;
		public float perlinFreq = 12f;
		public float perturb = 32f;
		public float erode = 25f;

		public HeightmapProperties () {}
		public HeightmapProperties (int size, float perlinFreq=12f, float perturb=32f, float erode=25f) {
			this.size = size;
			this.perlinFreq = perlinFreq;
			this.perturb = perturb;
			this.erode = erode;
		}

		public static HeightmapProperties GetRand(int seed) {
			System.Random rand = new System.Random(seed);
			UnityEngine.Random.seed = rand.Next ();

			return new HeightmapProperties(512, 
			                        UnityEngine.Random.Range (12f, 15f), 
			                        UnityEngine.Random.Range (15f, 20f), 
			                        UnityEngine.Random.Range (5f, 6f));
		}

		public static HeightmapProperties GetRandSub(int seed) {
			System.Random rand = new System.Random(seed);
			UnityEngine.Random.seed = rand.Next ();

			return new HeightmapProperties(HeightmapProperties.subHeightmapSizes[UnityEngine.Random.Range (0, 
			                                                         				HeightmapProperties.subHeightmapSizes.Length - 1)], 
			                               UnityEngine.Random.Range (3f, 16f),
			                               UnityEngine.Random.Range (4f, 12f), 
			                               UnityEngine.Random.Range (12f, 32f));
		}
	}
}

