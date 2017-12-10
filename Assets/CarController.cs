using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	public float mpsToMPH = 2.2369362912f;

	UnityStandardAssets.Vehicles.Car.CarController m_Car;
	Vertices road;

	private Vector3 lastPos;

	Rigidbody rigidbody;

	void Start()
	{
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.GetComponent<Vertices>();

		transform.position = road.trackedPoints[0];
		Vector3 direction = road.trackedPoints[1] - road.trackedPoints[0];
		transform.rotation = Quaternion.LookRotation(direction);

		rigidbody = GetComponent<Rigidbody>();

		lastPos = transform.position;
	}

	void Awake() {
		m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
	}

	void FixedUpdate() {
		// pass the input to the car!
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		Debug.Log(m_Car.CurrentSpeed);

#if !MOBILE_INPUT
		float handbrake = CrossPlatformInputManager.GetAxis("Jump");
		m_Car.Move(h, v, v, handbrake);
#else
        m_Car.Move(h, v, v, 0f);
#endif
	}
}
