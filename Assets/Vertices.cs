using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Vertices : MonoBehaviour {
    private GameObject pathPlane;
	private Mesh roadMesh;

	Vector3[] vertices;
	int[] triangles;

	List<Vector3> trackedPoints = new List<Vector3>();

	int numberOfTriangles = 0;

	ArrayList vertexTriples = new ArrayList();

	// Use this for initialization
	void Start() {
		pathPlane = GameObject.Find("Plane.001");

		roadMesh = pathPlane.GetComponent<MeshFilter>().mesh;
		vertices = roadMesh.vertices;
        triangles = roadMesh.triangles;
		

		Vector3 roadCenter = pathPlane.transform.position;

		Vector3[] absolutePositions = new Vector3[vertices.Length];

		for (int i = 0; i < vertices.Length; ++i) {
			Vector3 vertex = transform.TransformPoint(vertices[i]);
			absolutePositions[i] = new Vector3(vertex.x, 0, vertex.z);
		}

		vertices = absolutePositions;
		
		numberOfTriangles = triangles.Length / 3;

		for (int i = 0; i < numberOfTriangles; ++i) {
			int startIndex = i * 3;
			Vector3[] triple = {
				vertices[ triangles[startIndex] ],
				vertices[ triangles[startIndex + 1] ],
				vertices[ triangles[startIndex + 2] ]
			};

			vertexTriples.Add(triple);
		}

		for (int i = 0; i < numberOfTriangles; ++i)
		{
			Vector3[] curr = (Vector3[])vertexTriples[i];
			Vector3[] next = (Vector3[])vertexTriples[(i + 1) % numberOfTriangles];

			ArrayList edgePoints = new ArrayList();
			foreach (Vector3 vertex in next) {
				if (Array.IndexOf(curr, vertex) != -1) {
					edgePoints.Add(vertex);
				}
			}

			if (edgePoints.Count == 2) {
				trackedPoints.Add(((Vector3) edgePoints[0] + (Vector3) edgePoints[1]) / 2);
			}
		}

		foreach (Vector3 point in trackedPoints) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = point;
		}
	}
}
