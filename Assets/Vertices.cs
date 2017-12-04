using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertices : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Debug.Log(mesh.vertices.Length);
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length)
		{
			Debug.Log(vertices[i].x);
			i++;
		}
	}
}
