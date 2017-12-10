using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	UnityStandardAssets.Vehicles.Car.CarController m_Car;
	Vertices road;
    List<Vector3> trafficLightsFound = new List<Vector3>();
    List<Vector3> obstaclesFound = new List<Vector3>();

	void Start() {
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.GetComponent<Vertices>();

		transform.position = road.trackedPoints[0];
		Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
		transform.rotation = Quaternion.LookRotation(direction);
        detectLight();
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

	void detectLight() {
		GameObject[] traficLights =  GameObject.FindGameObjectsWithTag("TrafficLight");
        RaycastHit hit;
        for(int i = 0; i < traficLights.Length; ++i) {
            GameObject currTrafic = traficLights[i];
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(currTrafic.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			if(Physics.Raycast(ray, out hit))
			{
                if(currTrafic.GetComponent<TrafficLightBehavior>().lightColor != 0){
                    trafficLightsFound.Add(getLightTrack(currTrafic.transform.position,currTrafic.transform.forward));
                }
			}
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		    cube.transform.position = getLightTrack(currTrafic.transform.position,currTrafic.transform.forward);
        }
	}

    Vector3 getLightTrack(Vector3 lightPosition,Vector3 rotation) {
        Vector3 lightPoint = road.trackedPoints[0];
        float minDist = Vector3.Distance(lightPosition,road.trackedPoints[0]);
        for(int i = 1; i < road.trackedPoints.Count;++i) {
            Vector3 currPoint = road.trackedPoints[i];
            float curr = -rotation.z * (currPoint.x - lightPosition.x) + rotation.x * (currPoint.z - lightPosition.z);
            if(Mathf.Abs(curr) < 0.5) {
                float currDist = Vector3.Distance(lightPosition,currPoint);
                if(currDist < minDist) {
                    lightPoint = currPoint;
                    minDist = currDist;
                }
            }
        }
        return lightPoint;
    }

	void Awake() {
		m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
	}

	void FixedUpdate() {
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
