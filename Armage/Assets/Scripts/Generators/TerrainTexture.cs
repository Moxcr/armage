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
using UnityEngine;
namespace Armage
{
	public struct TerrainTexture
	{
		public Material textureMaterial;
		public float lowerBounds;
		public float upperBounds;
		public float noise;

		public TerrainTexture(string textureMaterial, float lowerBounds, float upperBounds, float noise) {
			this.textureMaterial = (Material) GameObject.Instantiate (Resources.Load<Material>(textureMaterial));
			this.lowerBounds = lowerBounds;
			this.upperBounds = upperBounds;
			this.noise = noise;
		}
	}
}

