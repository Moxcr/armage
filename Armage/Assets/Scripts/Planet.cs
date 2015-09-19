using UnityEngine;
using System.Collections;

namespace Armage {
	public class Planet : MonoBehaviour {
		public GameObject land = null;
		public GameObject ocean = null;
		public int bufferSpace = 50;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void SetLand(GameObject land) {
			this.land = land;
			this.SetName ("Planet");
		}

		public void SetOcean(GameObject ocean) {
			this.ocean = ocean;
			this.ocean.name = "Ocean";
			this.ocean.transform.SetParent (this.gameObject.transform);
		}

		public void SetName(string planetName) {
			this.land.gameObject.name = planetName;
		}
	}
}