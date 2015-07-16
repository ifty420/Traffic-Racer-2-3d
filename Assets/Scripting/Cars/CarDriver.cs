using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;

[RequireComponent(typeof(Rigidbody))]
public class CarDriver : MonoBehaviour 
{
    public Transform FirstPersonCameraPoint;

    public bool IsPlayer;

    public Renderer[] BreakingHeadLights;

    public Transform Body;

    public Transform VisualLFWheel;
    public Transform VisualLBWheel;
    public Transform VisualRFWheel;
    public Transform VisualRBWheel;

    public Transform WheelsRoot;

    public WheelCollider PhysicLFWheel;
    public WheelCollider PhysicLBWheel;
    public WheelCollider PhysicRFWheel;
    public WheelCollider PhysicRBWheel;

    public float MaxMotorTorque = 50;
    public float MaxBrakeTorque = 50;
    public float MaxWheelsSteer = 15;
    public float MaxSpeed = 150;

    public float Friction = 5f;

	public Rigidbody _rigidbody;

    public Vector3 COM = Vector3.zero;

    [Range(-1,1)]
    public float CurrentAcceleration = 0;
    [Range(-1,1)]
    public float CurrentWheelsSteer = 0;

	public bool Nitro;
	public float NitroPoints;

    public float throttle;

    public bool Dead;

    public bool CreateSkidmarks = false;

    public float Speed { get { return PhysicRBWheel.rpm * 0.10472f * PhysicRBWheel.radius; } }

    public AnimationCurve AccelarationCurve;

    private const float _maxUpDownBodyMovement = 7.5f;
    private const float _UpDownBodySensetivity = 5.0f;

    private float _currentUpDown;

    public float RotationSpeed = 1f;
    public float RotationAmount = 1f;
    private int lastSkidIndex = -1;
    private int lastSkidIndex2 = -1;

	private float SideSpeedMult = 2.5f;

	private float _currentMaxSpeed = 0;
	
    public static List<Transform> Drivers = new List<Transform>();

    public bool CheckGrounded()
    {
        return PhysicRFWheel.isGrounded && 
            PhysicLFWheel.isGrounded &&
            PhysicLFWheel.isGrounded &&
            PhysicLBWheel.isGrounded;
    }

    void Start()
    {
		gameObject.GetComponent<Rigidbody>().centerOfMass = COM;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        AccelarationCurve = ConstantsStorage.I.AccelerationCurveSlow;
        _rigidbody.freezeRotation = true;

        _currentMaxSpeed = MaxSpeed;

        Drivers.Add(transform);
    }

    private void OnDisable()
    {
        Drivers.Remove(transform);
    }

    private void OnDestroy()
    {
        Drivers.Remove(transform);
    }

    void Update()
    {
        if (Dead)
        {
            CurrentAcceleration = 0;
            CurrentWheelsSteer = 0;
        }

        UpdateVisuals();


        if (Dead)
        {
            return;
        }

        if (BreakingHeadLights.Length > 0)
        {
            if (CurrentAcceleration >= 0 && BreakingHeadLights[0].enabled)
                foreach (Renderer r in BreakingHeadLights)
                    r.enabled = false;

            if (CurrentAcceleration < 0 && !BreakingHeadLights[0].enabled)
                foreach (Renderer r in BreakingHeadLights)
                    r.enabled = true;
        }
    }

    void FixedUpdate()
    {
        UpdatePhysics();

        UpdateSkidmarks();

		LineMagnet();
    }


    private float _lastAcceleration;

    void UpdatePhysics()
    {

        if (Dead)
        {
            CurrentAcceleration = 0;
            CurrentWheelsSteer = 0;
        }

        // some magic...
		float steer = CurrentWheelsSteer * MaxWheelsSteer * SideSpeedMult;//*2.5f;

        steer = Mathf.Abs(_rigidbody.velocity.z) < 10 
            ? steer * _rigidbody.velocity.z / 10f
            : steer;

        float acceleration = CurrentAcceleration > 0
            ? CurrentAcceleration*MaxMotorTorque
            : CurrentAcceleration*MaxBrakeTorque;

        acceleration *= 0.5f;

		if (Nitro && CurrentAcceleration > 0 && NitroPoints > 0)
        {
            acceleration *= 2.5f;
			NitroPoints -= 3;
		}

        if (CurrentAcceleration > 0)
        {
            if (!Nitro)
            {
                acceleration *= AccelarationCurve.Evaluate(_rigidbody.velocity.z);
			}
        }

        float angle = transform.localEulerAngles.y;

        angle = Mathf.Clamp(angle, 0, 360);



        bool forward;
        if ((angle > 269 && angle < 360) || (angle >= 0 && angle < 90))
        {
            forward = true;
        }
        else
        {
            forward = false;
            acceleration = -acceleration;
        }

        var velocity = _rigidbody.velocity;
        float oldVelocity = velocity.z;

        if (Mathf.Approximately(acceleration, 0f))
        {
            velocity.z = Mathf.MoveTowards(velocity.z, 0, Time.fixedDeltaTime*Friction*(Dead ? 4f : 1f));
        }
        else
        {
            velocity += new Vector3(0, 0, acceleration * Time.fixedDeltaTime);
        }

        float maxSpeedMod = Nitro ? 1.5f : 1f;

        _currentMaxSpeed = Mathf.Lerp(_currentMaxSpeed, MaxSpeed*maxSpeedMod, Time.fixedDeltaTime*2.5f);


        if (forward)
        {
            velocity.z = Mathf.Clamp(velocity.z, 0, _currentMaxSpeed);
        }
        else
        {
            velocity.z = Mathf.Clamp(velocity.z, -_currentMaxSpeed, 0);

        }

        velocity.x = Mathf.Lerp(velocity.x, 0, Time.fixedDeltaTime);

        _rigidbody.velocity = velocity;

        if (Dead)
        {
            CurrentAcceleration = 0;
            CurrentWheelsSteer = 0;
            return;
        }

        _lastAcceleration = (oldVelocity - velocity.z) * _UpDownBodySensetivity;
        _lastAcceleration = Mathf.Clamp(_lastAcceleration, -_maxUpDownBodyMovement*0.5f, _maxUpDownBodyMovement*1.5f);
        _lastAcceleration = Mathf.Lerp(_lastAcceleration, 0, Time.fixedDeltaTime);


        _currentUpDown += _lastAcceleration;
        _currentUpDown = Mathf.Clamp(_currentUpDown, -_maxUpDownBodyMovement, _maxUpDownBodyMovement);

        _currentUpDown = Mathf.Lerp(_currentUpDown, 0, Time.fixedDeltaTime);

        _rigidbody.MovePosition(transform.position + new Vector3(steer*Time.fixedDeltaTime, 0, 0));

    }
    
