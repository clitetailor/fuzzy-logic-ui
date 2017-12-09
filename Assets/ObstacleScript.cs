using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

	private new Camera camera;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			ClickEventHandler();
		}
	}

	void ClickEventHandler() {
		RaycastHit hit;

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform.name == "Plane") {
				CreateNewObstacle(hit.point);
			} else if (hit.transform.gameObject.tag == "Obstacle") {
				RemoveObstacle(hit.transform.gameObject);
			} else {
				//Pass
			}
			
		}	
	}		
			
	void CreateNewObstacle(Vector3 position) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

		cube.transform.position = position;
		cube.tag = "Obstacle";

		Renderer rend = cube.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
	}

	void RemoveObstacle(GameObject gameObject) {
		Destroy(gameObject);
	}
}
