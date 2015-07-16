using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
    public int CarIndex = 0;
    public int ColorIndex = 0;

	public GameObject[] CarPrefabs;
	public GameObject CarBody;

    public GroundController Ground;
    public CameraController Camera;

    void Awake()
    {
        if (PlayerPrefs.HasKey("CarModel"))
            CarIndex = PlayerPrefs.GetInt("CarModel");

        if (PlayerPrefs.HasKey("CarColor"))
            ColorIndex = PlayerPrefs.GetInt("CarColor");

        GameObject car = GameObject.Instantiate(CarPrefabs [CarIndex], new Vector3(2.4f,0,0), Quaternion.Euler(Vector3.zero)) as GameObject;
        Ground.CharCar = car.transform;
        Camera.Target = car.transform;

        car.GetComponent<CarDriver>().IsPlayer = true;
		/*
        ColorPicker col = car.GetComponent<ColorPicker>();
        col.CarColor = (ColorPicker.Colors)ColorIndex;
        col.UpdateColor();
		*/

		// Color Select
		CarBody = GameObject.Find("model");

		if (PlayerPrefs.GetInt("CarColor") == 0)
			CarBody.transform.Find("body").GetComponent<Renderer>().material.color = new Color(1,0,0);
		else if (PlayerPrefs.GetInt("CarColor") == 1)
			CarBody.transform.Find("body").GetComponent<Renderer>().material.color = new Color(1,0.5f,0);
		else if (PlayerPrefs.GetInt("CarColor") == 2)
			CarBody.transform.Find("body").GetComponent<Renderer>().material.color = new Color(0,0,1);
		else if (PlayerPrefs.GetInt("CarColor") == 3)
			CarBody.transform.Find("body").GetComponent<Renderer>().material.color = new Color(0.1f,0.1f,0.1f);
	}
}
