using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightBehavior : MonoBehaviour {

	public int redTime = 10;
	public int yellowTime = 5;
	public int greenTime = 10;

	Material redLight;
	Material yellowLight;
	Material greenLight;

	enum LightColor { 
		GREEN_LIGHT,
		YELLOW_LIGHT,
		RED_LIGHT
	}

	LightColor lightColor = LightColor.GREEN_LIGHT;
	int countDown = 10;

	// Use this for initialization
	void Start () {
		redLight = transform.Find("TopLight").GetComponent<Renderer>().material;
		yellowLight = transform.Find("MiddleLight").GetComponent<Renderer>().material;
		greenLight = transform.Find("BottomLight").GetComponent<Renderer>().material;

		if (
			name == "TrafficLight"
			&& redLight != null
			&& yellowLight != null
			&& greenLight != null
		) {
			StartCoroutine(Lighting());
		}
	}

	IEnumerator Lighting() {
		if (countDown > 0) {
			--countDown;
		} else {
			switch (lightColor) {
				case LightColor.GREEN_LIGHT: {
						lightColor = LightColor.YELLOW_LIGHT;
						countDown = yellowTime;
				
						break;
				}

				case LightColor.YELLOW_LIGHT: {
						lightColor = LightColor.RED_LIGHT;
						countDown = redTime;

						break;
				}

				case LightColor.RED_LIGHT: {
						lightColor = LightColor.GREEN_LIGHT;
						countDown = greenTime;
				
						break;
				}

				default: {
						lightColor = LightColor.GREEN_LIGHT;
						countDown = greenTime;

						break;
				}
			}
		}

		yield return new WaitForSeconds(1);
		StartCoroutine(Lighting());
	}

	void TurnOff(ref Material lightMaterial) {
		lightMaterial.DisableKeyword("_EMISSION");
	}

	void TurnOn(ref Material lightMaterial) {
		lightMaterial.EnableKeyword("_EMISSION");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
