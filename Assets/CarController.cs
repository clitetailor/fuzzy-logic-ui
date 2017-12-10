using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (UnityStandardAssets.Vehicles.Car.CarController))]
public class CarController : MonoBehaviour {

	UnityStandardAssets.Vehicles.Car.CarController m_Car; // the car controller we want to use
	Vertices road;

	void Start() {
		GameObject path = GameObject.Find("Path").gameObject;
		road = path.transform.Find("Plane.001").GetComponent<Vertices>();
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
