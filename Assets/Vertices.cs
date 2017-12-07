using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertices : MonoBehaviour
{

    public GameObject anObject;
	public GameObject a;
    public Collider anObjCollider;
	public static Mesh viewedModel;
    private Camera cam;
    private Plane[] planes;
    // Use this for initialization
    void Start()
    {
		anObject = GameObject.Find("path-4");
		a = GameObject.Find("Car");
		MeshFilter viewedModelFilter = (MeshFilter)anObject.GetComponent("MeshFilter");
		viewedModel=viewedModelFilter.mesh;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = viewedModel.vertices;
		Debug.Log(anObject.transform.position);
		int i = 0;
		while (i < vertices.Length)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        	cube.transform.position = new Vector3(vertices[i].x, vertices[i].y, 0);
			i++;
		}
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
}
