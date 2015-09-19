using UnityEngine;
using System.Collections.Generic;

namespace Armage
{
	public class Game : MonoBehaviour {
		public static Game currentGame;

		// Use this for initialization
		void Start () {
			Game.currentGame = this;

			int seed = UnityEngine.Random.Range(0, int.MaxValue);
			UnityEngine.Random.seed = seed;

			Stage stage = new Stage(seed);
			stage.SetAsCurrentStage();
			GameObject ssPF = Resources.Load<GameObject>("Prefabs/StarSystem");
			GameObject ss = (GameObject) Instantiate(ssPF, ssPF.transform.position, ssPF.transform.rotation);
			ss.name = "Solar System";
			stage.StarSystem = ss.GetComponent<StarSystem>();

			stage.StarSystem.numPlanets = 5;//UnityEngine.Random.Range (1, 4);

			/*planet = PlanetGen.GeneratePlanet("Earth", 
			                                         new PlanetProperties("Materials/Terrain/Diffuse",
	                                                       "Textures/Terrain/Earth",
	                                                       "Textures/Terrain/Earth-heightmap",
	                                                       null,
			                     						   1.5f,
	                                                       1), 
			                                         PlanetGen.defaultOceanProps);*/



			PlanetGen.GenerateStarSystem();
		}
		
		// Update is called once per frame
		void Update () {

		}
	}
}