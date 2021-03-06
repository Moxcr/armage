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
using Random=System.Random;
using System.Collections.Generic;
using UnityEngine;
namespace Armage
{
	public static class NameGen
	{
		private static NameFeed nameFeed;
		private static Dictionary<string, string[]> currentDialect;

		public static string[] Generate(int amount, NameFeed nameFeed) {
			if (Stage.currentStage == null || Stage.currentStage.seedF10 == 0) {
				Debug.LogError("NameGen requires the current stage to be defined.");
				Debug.Break();
			}

			NameGen.SetNameFeed(nameFeed);
			generateDialect();

			return generatePlanetNames(amount);
		}

		public static void SetNameFeed(NameFeed feed) {
			NameGen.nameFeed = feed;
		}

		private static string[] generatePlanetNames(int amount) {
			int seed = Stage.currentStage.seed;
			Random r = new Random(seed);

			List<string> names = new List<string>();
			for (int a = 0; a < amount; a++) {
				int nameCount = r.Next() > seed ? 2 : 1;

				string fullName = "";

				for (int n = 0; n < nameCount; n++) {
					int loc1 = (int) Math.Round ((double) NameGen.nameFeed.prefixes.Length * r.NextDouble()) - 1;
					int loc2 = (int) Math.Round ((double) NameGen.nameFeed.postfixes.Length * r.NextDouble()) - 1;
					if (loc1 < 0) loc1 = 0;
					if (loc2 < 0) loc2 = 0;

					string pre = NameGen.nameFeed.prefixes[loc1];
					pre = pre.Substring (0, 1).ToUpper () + pre.Substring (1);
					string post = NameGen.nameFeed.postfixes[loc2];
					fullName += pre + post;
					if (n+1 < nameCount) {
						fullName += " ";
					}
				}

				fullName = NameGen.applyDialect (fullName);

				names.Add (fullName);
			}

			return names.ToArray ();
		}

		private static string applyDialect(string name) {
			int seed = Stage.currentStage.seed;
			Random r = new Random(seed);
			int counter = r.Next() > seed ? 1 : 2;

			string newName = "";
			for (int i = 0; i < name.Length; i++) {
				char l = name[i];

				if (l != ' ' && counter <= 0) {
					string[] d = NameGen.currentDialect[l.ToString().ToLower ()];
					int loc = (int) Math.Round ((double) d.Length * r.NextDouble ()) - 1;
					if (loc < 0) {
						loc = 0;
					}

					string replacement = d[(int) loc];
					newName += replacement;
					counter = ((r.Next() < seed) ? 5 : 7);
				} else {
					newName += l;
				}

				counter--;
			}

			return newName;
		}

		private static void generateDialect() {
			NameGen.currentDialect = new Dictionary<string, string[]>();

			int seed = Stage.currentStage.seed;
			Random r = new Random(seed);
			double seedSm1 = r.NextDouble();
			double seedSm2 = r.NextDouble();
			double seedSm3 = r.NextDouble();
			double seedLr1 = r.NextDouble();
			double seedLr2 = r.NextDouble();

			foreach (KeyValuePair<string, string[]> kv in NameGen.nameFeed.interchangeables) {
				float rootIndex = kv.Value.Length * seed;
				int[] letterDialectIndex = new int[]{ (int) Math.Round (rootIndex),
					(int) Math.Round ((double) kv.Value.Length * seedSm1),
					(int) Math.Round ((double) kv.Value.Length * seedSm2),
					(int) Math.Round ((double) kv.Value.Length * seedSm3),
					(int) Math.Round ((double) kv.Value.Length * seedLr1),
					(int) Math.Round ((double) kv.Value.Length * seedLr2)
				};
				string[] letterDialect = new string[letterDialectIndex.Length];
				for (int i = 0; i < letterDialectIndex.Length; i++) {
					int val = letterDialectIndex[i];
					if (val < 0) {
						val = 0;
					} else if (val > kv.Value.Length - 1) {
						val = kv.Value.Length - 1;
					}

					letterDialect[i] = kv.Value[val];
				}

				NameGen.currentDialect.Add (kv.Key, letterDialect);
			}
		}
	}
}
