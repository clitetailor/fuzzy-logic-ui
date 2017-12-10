using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	UnityStandardAssets.Vehicles.Car.CarController m_Car;
	Vertices road;

	void Start() {
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.GetComponent<Vertices>();

		transform.position = road.trackedPoints[0];
		Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
		transform.rotation = Quaternion.LookRotation(direction);
		
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void getObjectInView()
	{
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		RaycastHit hit;
		for(var i = 0; i < obstacles.Length; i++)
		{
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(obstacles[i].transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			if(Physics.Raycast(ray, out hit))
			{
				Debug.Log(isObstacleInRoad(obstacles[i].transform.position));
			}
		}
	}

	bool isObstacleInRoad(Vector3 obstaclePoint) {
        int minPoint = 0;
        float minDistance = Vector3.Distance(obstaclePoint,road.trackedPoints[0]);
        for(int i = 1; i < road.trackedPoints.Count; ++i) {
            float curr = Vector3.Distance(obstaclePoint,road.trackedPoints[i]);
            if(curr < minDistance) {
                minPoint = i;
                minDistance = curr;
            }
        }
        if(minDistance > (road.roadWidth/2)) {
            return false;
        }
        return true;
    }

	void Awake() {
		m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
	}

	void FixedUpdate() {
        getObjectInView();
		// pass the input to the car!
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		#if !MOBILE_INPUT
			float handbrake = CrossPlatformInputManager.GetAxis("Jump");
			m_Car.Move(h, v, v, handbrake);
		#else
			m_Car.Move(h, v, v, 0f);
		#endif
	}
}
