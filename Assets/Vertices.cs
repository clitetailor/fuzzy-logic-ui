using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertices : MonoBehaviour {
    private GameObject pathPlane;
    private Collider anObjCollider;
	private Mesh viewedModel;

    // Use this for initialization
    void Start() {
		pathPlane = GameObject.Find("Plane.001");

		MeshFilter viewedModelFilter = (MeshFilter) pathPlane.GetComponent("MeshFilter");
		viewedModel = viewedModelFilter.mesh;
		Vector3[] vertices = viewedModel.vertices;
        int[] triangles = viewedModel.triangles;
		Vector3 scale = pathPlane.transform.localScale;
		Quaternion rotation = pathPlane.transform.localRotation;
        Vector3[] filteredVirtice = new Vector3[vertices.Length];
		int i = 0;

		while(i < vertices.Length) {
			Vector3 worldPoint = transform.TransformPoint(vertices[i]);
			Vector3 centerWorldPoint = pathPlane.transform.position;
			float distance = Vector3.Distance(centerWorldPoint,worldPoint);

			if(distance < 1000) {
				if (distance > 1){
                    filteredVirtice[i] = new Vector3(worldPoint.x, 0, worldPoint.z);
					i++;
				} else {
                    i++;
                }                    
			} else {
                i++;
            }
		}

        for(int j = 1; j < triangles.Length; ++j) {
            int index1 = triangles[j],
                index2 = triangles[j-1];

            Vector3 worldPoint = filteredVirtice[index1],
                    worldPoint1 = filteredVirtice[index2];

            if (worldPoint != Vector3.zero){
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3((worldPoint.x + worldPoint1.x)/2, 0, (worldPoint.z + worldPoint1.z)/2);
			    cube.name = j.ToString();
            }
        }
	}
}
