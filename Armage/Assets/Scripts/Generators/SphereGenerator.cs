using UnityEngine;
using System.Collections.Generic;
using System;
namespace Armage
{
	public static class SphereGenerator
	{
		public static GameObject GenPrimitive(SphereProperties initProps) {
			GameObject obj = new GameObject();
			obj.name = "SpherePrimitive";
			MeshFilter filter = obj.AddComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			mesh.name = "SphereMesh";
			mesh.Clear();
			
			float radius = initProps.radius; //1f
			// Longitude |||
			int nbLong = initProps.longitude;  //24
			// Latitude ---
			int nbLat = initProps.latitude;  //16
			
			#region Vertices
			Vector3[] vertices = new Vector3[(nbLong+1) * nbLat + 2];
			float _pi = Mathf.PI;
			float _2pi = _pi * 2f;
			
			vertices[0] = Vector3.up * radius;
			for( int lat = 0; lat < nbLat; lat++ )
			{
				float a1 = _pi * (float)(lat+1) / (nbLat+1);
				float sin1 = Mathf.Sin(a1);
				float cos1 = Mathf.Cos(a1);
				
				for( int lon = 0; lon <= nbLong; lon++ )
				{
					float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
					float sin2 = Mathf.Sin(a2);
					float cos2 = Mathf.Cos(a2);
					
					vertices[ lon + lat * (nbLong + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * radius;
				}
			}
			vertices[vertices.Length-1] = Vector3.up * -radius;
			#endregion
			
			#region Normals		
			Vector3[] normals = new Vector3[vertices.Length];
			for( int n = 0; n < vertices.Length; n++ )
				normals[n] = vertices[n].normalized;
			#endregion
			
			#region UVs
			Vector2[] uvs = new Vector2[vertices.Length];
			uvs[0] = Vector2.up;
			uvs[uvs.Length-1] = Vector2.zero;
			for( int lat = 0; lat < nbLat; lat++ )
				for( int lon = 0; lon <= nbLong; lon++ )
					uvs[lon + lat * (nbLong + 1) + 1] = new Vector2( (float)lon / nbLong, 1f - (float)(lat+1) / (nbLat+1) );
			#endregion
			
			#region Triangles
			int nbFaces = vertices.Length;
			int nbTriangles = nbFaces * 2;
			int nbIndexes = nbTriangles * 3;
			int[] triangles = new int[ nbIndexes ];
			
			//Top Cap
			int i = 0;
			for( int lon = 0; lon < nbLong; lon++ )
			{
				triangles[i++] = lon+2;
				triangles[i++] = lon+1;
				triangles[i++] = 0;
			}
			
			//Middle
			for( int lat = 0; lat < nbLat - 1; lat++ )
			{
				for( int lon = 0; lon < nbLong; lon++ )
				{
					int current = lon + lat * (nbLong + 1) + 1;
					int next = current + nbLong + 1;
					
					triangles[i++] = current;
					triangles[i++] = current + 1;
					triangles[i++] = next + 1;
					
					triangles[i++] = current;
					triangles[i++] = next + 1;
					triangles[i++] = next;
				}
			}
			
			//Bottom Cap
			for( int lon = 0; lon < nbLong; lon++ )
			{
				triangles[i++] = vertices.Length - 1;
				triangles[i++] = vertices.Length - (lon+2) - 1;
				triangles[i++] = vertices.Length - (lon+1) - 1;
			}
			#endregion
			
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
			
			mesh.RecalculateBounds();
			mesh.Optimize();

			return obj;
		}

		public static void ApplyNoise(GameObject sphere, int scaleFactor) {
			MeshFilter filter = sphere.GetComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;

			for (int i = 0; i < vertices.Length; i++) {
				float noise = SimplexNoise.Noise.Generate (vertices[i].x, vertices[i].y, vertices[i].z);
				vertices[i] += normals[i] * (noise - Mathf.Sin(vertices[i].x * vertices[i].z)) / 10 * (scaleFactor / 3);
			}

			filter.mesh.vertices = vertices;
		}

		public static void ApplyHeightMap(GameObject sphere, float scaleFactor, Texture2D heightTex, float cutoff = 0f) {
			MeshFilter filter = sphere.GetComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			Vector3[] vertices = mesh.vertices;
			Vector2[] uvs = mesh.uv;
			Vector3[] normals = mesh.normals;
			for (int i = 0; i < vertices.Length; i++) {
				float grayscale = heightTex.GetPixelBilinear(uvs[i].x, uvs[i].y).grayscale;
				if (grayscale >= cutoff) {
					vertices[i] += normals[i] * grayscale * scaleFactor;
				}
			}

			filter.mesh.vertices = vertices;
		}

		public static void ApplyHeightMap(GameObject sphere, float scaleFactor, float[,] heights, float cutoff = 0f) {
			MeshFilter filter = sphere.GetComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			Vector3[] vertices = mesh.vertices;
			Vector2[] uvs = mesh.uv;
			Vector3[] normals = mesh.normals;

			int heightXlen = heights.GetLength (0);
			int heightYlen = heights.GetLength (1);
			for (int i = 0; i < uvs.Length; i++) {
				int x = Mathf.RoundToInt ((heightXlen - 1f) * uvs[i].x);
				int y = Mathf.RoundToInt ((heightYlen - 1f) * uvs[i].y);
				float height = heights[x, y];
				if (height >= cutoff) {
					vertices[i] += normals[i] * height * scaleFactor;
				}
			}
			
			filter.mesh.vertices = vertices;
		}

		public static void Subdivide(GameObject sphere, Texture2D heightTex = null, int subLevel = 1) {
			MeshFilter filter = sphere.GetComponent<MeshFilter>();
			Mesh mesh = filter.mesh;
			List<Vector3> vertices = new List<Vector3> (mesh.vertices);
			List<Vector3> normals = new List<Vector3> (mesh.normals);
			List<Vector2> uvs = new List<Vector2> (mesh.uv);
			List<int> tris = new List<int>(mesh.triangles);

			int[] levels = new int[mesh.uv.Length];

			if (heightTex != null) {
				for (int i = 0; i < mesh.uv.Length; i++) {
					float grayscale = heightTex.GetPixelBilinear(mesh.uv[i].x, mesh.uv[i].y).grayscale;
					levels[i] = 0;
					if ((subLevel == 1 && (grayscale <= 0.3f || grayscale >= 0.7f)) || (subLevel == 2 && (grayscale <= 0.15f || grayscale >= 0.95f))) {
						levels[i] = 1;
					}
				}
			}

			for (int i = 2; i < mesh.triangles.Length; i+=3) {
				int p0trigwhomi = mesh.triangles[i-2];
				int p1trigwhomi = mesh.triangles[i-1];
				int p2trigwhomi = mesh.triangles[i];

				if (levels != null && levels[p0trigwhomi] == 0) {
					continue;
				}


				int p0trigwheremi = i-2;
				int p1trigwheremi = i-1;
				int p2trigwheremi = i;
				
				Vector3 p0mi = mesh.vertices[p0trigwhomi];
				Vector3 p1mi = mesh.vertices[p1trigwhomi];
				Vector3 p2mi = mesh.vertices[p2trigwhomi];
				
				Vector3 p0mn = mesh.normals[p0trigwhomi];
				Vector3 p1mn = mesh.normals[p1trigwhomi];
				Vector3 p2mn = mesh.normals[p2trigwhomi];
				
				Vector2 p0mu = mesh.uv[p0trigwhomi];
				Vector2 p1mu = mesh.uv[p1trigwhomi];
				Vector2 p2mu = mesh.uv[p2trigwhomi];
				
				Vector3 p0modmi = (p0mi+p1mi+p2mi)/3;
				Vector3 p0modmn = ((p0mn+p1mn+p2mn)/3).normalized;
				Vector3 p0modmu = (p0mu+p1mu+p2mu)/3;	
				
				int p0modtrigwhomi = vertices.Count;
				
				vertices.Add(p0modmi);
				normals.Add(p0modmn);
				uvs.Add(p0modmu);
				
				tris[p0trigwheremi] = p0trigwhomi;
				tris[p1trigwheremi] = p1trigwhomi;
				tris[p2trigwheremi] = p0modtrigwhomi;
				
				tris.Add(p0modtrigwhomi);
				tris.Add(p1trigwhomi);
				tris.Add(p2trigwhomi);
				
				tris.Add(p0trigwhomi);
				tris.Add(p0modtrigwhomi);
				tris.Add(p2trigwhomi);
			}

			mesh.vertices = vertices.ToArray ();
			mesh.normals = normals.ToArray ();
			mesh.uv = uvs.ToArray ();
			mesh.triangles = tris.ToArray ();
		}
	}
}
