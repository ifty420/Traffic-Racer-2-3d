using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour {

	public bool Slided = true;
	private bool Sliding = true;
	private bool Slided2 = false;
	private bool Sliding2 = true;
	private float ColorPanelScale;

	GameObject tiltImage;
	GameObject tiltImage2;

	public void Start ()
	{
		GameObject.Find("PercentMusic").GetComponent<Text>().text = PlayerPrefs.GetInt("MusicVolume").ToString() + "%";
		GameObject.Find("PercentSFX").GetComponent<Text>().text = PlayerPrefs.GetInt("SFXVolume").ToString() + "%";
		GameObject.Find("Slider_Music").GetComponent<Slider>().value = (float)(PlayerPrefs.GetInt("MusicVolume"))/100;
		GameObject.Find("Slider_SFX").GetComponent<Slider>().value = (float)(PlayerPrefs.GetInt("SFXVolume"))/100;

		// Tilt/Buttons Activate
		tiltImage = GameObject.Find("TiltImage");
		tiltImage2 = GameObject.Find("TiltButtonsSwitch");

		if (PlayerPrefs.GetInt("TiltButtons") == 0)
			{
			tiltImage.GetComponent<Image>().color = new Color(1,1,1,1);
			tiltImage2.GetComponent<Image>().color = new Color(1,1,1,0);
		}
		else
			{
			tiltImage.GetComponent<Image>().color = new Color(1,1,1,0);
			tiltImage2.GetComponent<Image>().color = new Color(1,1,1,1);
		}
}
	
	public void ControlsSwitch ()
	{
		if (PlayerPrefs.GetInt("TiltButtons") == 0)
		{
			transform.Find("TiltImage").GetComponent<Image>().color = new Color(1,1,1,0);
			GetComponent<Image>().color = new Color(1,1,1,1);
			PlayerPrefs.SetInt("TiltButtons",1);
		}
		else
		{
			transform.Find("TiltImage").GetComponent<Image>().color = new Color(1,1,1,1);
			GetComponent<Image>().color = new Color(1,1,1,0);
			PlayerPrefs.SetInt("TiltButtons",0);
		}
	}

	public void MapSwitch ()
	{
		if (PlayerPrefs.GetInt("Map") == 1)
		{
			transform.Find("Map").localPosition = new Vector3(-65f,-3.6f,0);
			PlayerPrefs.SetInt("Map",0);
		}
		else
		{
			transform.Find("Map").localPosition = new Vector3(65f,-3.6f,0);
			PlayerPrefs.SetInt("Map",1);
		}
	}

	public void SlideSettings ()
	{
		if (!Sliding)
		{
			Sliding = true;
		}
	}
	
	public void SlideColor ()
	{
		if (!Sliding2)
			Sliding2 = true;
	}

	public void PercentMusic ()
	{
		PlayerPrefs.SetInt("MusicVolume",(int)(GetComponent<Slider>().value *100));
		transform.Find("PercentMusic").GetComponent<Text>().text = PlayerPrefs.GetInt("MusicVolume").ToString() + "%";
	}

	public void PercentSFX ()
	{
		PlayerPrefs.SetInt("SFXVolume",(int)(GetComponent<Slider>().value *100));
		transform.Find("PercentSFX").GetComponent<Text>().text = PlayerPrefs.GetInt("SFXVolume").ToString() + "%";
	}
	
	public void SelectColor0 ()
	{
		PlayerPrefs.SetInt("CarColor",0);
	}
	
	public void SelectColor1 ()
	{
		PlayerPrefs.SetInt("CarColor",1);
	}
	
	public void SelectColor2 ()
	{
		PlayerPrefs.SetInt("CarColor",2);
	}
	
	public void SelectColor3 ()
	{
		PlayerPrefs.SetInt("CarColor",3);
	}

	void Update ()
	{
		if (Application.loadedLevel == 1)
		{
		// Slide Settings
		if (!Slided && Sliding)
			if (GameObject.Find("Panel").transform.localPosition.x < 400)
				GameObject.Find("Panel").transform.localPosition += new Vector3(50,0,0);
			else
		{
			Slided = true;
			Sliding = false;
		}

		if (Slided && Sliding)
			if (GameObject.Find("Panel").transform.localPosition.x > -550)
				GameObject.Find("Panel").transform.localPosition += new Vector3(-50,0,0);
		else
		{
			Slided = false;
			Sliding = false;
		}

		// Slide Color Panel
		if (!Slided2 && Sliding2)
			if (ColorPanelScale > 0)
				{
				GameObject.Find("PanelColor").transform.localScale = new Vector3(ColorPanelScale,ColorPanelScale,1);
				ColorPanelScale -= 0.1f;
				}
		else
		{
			Slided2 = true;
			Sliding2 = false;
		}
		
		if (Slided2 && Sliding2)
			if (ColorPanelScale < 1)
		{
			GameObject.Find("PanelColor").transform.localScale = new Vector3(ColorPanelScale,ColorPanelScale,1);
			ColorPanelScale += 0.1f;
		}
		else
		{
			Slided2 = false;
			Sliding2 = false;
		}

		}
	}
}
