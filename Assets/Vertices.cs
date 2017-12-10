using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Vertices : MonoBehaviour {
    private GameObject pathPlane;
	private Mesh roadMesh;
    private int MAX_TRAFFIC_LIGHT = 4;

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

        List<Vector3> trafficLights = getExistedTrafficLight(vertices);
        randomTrafficLight(trafficLights, vertices);

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
                for(int j = 1; j < trafficLights.Count; ++j) {
                    if(edgePoints[0] == trafficLights[j]) {
                        Vector3 position = edgePoints[0];
                        Vector3 rotation;
                        rotation = edgePoints[1] - edgePoints[0];
                        Instantiate(GameObject.FindGameObjectsWithTag("TrafficLight")[0], position, Quaternion.LookRotation(rotation));
                        trafficLights[j] = Vector3.zero;
                    } 
                    if(edgePoints[1] == trafficLights[j]) {
                        Vector3 position = edgePoints[1];
                        Vector3 rotation;
                        rotation = edgePoints[0] - edgePoints[1];
                        Instantiate(GameObject.FindGameObjectsWithTag("TrafficLight")[0], position, Quaternion.LookRotation(rotation));
                        trafficLights[j] = Vector3.zero;
                    }
                }
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

    List<Vector3> getExistedTrafficLight(List<Vector3> vertices) {
        GameObject[] trafficLights = GameObject.FindGameObjectsWithTag("TrafficLight");
        List<Vector3> trafficPosition = new List<Vector3>();
        for(int i = 0; i < trafficLights.Length;++i) {
            trafficPosition.Add(findTrafficLight(trafficLights[i].transform.position,vertices));
        }
        return trafficPosition;
    }

    Vector3 findTrafficLight(Vector3 lightPosition,List<Vector3> vertices) {
        Vector3 position = Vector3.zero;
        float minDist = Mathf.Infinity;
        for(int i = 0; i < vertices.Count; ++i) {
            float currDist = Vector3.Distance(lightPosition,vertices[i]);
            if(currDist < minDist) {
                minDist = currDist;
                position = vertices[i];
            }
        }
        return position;
    }

    void randomTrafficLight(List<Vector3> trafficLights,List<Vector3> vertices) {
        while(trafficLights.Count < MAX_TRAFFIC_LIGHT) {
            bool add = true;
            int random = UnityEngine.Random.Range(0,vertices.Count);
            for(int i = 0; i < trafficLights.Count;++i) {
                float dist = Vector3.Distance(trafficLights[i],vertices[random]);
                if(dist < 50) {
                    add = false;
                }
            }
            if(add){
                trafficLights.Add(vertices[random]);
            }
        }
    }
}
