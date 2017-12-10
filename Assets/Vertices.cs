using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Vertices : MonoBehaviour {
    private GameObject pathPlane;
	private Mesh roadMesh;

	[HideInInspector]
	public List<Vector3> trackedPoints = new List<Vector3>();

	[HideInInspector]
	public List<List<Vector3>> vertexTriples = new List<List<Vector3>>();
    public float roadWidth;

	// Use this for initialization
	void Start() {
		pathPlane = GameObject.Find("Plane.001");

		roadMesh = pathPlane.GetComponent<MeshFilter>().mesh;
		List<Vector3> vertices = new List<Vector3>(roadMesh.vertices);
        List<int> triangles = new List<int>(roadMesh.triangles);
		
		// To absolute positions
		vertices = vertices
			.Select(vertex => pathPlane.transform.TransformPoint(vertex))
			.ToList();

		int numberOfTriangles = triangles.Count / 3;

		for (int i = 0; i < numberOfTriangles; ++i) {
			int startIndex = i * 3;

			List<Vector3> triple = new List<Vector3>(new Vector3[] {
				vertices[ triangles[startIndex] ],
				vertices[ triangles[startIndex + 1] ],
				vertices[ triangles[startIndex + 2] ]
			});

			vertexTriples.Add(triple);
		}

		for (int i = 0; i < numberOfTriangles; ++i)
		{
			List<Vector3> curr = vertexTriples[i];
			List<Vector3> next = vertexTriples[(i + 1) % numberOfTriangles];

			// Find the shared edge of two consecutive triangle
			List<Vector3> edgePoints = new List<Vector3>();
			foreach (Vector3 vertex in next) {
				if (curr.IndexOf(vertex) != -1) {
					edgePoints.Add(vertex);
				}
			}

			// Find the edge midpoint
			if (edgePoints.Count == 2) {
				trackedPoints.Add((edgePoints[0] + edgePoints[1]) / 2);
                float currWidth = Vector3.Distance(edgePoints[0],edgePoints[1]);
                if(roadWidth < currWidth) {
                    roadWidth = currWidth;
                }
			}
		}

		// foreach (Vector3 vertex in trackedPoints) {
		// 	GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		// 	cube.transform.position = vertex;
		// }
	}
}
