using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using FuzzyLogic;

[RequireComponent(typeof(UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour
{
    private const float mpsToMPH = 2.2369362912f;

    UnityStandardAssets.Vehicles.Car.CarController m_Car;
    Vertices road;
    List<Vector3> trafficLightsFound = new List<Vector3>();
    List<Vector3> obstaclesOnScreen = new List<Vector3>();

    Transform target;
    Vector3 offsetTargetPos;
    new Rigidbody rigidbody;

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
		
		

        rigidbody = GetComponent<Rigidbody>();
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

		float speed = CaculateSpeed();

    #if !MOBILE_INPUT
        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
        m_Car.Move(h, v, v, handbrake);
    #else
            m_Car.Move(h, v, v, 0f);
    #endif
    }

	private float CaculateSpeed() {
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


	private GameObject DetectTrafficLight()
	{
		List<GameObject> traficLights = new List<GameObject>(GameObject.FindGameObjectsWithTag("TrafficLight"));
		RaycastHit hit;

		foreach (GameObject trafficLight in traficLights)
		{
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(trafficLight.transform.position);
			Ray ray = Camera.main.ScreenPointToRay(screenPoint);

			if (Physics.Raycast(ray, out hit))
			{
				if (trafficLight.GetComponent<TrafficLightBehavior>().lightColor != 0)
				{
					trafficLightsFound.Add(TrafficLightTrackPoint(trafficLight.transform.position, trafficLight.transform.forward));
				}
			}

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = TrafficLightTrackPoint(trafficLight.transform.position, trafficLight.transform.forward);
		}

		return null;
	}

	private Vector3 TrafficLightTrackPoint(Vector3 lightPosition, Vector3 rotation)
	{
		Vector3 lightPoint = road.trackedPoints[0];
		float minDist = Vector3.Distance(lightPosition, road.trackedPoints[0]);

		for (int i = 1; i < road.trackedPoints.Count; ++i)
		{
			Vector3 currPoint = road.trackedPoints[i];
			float curr = -rotation.z * (currPoint.x - lightPosition.x) + rotation.x * (currPoint.z - lightPosition.z);

			if (Mathf.Abs(curr) < 0.5)
			{
				float currDist = Vector3.Distance(lightPosition, currPoint);
				if (currDist < minDist)
				{
					lightPoint = currPoint;
					minDist = currDist;
				}
			}
		}

		return lightPoint;
	}

}
