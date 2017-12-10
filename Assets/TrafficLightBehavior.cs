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

	TextMesh countDownText;

	public enum LightColor { 
		GREEN_LIGHT,
		YELLOW_LIGHT,
		RED_LIGHT
	}
    [HideInInspector]
	public LightColor lightColor = LightColor.GREEN_LIGHT;
	int countDown = 0;

	// Use this for initialization
	void Start () {
		redLight = transform.Find("TopLight").GetComponent<Renderer>().material;
		yellowLight = transform.Find("MiddleLight").GetComponent<Renderer>().material;
		greenLight = transform.Find("BottomLight").GetComponent<Renderer>().material;

		countDownText = transform.Find("CountDown").GetComponent<TextMesh>();

		TurnOn(greenLight);
		
		StartCoroutine(Lighting());
	}

	IEnumerator Lighting() {
		--countDown;
		
		if (countDown < 0) {
			switch (lightColor) {
				case LightColor.GREEN_LIGHT: {
						lightColor = LightColor.YELLOW_LIGHT;
						
						TurnOn(yellowLight);
						TurnOff(greenLight);

						countDown = yellowTime;
				
						break;
				}

				case LightColor.YELLOW_LIGHT: {
						lightColor = LightColor.RED_LIGHT;

						TurnOn(redLight);
						TurnOff(yellowLight);

						countDown = redTime;

						break;
				}

				case LightColor.RED_LIGHT: {
						lightColor = LightColor.GREEN_LIGHT;

						TurnOn(greenLight);
						TurnOff(redLight);

						countDown = greenTime;
				
						break;
				}

				default: {
						lightColor = LightColor.GREEN_LIGHT;

						TurnOff(redLight);
						TurnOff(yellowLight);
						TurnOn(greenLight);

						countDown = greenTime;

						break;
				}
			}
		}

		if (countDownText != null)
		{
			countDownText.text = countDown.ToString();
		}

		yield return new WaitForSeconds(1);
		StartCoroutine(Lighting());
	}

	void TurnOff(Material lightMaterial) {
		if (lightMaterial != null) {
			lightMaterial.DisableKeyword("_EMISSION");
		}
	}

	void TurnOn(Material lightMaterial) {
		if (lightMaterial != null) {
			lightMaterial.EnableKeyword("_EMISSION");
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
