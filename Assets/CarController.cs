using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using FuzzyLogic;

[RequireComponent(typeof(UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour
{
    UnityStandardAssets.Vehicles.Car.CarController m_Car;
    Vertices road;
    List<Vector3> trafficLightsFound = new List<Vector3>();
    List<Vector3> obstaclesOnScreen = new List<Vector3>();

	List<TrafficLightBehavior> trafficLights;

    void Start()
    {
        GameObject path = GameObject.Find("Path").gameObject;
        road = path.GetComponent<Vertices>();

        transform.position = road.trackedPoints[0];
        Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
        transform.rotation = Quaternion.LookRotation(direction);

		trafficLights = GameObject.FindGameObjectsWithTag("TrafficLight")
			.Select(gameObject => gameObject.GetComponent<TrafficLightBehavior>())
			.ToList();
    }

    void Awake()
    {
        m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
    }


    private void FixedUpdate()
    {
        // pass the input to the car!
        List<GameObject> trackPoints;
        trackPoints = FrontTrackPoints(10);
        float desiredAngle = RotateAngle(trackPoints);

        float h = desiredAngle / 30;
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

		float speed = CaculateSpeed(desiredAngle);

		Debug.Log(speed);

    #if !MOBILE_INPUT
        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
        m_Car.Move(h, speed, v, handbrake);
    #else
            m_Car.Move(h, v, v, 0f);
    #endif
    }

	private float CaculateSpeed(float roadAngle) {
		TrafficLightBehavior trafficLight = DetectTrafficLight();
		GameObject obstacle = NearestObstacle();

		float offsetObstacle = Mathf.Infinity;
		if (obstacle != null) {
			offsetObstacle = Vector3.Distance(obstacle.transform.position, transform.position);
		}

		float offsetTrafficLight = Mathf.Infinity;
		if (trafficLight != null) {
			offsetTrafficLight = Vector3.Distance(trafficLight.transform.position, transform.position);
		}

		if (obstacle == null && trafficLight == null) {
			Debug.Log("Both");
			return 0.1f;
		}

		bool isLight = offsetTrafficLight < offsetObstacle ? true : false;
		
		if (offsetTrafficLight < offsetObstacle)
		{
			Debug.Log("TrafficLight");
			object[] lightStatus;

			switch (trafficLight.lightColor) {
				case (TrafficLightBehavior.LightColor.GREEN_LIGHT): {
						lightStatus = new object[2] { "Green", (double) trafficLight.countDown };
						break;
				}

				case (TrafficLightBehavior.LightColor.YELLOW_LIGHT): {
						lightStatus = new object[2] { "Yellow", (double) trafficLight.countDown };
						break;
				}

				case (TrafficLightBehavior.LightColor.RED_LIGHT): {
						lightStatus = new object[2] { "Red", (double) trafficLight.countDown };
						break;
				}

				default: {
						lightStatus = new object[2] { "Green", trafficLight.countDown };
						break;
				}
			}

			return (float) Program.CalculateSpeed(true, offsetTrafficLight, roadAngle, lightStatus);
		} else
		{
			Debug.Log("Obstacle");
			return (float) Program.CalculateSpeed(false, offsetObstacle, roadAngle, null);
		}
	}

    private List<GameObject> FrontTrackPoints(int radius)
    {
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, radius));

        return colliders
			.Where(collider =>
			{
				Vector3 colliderOffset = collider.transform.position - transform.position;
				Quaternion lookRotation = Quaternion.LookRotation(colliderOffset);

				return collider.gameObject.name == "TrackedPoint"
					&& Quaternion.Angle(lookRotation, transform.rotation) < 90
					&& Vector3.Distance(transform.position, collider.gameObject.transform.position) > 0f;
			})
			.Select(collider => collider.gameObject)
			.ToList();
    }

    private float RotateAngle(List<GameObject> trackPoints)
    {
        List<float> amounts = trackPoints
			.Select(point =>
				Vector3.Distance(point.transform.position, transform.position))
			.Select(length => 1f / (Mathf.Abs(length - 5) + 1))
			.ToList();

        float sum = 0;
        for (int i = 0; i < amounts.Count; ++i)
        {
			sum += amounts[i];
        }

        float accum = 0;
        for (int i = 0; i < trackPoints.Count; ++i)
        {
			Quaternion rotation = Quaternion.LookRotation(trackPoints[i].transform.position - transform.position);
			float carY = transform.rotation.eulerAngles.y;
			float roadY = rotation.eulerAngles.y;

			Modular(ref carY);
			Modular(ref roadY);

			float deltaY = roadY - carY;
			Modular(ref deltaY);
			accum += deltaY * amounts[i] / sum;
        }

        float angle = accum;

        return angle;
    }

    private void Modular(ref float angle)
    {
        while (angle < -180f)
        {
			angle += 360f;
        }

        while (angle > 180f)
        {
			angle -= 360f;
        }
    }


	private GameObject NearestObstacle()
	{
		obstaclesOnScreen = new List<Vector3>();

		List<GameObject> obstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Obstacle"));
		RaycastHit hit;

		float minDist = Mathf.Infinity;
		GameObject nearestObstacle = null;
		foreach (GameObject obstacle in obstacles)
		{
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(obstacle.transform.position);
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);

			if (Physics.Raycast(ray, out hit))
			{
				if (IsInTheRoad(obstacle.transform.position))
				{
					float dist = Vector3.Distance(obstacle.transform.position, transform.position);
					if (dist < minDist)
					{
						minDist = dist;
						nearestObstacle = obstacle;
					}
				}
			}
		}

		return nearestObstacle;
	}

	private bool IsInTheRoad(Vector3 pos)
	{
		List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(pos, road.roadWidth / 2));

		foreach (Collider collider in colliders)
		{
			if (collider.name == "TrackedPoint")
			{
				return true;
			}
		}
		return false;
	}


	private TrafficLightBehavior DetectTrafficLight()
	{
		RaycastHit hit;
		float minDist = Mathf.Infinity;
		TrafficLightBehavior nearestTrafficLight = null;

		foreach (TrafficLightBehavior trafficLight in trafficLights)
		{
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(trafficLight.transform.position);
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);

			if (Physics.Raycast(ray, out hit))
			{
				float dist = Vector3.Distance(transform.position, trafficLight.transform.position);
				if (dist < minDist) {
					minDist = dist;
					nearestTrafficLight = trafficLight;
				}
			}
		}

		return nearestTrafficLight;
	}
}
