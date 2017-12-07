using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertices : MonoBehaviour
{

    private GameObject pathPlane;
    private Collider anObjCollider;
	private Mesh viewedModel;

    // Use this for initialization
    void Start()
    {
		pathPlane = GameObject.Find("Plane.001");

		MeshFilter viewedModelFilter = (MeshFilter) pathPlane.GetComponent("MeshFilter");
		viewedModel = viewedModelFilter.mesh;
		
		Vector3[] vertices = viewedModel.vertices;
		Vector3 scale = pathPlane.transform.localScale;
		Quaternion rotation = pathPlane.transform.localRotation;

		int i = 0;
		while (i < vertices.Length)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        	cube.transform.position = new Vector3(vertices[i].x * scale.x + 22, 0, - vertices[i].y * scale.y);
			i++;
		}
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
}
