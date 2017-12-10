using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	private const float mpsToMPH = 2.2369362912f;

	UnityStandardAssets.Vehicles.Car.CarController m_Car;
	Vertices road;
    List<Vector3> trafficLightsFound = new List<Vector3>();
    List<Vector3> obstaclesFound = new List<Vector3>();

	Transform target;
	Vector3 offsetTargetPos;
	new Rigidbody rigidbody;

	void Start()
	{
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.GetComponent<Vertices>();

		transform.position = road.trackedPoints[0];
		Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
		transform.rotation = Quaternion.LookRotation(direction);

		rigidbody = GetComponent<Rigidbody>();
        detectLight();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void getObjectInView()
	{
        obstaclesFound = new List<Vector3>();
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		RaycastHit hit;
		for(var i = 0; i < obstacles.Length; i++)
		{
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(obstacles[i].transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			if(Physics.Raycast(ray, out hit))
			{
				if(isObstacleInRoad(obstacles[i].transform.position)){
                    obstaclesFound.Add(obstacles[i].transform.position);
                }
			}
		}
	}

	bool isObstacleInRoad(Vector3 obstaclePoint) {
        float minDistance = Vector3.Distance(obstaclePoint,road.trackedPoints[0]);
        for(int i = 1; i < road.trackedPoints.Count; ++i) {
            float curr = Vector3.Distance(obstaclePoint,road.trackedPoints[i]);
            if(curr < minDistance) {
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

        foreach (GameObject trafficLight in traficLights) {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(trafficLight.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

			if (Physics.Raycast(ray, out hit))
			{
                if (trafficLight.GetComponent<TrafficLightBehavior>().lightColor != 0){
                    trafficLightsFound.Add(getLightTrack(trafficLight.transform.position, trafficLight.transform.forward));
                }
			}

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		    cube.transform.position = getLightTrack(trafficLight.transform.position, trafficLight.transform.forward);
        }
	}

    Vector3 getLightTrack(Vector3 lightPosition,Vector3 rotation) {
        Vector3 lightPoint = road.trackedPoints[0];
        float minDist = Vector3.Distance(lightPosition,road.trackedPoints[0]);

        for (int i = 1; i < road.trackedPoints.Count; ++i) {
            Vector3 currPoint = road.trackedPoints[i];
            float curr = -rotation.z * (currPoint.x - lightPosition.x) + rotation.x * (currPoint.z - lightPosition.z);

            if (Mathf.Abs(curr) < 0.5) {
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

	/*
	void FixedUpdate()
	{
		Vector3 fwd = transform.forward;
		if (rigidbody.velocity.magnitude > m_Car.MaxSpeed * 0.1f)
		{
			fwd = rigidbody.velocity;
		}

		float desiredSpeed = m_Car.MaxSpeed;

		// now it's time to decide if we should be slowing down...
		// the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

		// check out the angle of our target compared to the current direction of the car
		float approachingCornerAngle = Vector3.Angle(target.forward, fwd);

		// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
		float spinningAngle = rigidbody.angularVelocity.magnitude * cautiousAngularVelocityFactor;

		// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
		float cautiousnessRequired = Mathf.InverseLerp(0, cautiousMaxAngle,
														Mathf.Max(spinningAngle,
																	approachingCornerAngle));

		desiredSpeed = Mathf.Lerp(m_Car.MaxSpeed, m_Car.MaxSpeed * m_CautiousSpeedFactor,
									cautiousnessRequired);

		// use different sensitivity depending on whether accelerating or braking:
		float accelBrakeSensitivity = (desiredSpeed < m_Car.CurrentSpeed)
											? m_BrakeSensitivity
											: m_AccelSensitivity;

		// decide the actual amount of accel/brake input to achieve desired speed.
		float accel = Mathf.Clamp((desiredSpeed - m_Car.CurrentSpeed) * accelBrakeSensitivity, -1, 1);

		// calculate the local-relative position of the target, to steer towards
		Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

		// work out the local angle towards the target
		float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

		// get the amount of steering needed to aim the car towards the target
		float steer = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(m_Car.CurrentSpeed);

		// feed input to the car controller.
		m_Car.Move(steer, accel, accel, 0f);
	}
	*/

	
	private void FixedUpdate()
	{
		// pass the input to the car!
		List<GameObject> trackPoints;
		trackPoints = FrontTrackPoints(10);
		float desiredAngle = RotateAngle(trackPoints);
		//float deltaAngle = desiredAngle - m_Car.CurrentSteerAngle;

		//Debug.Log("Current Angle: " + m_Car.CurrentSteerAngle);
		//Debug.Log("Desired Angle: " + desiredAngle);
		//Debug.Log("Delta Angle:   " + deltaAngle);

		//float h1 = deltaAngle / 90;
		float h = desiredAngle / 30;
		//float h = CrossPlatformInputManager.GetAxis("Horizontal");
		//float h = - desiredAngle / 90;
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		//Debug.Log("h:             " + h);
		//Debug.Log("h1:            " + h1);
		//Debug.Log("h2:            " + h2);

		//Debug.Log(h);

#if !MOBILE_INPUT
		float handbrake = CrossPlatformInputManager.GetAxis("Jump");
		m_Car.Move(h, v, v, handbrake);
#else
        m_Car.Move(h, v, v, 0f);
#endif
	}

	private List<GameObject> FrontTrackPoints(int radius) {
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

	private float RotateAngle(List<GameObject> trackPoints) {
		List<float> amounts = trackPoints
			.Select(point =>
				Vector3.Distance(point.transform.position, transform.position))
			.Select(length => 1f / ( Mathf.Abs(length - 5) + 1 ))
			.ToList();

		float sum = 0;
		for (int i = 0; i < amounts.Count; ++i) {
			sum += amounts[i];
		}

		float accum = 0;
		for (int i = 0; i < trackPoints.Count; ++i) {
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

	private void Modular(ref float angle) {
		while (angle < -180f) {
			angle += 360f;
		}

		while (angle > 180f) {
			angle -= 360f;
		}
	}
}
