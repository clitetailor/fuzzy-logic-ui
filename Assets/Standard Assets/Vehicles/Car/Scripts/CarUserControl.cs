using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        CarController m_Car; // the car controller we want to use

		private void Awake()
		{
			// get the car controllers>();

			m_Car = GetComponent<CarController>();
			Debug.Log("speed: " + m_Car.MaxSpeed);

			GameObject path = transform.Find("Path").gameObject;
		}


        private void FixedUpdate()
        {
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
}
