using UnityEngine;
using System.Collections;

public class InGameControls : MonoBehaviour {

	private CarDriver _driver;

	void Start ()
	{
		_driver = GameObject.FindGameObjectWithTag("PlayerCar").GetComponent<CarDriver>();;
	}

	public void AccelerateDown ()
	{
		_driver.CurrentAcceleration = 1;
	}

	public void AccelerateUp ()
	{
		_driver.CurrentAcceleration = 0;
	}

	public void BrakeDown ()
	{
		_driver.CurrentAcceleration = -1;
	}

	public void BrakeUp ()
	{
		_driver.CurrentAcceleration = 0;
	}

	public void NitroDown ()
	{
		if (_driver.NitroPoints > 0)
		_driver.Nitro = true;
	}

	public void NitroUp ()
	{
			_driver.Nitro = false;
	}

}
