using UnityEngine;
using System.Collections;

public class CreateLimiter : MonoBehaviour
{
	GameObject Car;

	void Start ()
	{
		Car = GameObject.FindGameObjectWithTag("PlayerCar");
	}

	void Update ()
	{
		if (Car.transform.position.z > transform.position.z + 250)
			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z+428f);
	}
}