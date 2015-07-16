using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NitroIndicator : MonoBehaviour {
	
	Image myImage;
	CarDriver playerCar;
	
	void Start () 
	{
		myImage = GetComponent<Image>();
		playerCar = GameObject.FindWithTag("PlayerCar").GetComponent<CarDriver>();
	}
	
	void Update()
	{
		myImage.fillAmount = playerCar.NitroPoints / 500;
	}
}
