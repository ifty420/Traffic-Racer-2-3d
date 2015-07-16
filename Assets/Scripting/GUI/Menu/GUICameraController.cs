using UnityEngine;
using System.Collections;

public class GUICameraController : MonoBehaviour 
{
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(Input.acceleration.x * 0.15f, -0.5f, 0.5f),0,-10);
    }
}