    private void UpdateSkidmarks()
    {
        if (CreateSkidmarks)
        {
            if (_lastAcceleration > 0.4f && _rigidbody.velocity.z > 50 && CurrentAcceleration < 0)
            {
                CreateSkidmark();
            }
            else
            {
                lastSkidIndex = -1;
                lastSkidIndex2 = -1;
            }
        }
    }

    private void CreateSkidmark()
    {
		lastSkidIndex = Skidmark.Instance.AddSkidMark(transform.position + new Vector3(VisualLBWheel.localPosition.x, 0, 2), Vector3.up, 0.1f, lastSkidIndex);
		lastSkidIndex2 = Skidmark.Instance.AddSkidMark(transform.position + new Vector3(VisualRBWheel.localPosition.x, 0, 2), Vector3.up, 0.1f, lastSkidIndex2);
	}

    void UpdateVisuals()
    {
        UpdateWheelsVisual();

        UpdateBodyVisual();

	//	LineMagnet();
    }

    private void UpdateWheelsVisual()
    {
        if (VisualLFWheel == null)
            return;

        float rot = _rigidbody.velocity.z*Time.deltaTime*30;
		float RotAngle = CurrentWheelsSteer;

		if (RotAngle > 0.5f) RotAngle = 0.5f;
		else if (RotAngle < -0.5f) RotAngle = -0.5f;

		VisualLBWheel.Rotate(new Vector3(1, 0, 0), rot);
		VisualRBWheel.Rotate(new Vector3(1, 0, 0), rot);

		VisualLFWheel.GetChild(0).Rotate(new Vector3(1, 0, 0), rot);
		VisualRFWheel.GetChild(0).Rotate(new Vector3(1, 0, 0), rot);

		if (_rigidbody.velocity.z > 30)
		{
		VisualLFWheel.rotation = Quaternion.Euler(new Vector3(0, 180+RotAngle*60, 0));
		VisualRFWheel.rotation = Quaternion.Euler(new Vector3(0, 180+RotAngle*60, 0));
		}
	}

    private void UpdateBodyVisual()
    {
        if (Body == null)
            return;

        var angle = Body.localEulerAngles;


        var steer = Mathf.Abs(_rigidbody.velocity.z) < 10
            ? CurrentWheelsSteer*_rigidbody.velocity.z/10f
            : CurrentWheelsSteer;

        var targetAngle = new Vector3(_lastAcceleration * (IsPlayer ? 1 : 0), 15*steer*RotationAmount, steer*15f);

	// Car rotation
	if (Mathf.Abs(_rigidbody.transform.position.x) < 7.55f)
		{
        angle.y = Mathf.LerpAngle(angle.y, targetAngle.y * RotationSpeed, Time.deltaTime * 11.5f);
        angle.x = Mathf.LerpAngle(angle.x, targetAngle.x, Time.deltaTime*2.5f);
        angle.z = Mathf.LerpAngle(angle.z, targetAngle.z, Time.deltaTime*2.5f);
        Body.localEulerAngles = angle;
		}
		else
			Body.localEulerAngles = new Vector3(0,0,0);
	
    }

    public bool OtherCarIsClose(float distance)
    {
        foreach (var driver in Drivers)
        {
            bool result = driver.gameObject != transform.gameObject;
            if (result)
            {
                float distanceToDriver = Vector3.Distance(driver.position, transform.position);

                if (distanceToDriver < distance)
                {
                    return true;
                }
				else if (driver.gameObject.tag == "PlayerCar" && distanceToDriver < distance*2)
				{
					return true;
				}
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Road"))
        {
        }
        else
        {
        //    Dead = true;
        }
    }

	private void LineMagnet()
	{
		if (tag == "PlayerCar")
		{

            if (_rigidbody.velocity.z > 75 && NitroPoints < 500 && !Nitro)
            {
				NitroPoints++;
			}

			if (Mathf.Abs(_rigidbody.transform.position.x) > 0f && Mathf.Abs(_rigidbody.transform.position.x) < 4.8f)
				SideSpeedMult = Mathf.Abs(_rigidbody.transform.position.x -2.4f) * 1f + 7f;
			else if (Mathf.Abs(_rigidbody.transform.position.x) > 4.8f && Mathf.Abs(_rigidbody.transform.position.x) < 7.2f)
				SideSpeedMult = Mathf.Abs(_rigidbody.transform.position.x -7.2f) * 1f + 7f;
			else
				SideSpeedMult = Mathf.Abs(_rigidbody.transform.position.x -7.2f) * 1f + 7f;

			SideSpeedMult *= (300 - _rigidbody.velocity.z)/300;

		}
	}
}
