using UnityEngine;
using System.Collections;

namespace Armage
{
	public class Sun : MonoBehaviour {
		public Transform atmosphere;
		public int bufferSpace = 1200;
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			this.transform.Rotate (0, 2 * Time.deltaTime, 0);
			atmosphere.Rotate (0, -3 * Time.deltaTime, 0);
		}
	}
}
