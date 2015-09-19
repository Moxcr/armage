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
using System.Collections.Generic;
using UnityEngine;
namespace Armage
{
	public class Stage
	{
		public static Stage currentStage;
		public int seed;
		public float seedF1;
		public float seedF10;
		public float seedF100;

		public StarSystem StarSystem { get; set; }

		public Stage (int seed) {
			if (seed < 1) {
				seed = 1;
			}
			this.seed = seed;
			setSeedFloats();
		}

		private void setSeedFloats() {
			double log =  Math.Log10(this.seed);
			seedF1 = (float) ((double) this.seed / Math.Pow (10, Math.Floor (log)));
			seedF10 = seedF1 / 10;
			seedF100 = seedF1 / 100;
		}

		public void SetAsCurrentStage() {
			if (currentStage != null) {
				currentStage.OnExit ();
			}
			Stage.currentStage = this;
		}

		public void OnExit() {
		}
	}
}
