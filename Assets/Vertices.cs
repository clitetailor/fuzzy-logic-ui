using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Vertices : MonoBehaviour
{
    private GameObject pathPlane;
    private Mesh roadMesh;
    private int MAX_TRAFFIC_LIGHT = 4;

    [HideInInspector] public List<Vector3> trackedPoints = new List<Vector3>();
    [HideInInspector] public List<GameObject> trackedNavigators = new List<GameObject>();

    [HideInInspector]
    public List<List<Vector3>> vertexTriples = new List<List<Vector3>>();
    public float roadWidth;

    // Use this for initialization
    void Start()
    {
		pathPlane = GameObject.Find("Plane.001");

		roadMesh = pathPlane.GetComponent<MeshFilter>().mesh;
		List<Vector3> vertices = new List<Vector3>(roadMesh.vertices);
		List<int> triangles = new List<int>(roadMesh.triangles);

		// To absolute positions
		vertices = vertices
			.Select(vertex => pathPlane.transform.TransformPoint(vertex))
			.ToList();

		List<Vector3> trafficLights = GetExistingTrafficLights(vertices);
		CreateRandomTrafficLights(ref trafficLights, vertices);

		int numberOfTriangles = triangles.Count / 3;

		for (int i = 0; i < numberOfTriangles; ++i)
		{
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

			// Find the shared edge of two consecutive triangles
			List<Vector3> edgePoints = new List<Vector3>();
			foreach (Vector3 vertex in next)
			{
				if (curr.IndexOf(vertex) != -1)
				{
					edgePoints.Add(vertex);
				}
			}

			// Find the edge midpoint
			if (edgePoints.Count == 2)
			{
				for (int j = 1; j < trafficLights.Count; ++j)
				{
					if (edgePoints[0] == trafficLights[0])
					{
						//Debug.Log(1);
						Vector3 position = edgePoints[0];
						Vector3 rotation;
						rotation = edgePoints[1] - edgePoints[0];
						Instantiate(GameObject.FindGameObjectsWithTag("TrafficLight")[0], position, Quaternion.LookRotation(rotation));
						trafficLights[j] = Vector3.zero;
					}

					if (edgePoints[1] == trafficLights[j])
					{
						//Debug.Log(2);
						Vector3 position = edgePoints[1];
						Vector3 rotation;
						rotation = edgePoints[1] - edgePoints[0];
						Instantiate(GameObject.FindGameObjectsWithTag("TrafficLight")[0], position, Quaternion.LookRotation(rotation));
						trafficLights[j] = Vector3.zero;
					}
				}
				trackedPoints.Add((edgePoints[0] + edgePoints[1]) / 2);

				float currWidth = Vector3.Distance(edgePoints[0], edgePoints[1]);
				if (roadWidth < currWidth)
				{
					roadWidth = currWidth;
				}
			}
		}

		for (int i = 0; i < trackedPoints.Count; ++i)
		{
			Vector3 currPoint = trackedPoints[i];
			Vector3 nextPoint = trackedPoints[(i + 1) % trackedPoints.Count];

			GameObject navigator = new GameObject("TrackedPoint");
			navigator.transform.position = currPoint;
			navigator.transform.rotation = Quaternion.LookRotation(nextPoint - currPoint);
			navigator.AddComponent<BoxCollider>();
			Collider collider = navigator.GetComponent<BoxCollider>();
			collider.isTrigger = true;

			trackedNavigators.Add(navigator);
		}
    }

    List<Vector3> GetExistingTrafficLights(List<Vector3> vertices)
    {
		List<GameObject> trafficLights = new List<GameObject>(GameObject.FindGameObjectsWithTag("TrafficLight"));
		List<Vector3> trafficPositions = new List<Vector3>();

		foreach (GameObject trafficLight in trafficLights)
		{
			trafficPositions.Add(FindTrafficLightPos(trafficLight.transform.position, vertices));
		}
		return trafficPositions;
	}

	Vector3 FindTrafficLightPos(Vector3 lightPosition, List<Vector3> vertices)
	{
		Vector3 position = Vector3.zero;
		float minDist = Mathf.Infinity;
    
		foreach (Vector3 vertex in vertices)
		{
			float currDist = Vector3.Distance(lightPosition, vertex);
			if (currDist < minDist)
			{
				minDist = currDist;
				position = vertex;
			}
		}
		return position;
    }

    void CreateRandomTrafficLights(ref List<Vector3> trafficLights, List<Vector3> vertices)
    {
		while (trafficLights.Count < MAX_TRAFFIC_LIGHT)
		{
			bool add = true;
			int random = UnityEngine.Random.Range(0, vertices.Count);
			
			for (int i = 0; i < trafficLights.Count; ++i)
			{
				float dist = Vector3.Distance(trafficLights[i], vertices[random]);
				if (dist < 50)
				{
					add = false;
				}
			}

			if (add)
			{
				trafficLights.Add(vertices[random]);
			}
		}
    }
}
