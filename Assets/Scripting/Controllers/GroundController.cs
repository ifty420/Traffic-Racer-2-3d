using UnityEngine;
using System.Collections;

public class GroundController : MonoBehaviour 
{
    public Transform CharCar = null;

    private AsphaltTextureController[] _ATCControllers = null;

    void Start()
    {
        _ATCControllers = GameObject.FindObjectsOfType<AsphaltTextureController>() as AsphaltTextureController[];
    }

    void Update()
    {
        if (CharCar != null)
        {
            Vector3 tpos = CharCar.position;
            tpos.x=0;
            tpos.y=0;
            transform.position = tpos;

            foreach (var atc in _ATCControllers)
                atc.UpdateTex();
        }
    }
}
