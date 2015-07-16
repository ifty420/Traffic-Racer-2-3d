using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour {

	Image myImage;
	Image SpeedMark;
	float playerMaxSpeed;
	GameObject playerCar;

	void Start () 
	{
		myImage = GetComponent<Image>();
		playerMaxSpeed = GameObject.FindWithTag("PlayerCar").GetComponent<CarDriver>().MaxSpeed;
		playerCar = GameObject.FindWithTag("PlayerCar");
		SpeedMark = GameObject.Find("Speed100kmMark").GetComponent<Image>();

		SpeedMark.fillAmount = 75f / playerMaxSpeed;
	}
	
	void Update()
	{
		myImage.fillAmount = playerCar.GetComponent<Rigidbody>().velocity.z / playerMaxSpeed;
	}
}
