using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        for(int j = 1; j < (triangles.Length - 2); ++j) {
            int index1 = triangles[j],
                index2 = triangles[j-1],
                index3 = triangles[j+1];

            Vector3 worldPoint = filteredVirtice[index1],
                    worldPoint1 = filteredVirtice[index2],
                    worldPoint2 = filteredVirtice[index3];

            if (worldPoint1 != Vector3.zero){
                int[] paragram = {index2,index1,index3}; 
                int oppoVertice = getNotSameSideVertice(filteredVirtice, new int[3] {index2,index1,index3});
                if(oppoVertice == index1) {
                    Vector3 firstEdge = new Vector3((worldPoint.x + worldPoint1.x)/2,0,(worldPoint.z + worldPoint1.z)/2);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(firstEdge.x, 0, firstEdge.z);
			        cube.name = index2.ToString();
                    Vector3 seconEdge = new Vector3((worldPoint.x + worldPoint2.x)/2,0,(worldPoint.z + worldPoint2.z)/2);
                    GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.position = new Vector3(seconEdge.x, 0, seconEdge.z);
			        cube1.name = index2.ToString();
                }
                if(oppoVertice == index2) {
                    Vector3 firstEdge = new Vector3((worldPoint1.x + worldPoint.x)/2,0,(worldPoint1.z + worldPoint.z)/2);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(firstEdge.x, 0, firstEdge.z);
			        cube.name = index2.ToString();
                    Vector3 seconEdge = new Vector3((worldPoint1.x + worldPoint2.x)/2,0,(worldPoint1.z + worldPoint2.z)/2);
                    GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.position = new Vector3(seconEdge.x, 0, seconEdge.z);
			        cube1.name = index2.ToString();
                }
                if(oppoVertice == index3) {
                    Vector3 firstEdge = new Vector3((worldPoint2.x + worldPoint.x)/2,0,(worldPoint2.z + worldPoint.z)/2);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(firstEdge.x, 0, firstEdge.z);
			        cube.name = index2.ToString();
                    Vector3 seconEdge = new Vector3((worldPoint1.x + worldPoint2.x)/2,0,(worldPoint1.z + worldPoint2.z)/2);
                    GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.position = new Vector3(seconEdge.x, 0, seconEdge.z);
			        cube1.name = index2.ToString();
                }
            }
        }
	}

	int getNotSameSideVertice(Vector3[] vertices,int[] indexs) {
		Vector3 firstVertice = vertices[indexs[0]],
                secondVertice = vertices[indexs[1]],
                thirtVertice = vertices[indexs[2]];
        float firstThird = Vector3.Distance(firstVertice,thirtVertice),
              firstSecond = Vector3.Distance(firstVertice,secondVertice),
              secondThird = Vector3.Distance(secondVertice,thirtVertice);
        float[] distance = new float[3] {firstSecond,firstThird,secondThird};
        float minDistance = distance.Min();
        if(firstSecond == minDistance){
            return indexs[2];
	    } else if(firstThird == minDistance){
            return indexs[1];
        } else if(secondThird == minDistance){
            return indexs[0];
        }
        return -1;
    }
}
