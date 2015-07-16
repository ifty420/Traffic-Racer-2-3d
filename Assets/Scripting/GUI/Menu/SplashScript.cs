using UnityEngine;
using System.Collections;

public class SplashScript : MonoBehaviour {
	
	void Update ()
	{
		if (Application.GetStreamProgressForLevel("Level0") == 1 && PlayerPrefs.GetInt("level.loading") == 0)
		{
			PlayerPrefs.SetInt("level.loading",0);
			Application.LoadLevel("StartScreen");
		}
		else if (Application.GetStreamProgressForLevel("Level1") == 1 && PlayerPrefs.GetInt("level.loading") == 1)
		{
			PlayerPrefs.SetInt("level.loading",0);
			Application.LoadLevel("CarSelect");
		}
		else if (Application.GetStreamProgressForLevel("main") == 1 && PlayerPrefs.GetInt("level.loading") == 2)
		{
			PlayerPrefs.SetInt("AdsCounter",0);
			PlayerPrefs.SetInt("level.loading",0);
			Application.LoadLevel("main");
		}
		else if (Application.GetStreamProgressForLevel("main2") == 1 && PlayerPrefs.GetInt("level.loading") == 3)
		{
			PlayerPrefs.SetInt("AdsCounter",0);
			PlayerPrefs.SetInt("level.loading",0);
			Application.LoadLevel("main2");
		}
	}

	void Start ()
	{
		if (PlayerPrefs.GetInt("level.loading") == 1 && PlayerPrefs.GetInt("FirstStart") == 0)
		{
		PlayerPrefs.SetInt("SFXVolume",50);
		PlayerPrefs.SetInt("MusicVolume",50);
		PlayerPrefs.SetInt("TiltButtons",1);
		PlayerPrefs.SetInt("CarModel",0);
		}
	}
}
