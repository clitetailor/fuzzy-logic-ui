using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	private const float mpsToMPH = 2.2369362912f;

	UnityStandardAssets.Vehicles.Car.CarController m_Car;
	Vertices road;

	Transform target;
	Vector3 offsetTargetPos;
	new Rigidbody rigidbody;
	
	private float cautiousMaxAngle = 50f;
	private float cautiousAngularVelocityFactor = Mathf.Rad2Deg * 2 / 3;

	private float m_AccelSensitivity = 0.0f;
	private float m_BrakeSensitivity = 1f;
	private float m_SteerSensitivity = 0.05f;

	private float m_CautiousSpeedFactor = 0.05f;

	void Start()
	{
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.GetComponent<Vertices>();

		transform.position = road.trackedPoints[0];
		Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
		transform.rotation = Quaternion.LookRotation(direction);

		rigidbody = GetComponent<Rigidbody>();
	}

	void Awake() {
		m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
	}

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
}
