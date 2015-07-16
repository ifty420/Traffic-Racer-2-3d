using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Timers;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CarDriver))]
public class AIInputController : MonoBehaviour 
{
    public float TargetX = 0;
    public float TargetZ = 1;

    public float TargetAcceleration = 1;

    public bool Forward;

    public Renderer[] RightSignals;
    public Renderer[] LeftSignals;

    private CarDriver _driver;
    private bool _canChangeDir;
    private bool _canMove;


    private float _steer;



    private bool BlinkingLeft;
    private bool BlinkOn;

    private bool _setVelocity;

    private bool _overtook;

    private const float rotAmout = 5.5f;
    void Awake()
    {
        _driver = GetComponent<CarDriver>();

        _driver.CurrentAcceleration = TargetAcceleration;

        StartCoroutine(TestTimer());

        ActivateSignals(LeftSignals, false);
        ActivateSignals(RightSignals, false);

        _driver.RotationSpeed = 0.35f;
        _driver.RotationAmount = rotAmout;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateLineChanging();

        UpdateScore();
    }



    private void UpdateMovement()
    {
        _driver.RotationAmount = Forward ? -rotAmout : rotAmout;  

        if (!_setVelocity)
        {
            GetComponent<Rigidbody>().velocity += new Vector3(0,0, Forward ? -45f : 45f);
            _setVelocity = true;
        }

        RaycastHit hit;
        if (GetComponent<Rigidbody>().SweepTest(transform.forward, out hit, 25))
        {
            _driver.CurrentAcceleration = -(2 - Mathf.Clamp(hit.distance/5, 0, 2));
        }
        else
        {
            _driver.CurrentAcceleration = TargetAcceleration;
        }


        if (Mathf.Abs(transform.position.x - TargetX) < 0.25f)
        {
            _canMove = false;
        }
        if (_canMove)
        {
            if (TargetX < transform.position.x)
            {
				_steer = Mathf.Lerp(_steer, -1, Time.deltaTime*0.25f);
            }
            else
            {
				_steer = Mathf.Lerp(_steer, 1, Time.deltaTime*0.25f);
            }
        }
        else
        {
            _steer = Mathf.Lerp(_steer, 0, Time.deltaTime*10f);
        }


        _driver.CurrentWheelsSteer = _steer;
    }

    private void UpdateLineChanging()
    {
        if (_canChangeDir)
        {
            var result = Random.Range(0f, 1f);
            if (result > 0.12f)
            {
                if (CheckSides())
                {
                    StartCoroutine(StartChangeLane());
                }
                else
                {
                    StartCoroutine(TestTimer());
                }

            }
            else
            {
                StartCoroutine(TestTimer());
            }
        }
    }



    private IEnumerator StartChangeLane()
    {
        _canChangeDir = false;

		// Lane change delay
		yield return new WaitForSeconds(2);

        BlinkingLeft = Random.value > 0.5f;
        float x = BlinkingLeft ? 3 : 7;
        if (Forward) x *= -1;

        if (Math.Abs(x - TargetX) < 0.1f)
        {
            StartCoroutine(TestTimer());
            yield return null;
        }
        else
        {
            int blinksCount = 2 + Random.Range(0, 1);

            ActivateSignals(LeftSignals, false);
            ActivateSignals(RightSignals, false);

            for (int i = 0; i < blinksCount; i++)
            {
                ActivateSignals(GetSignals(), true);
                yield return new WaitForSeconds(0.2f);
                ActivateSignals(GetSignals(), false);
                yield return new WaitForSeconds(0.2f);
            }
            if (CheckSides())
            {
                TargetX = x;
                _canMove = true;
            }

            StartCoroutine(TestTimer());

            for (int i = 0; i < blinksCount*2; i++)
            {
                ActivateSignals(GetSignals(), true);
                yield return new WaitForSeconds(0.2f);
                ActivateSignals(GetSignals(), false);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void UpdateScore()
    {
        if (!_overtook && !Forward)
        {
            if (transform.position.z < PlayerCarBehaviour.Instance.transform.position.z - 2)
            {
                _overtook = true;
                ScoreSystem.Instance.CarOvertook();
            }
        }
    }

    private bool CheckSides()
    {
        //return !_driver.OtherCarIsClose(10.5f);
		return !_driver.OtherCarIsClose(transform.Find("CarBody").GetComponent<BoxCollider>().size.y * 5f);
    }

    private Renderer[] GetSignals()
    {
        return BlinkingLeft ? LeftSignals : RightSignals;
    }

    private void ActivateSignals(Renderer[] signals, bool activate)
    {
        foreach (var signal in signals)
        {
            if (signal == null)
            {
                Debug.Log(gameObject.name);
            }
            signal.enabled = activate;
        }
    }

    void OnTriggerEnter()
    {
        EventController.PostEvent("car.ai.needdestroy", gameObject);
    }

    private IEnumerator TestTimer()
    {
        _canChangeDir = false;
        yield return new WaitForSeconds(0.5f + Random.Range(0f,2f));
        _canChangeDir = true;
    }
}
