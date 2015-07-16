using UnityEngine;
using System.Collections;

public class ConstantsStorage : MonoBehaviour
{
    public static ConstantsStorage I { get; private set; }

    public AnimationCurve AccelerationCurveSlow;
    public AnimationCurve AccelerationCurveMedium;
    public AnimationCurve AccelerationCurveFast;

    public AnimationCurve ControlSensetivity;




	// Use this for initialization
    void Awake()
	{
	    I = this;
        DontDestroyOnLoad(this.gameObject);
	}
}
