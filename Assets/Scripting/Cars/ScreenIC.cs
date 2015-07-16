using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CarDriver))]
public class ScreenIC : MonoBehaviour, IEventSubscriber 
{
	private CarDriver _driver;
	private bool IsSteerLeft = false;
	private bool IsSteerRight = false;
	public bool IsAutoAccel = false;
	public bool IsAccelControl = true;
	private float _steer;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
        _driver = GetComponent<CarDriver>();

		if (PlayerPrefs.GetInt("TiltButtons") == 0)
		{
			IsAutoAccel = true;
			IsAccelControl = false;
		}
    }

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "input.screen.acceleration.down":
                _driver.CurrentAcceleration = 1;
                break;

            case "input.screen.acceleration.up":
                _driver.CurrentAcceleration = 0;
                break;

            case "input.screen.breaking.down":
                _driver.CurrentAcceleration = -1;
				if (PlayerPrefs.GetInt("TiltButtons") == 0) IsAutoAccel = false;
			break;
                
            case "input.screen.breaking.up":
                _driver.CurrentAcceleration = 0;
				if (PlayerPrefs.GetInt("TiltButtons") == 0) IsAutoAccel = true;
                break;

            case "input.screen.time.down":
  //              Time.timeScale = 0.5f;
	//			Time.fixedDeltaTime = 0.02f * Time.timeScale;
                break;

            case "input.screen.time.up":
//                Time.timeScale = 1.0f;
                break;
			
		case "input.button.left.pressed":
			IsSteerLeft = true;
			break;
			
		case "input.button.left.released":
			IsSteerLeft = false;
			break;
			
		case "input.button.right.pressed":
			IsSteerRight = true;
			break;
			
		case "input.button.right.released":
			IsSteerRight = false;
			break;

		case "input.button.nitro.pressed":
			if (_driver.NitroPoints > 0)
			{
				_driver.CurrentAcceleration = 1;
				_driver.Nitro = true;
			}
			else
				_driver.Nitro = false;
			break;

		case "input.button.nitro.released":
			_driver.CurrentAcceleration = 0;
			_driver.Nitro = false;
			break;

        case "gui.screen.game.show":
			if (PlayerPrefs.GetInt("TiltButtons") == 0)
			{
				IsAutoAccel = true;
				IsAccelControl = false;
			}
				else
			{
				IsAutoAccel = false;
				IsAccelControl = true;
			}
			break;
		}
    }

    #endregion

    void Update()
    {

        float steer = ConstantsStorage.I.ControlSensetivity.Evaluate(Input.acceleration.x);

		if (IsAccelControl)
		{
			_driver.CurrentWheelsSteer = Mathf.Clamp(Input.acceleration.x * steer, -1, 1);

			if (_driver.transform.position.x > 7.6f && Input.acceleration.x > 0)
				_driver.CurrentWheelsSteer = 0;
			else if (_driver.transform.position.x < -7.0f && Input.acceleration.x < 0)
				_driver.CurrentWheelsSteer = 0;

		}
		else
		{

		if (_driver._rigidbody.transform.position.x < -7.0f)
				IsSteerLeft = false;
		else if (_driver._rigidbody.transform.position.x > 7.6f)
				IsSteerRight = false;

		if (IsSteerLeft && !IsSteerRight)
		{
			if (_driver.CurrentWheelsSteer > -3)
			_driver.CurrentWheelsSteer -= 0.06f;
		}
		else if (!IsSteerLeft && IsSteerRight)
		{
			if (_driver.CurrentWheelsSteer < 3)
				_driver.CurrentWheelsSteer += 0.06f;
		}
		else
		{
			
			_driver.CurrentWheelsSteer = 0;
		}

		}

		if (IsAutoAccel &&_driver.GetComponent<Rigidbody>().velocity.z > 0)
		{
			_driver.CurrentAcceleration = 1;
		}
	}

}
