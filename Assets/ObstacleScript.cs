using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

	private new Camera camera;
	private GameObject car;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
		car = transform.parent.gameObject;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			OnMouseLeftClick();
		}

		if (Input.GetMouseButtonDown(1)) {
			OnMouseRightClick();
		}
	}

	void OnMouseLeftClick() {
		RaycastHit hit;

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
		{
			CreateNewObstacle(hit.point);
		}
	}		
			
	void CreateNewObstacle(Vector3 position) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

		cube.transform.position = position;
		cube.tag = "Obstacle";
		cube.transform.rotation.SetFromToRotation(Vector3.zero, new Vector3(0, 0, camera.transform.rotation.z));
		
		cube.transform.rotation *= car.transform.rotation;

		Renderer rend = cube.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
		rend.material.color = Color.blue;
	}

	void OnMouseRightClick() {
		RaycastHit hit;

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.gameObject.tag == "Obstacle")
			{
				RemoveObstacle(hit.transform.gameObject);
			}
		}
	}

	void RemoveObstacle(GameObject gameObject) {
		Destroy(gameObject);
	}
}
