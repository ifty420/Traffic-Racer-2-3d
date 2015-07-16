using UnityEngine;
using System.Collections;

public class LightScale : MonoBehaviour
{
	float Scale;
	SettingsControl Panel;

	void Start ()
	{
		Scale = 0;
		GetComponent<Renderer>().material.SetColor ("_TintColor", new Vector4(1,1,1,Scale));

		Panel = GameObject.Find("SlideIn").GetComponent<SettingsControl>();
	}
	
	void Update ()
	{
		if (Scale < 1 && transform.parent.parent.localScale.x > 8 && !Panel.Slided)
		{
			Scale += 0.02f;
			GetComponent<Renderer>().material.SetColor ("_TintColor", new Vector4(1,1,1,Scale));
		}
		else if (transform.parent.parent.localScale.x < 8)
			Scale = 0;
		else if (Scale > 0 && Panel.Slided)
		{
			Scale -= 0.06f;
			GetComponent<Renderer>().material.SetColor ("_TintColor", new Vector4(1,1,1,Scale));
		}
	}
}
