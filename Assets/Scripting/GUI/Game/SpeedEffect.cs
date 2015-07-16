using UnityEngine;
using System.Collections;

public class SpeedEffect : MonoBehaviour {

	GameObject Player;

	void Start ()
	{
		Player = GameObject.FindGameObjectWithTag("PlayerCar");
	}
	
	void Update ()
	{
//		if (Player.rigidbody.velocity.z > 60)
		if (Player.GetComponent<CarDriver>().Nitro)
			GetComponent<Renderer>().enabled = true;
		else
			GetComponent<Renderer>().enabled = false;
	}
}
