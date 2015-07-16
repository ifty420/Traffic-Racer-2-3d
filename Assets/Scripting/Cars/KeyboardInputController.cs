using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class KeyboardInputController : MonoBehaviour
{
    private CarDriver _driver;

    void Awake()
    {
        _driver = GetComponent<CarDriver>();

#if !UNITY_EDITOR
        GameObject.Destroy(this);
#endif
    }

    void Update()
    {
        _driver.CurrentAcceleration = Input.GetAxis("Vertical");  //4
        _driver.CurrentWheelsSteer = Input.GetAxis("Horizontal");     //4    
        _driver.Nitro = Input.GetKey(KeyCode.E);
    }
}
