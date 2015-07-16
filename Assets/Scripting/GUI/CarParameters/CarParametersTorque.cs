using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarParametersTorque : MonoBehaviour
{
	void Update ()
	{
		if (PlayerPrefs.GetInt("CarModel") == 0)
			GetComponent<Slider>().value = 0.14f;
		else if (PlayerPrefs.GetInt("CarModel") == 1)
			GetComponent<Slider>().value = 0.35f;
		else if (PlayerPrefs.GetInt("CarModel") == 2)
			GetComponent<Slider>().value = 0.35f;
		else if (PlayerPrefs.GetInt("CarModel") == 3)
			GetComponent<Slider>().value = 0.56f;
		else if (PlayerPrefs.GetInt("CarModel") == 4)
			GetComponent<Slider>().value = 0.56f;
		else if (PlayerPrefs.GetInt("CarModel") == 5)
			GetComponent<Slider>().value = 0.84f;
		else if (PlayerPrefs.GetInt("CarModel") == 6)
			GetComponent<Slider>().value = 0.84f;
		else if (PlayerPrefs.GetInt("CarModel") == 7)
			GetComponent<Slider>().value = 0.98f;
	}
}
