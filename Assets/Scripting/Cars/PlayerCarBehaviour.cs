using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class PlayerCarBehaviour : MonoBehaviour
{
    public static PlayerCarBehaviour Instance;

    private CarDriver _driver;
    private float _maxSpeed = 0;

    private Vector3 _oldPos;
    public float Distance = 0;

    WheelCollider[] _cols;

    public int Lifes = 0;
    private float _lastDTime = 0;

    public bool Grounded { get { return _driver.CheckGrounded(); } }

    void Awake()
    {
        _driver = GetComponent<CarDriver>();
        _maxSpeed = _driver.MaxSpeed;
        _driver.CreateSkidmarks = true;
        Instance = this;
    }

    void Start()
    {
        _oldPos = transform.position;

        _cols = new WheelCollider[4];
        _cols [0] = _driver.PhysicLBWheel;
        _cols [1] = _driver.PhysicLFWheel;
        _cols [2] = _driver.PhysicRBWheel;
        _cols [3] = _driver.PhysicRFWheel;
    }

    void Update()
    {
        if (Lifes < 0)
        {
            return;
        }

        float dist = Vector3.Distance(_oldPos, transform.position);
        if (dist> 1) 
        {
            Distance+=dist;
			EventController.PostEvent("update.gui.distance",gameObject);
			EventController.PostEvent("update.gui.speed",gameObject);
			_oldPos = transform.position;
        }

//        bool g = _driver.CheckGrounded();
//        if (!_upside && !g)
//        {
//            _upside = true;
//            StartCoroutine("UpsideCar");
//        } else
//            if (_upside && g)
//        {
//            _upside = false;
//            StopCoroutine("UpsideCar");
//        }
    }

    private bool _upside = false;

    IEnumerator UpsideCar()
    {
        yield return new WaitForSeconds(3);
        EventController.PostEvent("car.player.death", gameObject);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _driver.MaxSpeed = _maxSpeed * 0.2f;
        } else
        {
            if (!GetComponent<CarDriver>().Dead)
            {
                if (!col.gameObject.CompareTag("Road"))
                    if (Time.time - _lastDTime > 5)
                    {
                        Lifes--;
                        EventController.PostEvent("update.car.health", gameObject);
                        if (Lifes >= 0)
                            StartCoroutine("Immortal");
                        else
                        {
                            EventController.PostEvent("car.player.death", gameObject);
                            GetComponent<CarDriver>().Dead = true;
                        }
                        _lastDTime = Time.time;
                    }

                if (col.gameObject.CompareTag("OutWorld"))
                {
                    if (col.contacts.Length > 0)
                        GetComponent<Rigidbody>().AddForce(col.contacts[0].normal*0.05f, ForceMode.Impulse);
                }

                this._driver.WheelsRoot.gameObject.SetActive(true);

                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().freezeRotation = false;
            }
        }
    }

    IEnumerator Immortal()
    {
        EventController.PostEvent("car.player.immortal.start", null);
        yield return new WaitForSeconds(5);
        EventController.PostEvent("car.player.immortal.end", null);
    }

//    void OnCollisionStay(Collision col)
//    {
//        if (col.gameObject.CompareTag("OutWorld"))
//        {
//            if (col.contacts.Length>0)
//                rigidbody.AddForce(col.contacts[0].normal*0.02f,ForceMode.Impulse);
//        }
//
//        if (!col.gameObject.CompareTag("Road") && !col.gameObject.CompareTag("Ground") && col.relativeVelocity.magnitude>15f)
//            if (Time.time - _lastDTime > 5)
//        {
//            Lifes--;
//            EventController.PostEvent("update.car.health",gameObject);
//            if (Lifes>=0)
//                StartCoroutine("Immortal");
//            else
//                EventController.PostEvent("car.player.death",gameObject);
//            _lastDTime = Time.time;
//        }
//    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _driver.MaxSpeed = _maxSpeed;

            WheelFrictionCurve curve = new WheelFrictionCurve();
            curve.extremumSlip = 1;
            curve.extremumValue = 20000;
            curve.asymptoteSlip = 2;
            curve.asymptoteValue = 10000;
            curve.stiffness = 1;
            foreach(WheelCollider wcol in _cols)
                wcol.sidewaysFriction = curve;
        }
    }
}
