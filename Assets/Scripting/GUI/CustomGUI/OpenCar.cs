using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenCar : MonoBehaviour
{
	Text carPrice;
	Image Background;
	Text carUnlock;
	Image Background2;

	public void OpenCurrentCar ()
	{
		if (PlayerPrefs.GetInt("Car2Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 3999 && PlayerPrefs.GetInt("CarModel") == 1)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-4000);
			PlayerPrefs.SetInt("Car2Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 2 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car3Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 9999 && PlayerPrefs.GetInt("CarModel") == 2)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-10000);
			PlayerPrefs.SetInt("Car3Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 3 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car4Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 29999 && PlayerPrefs.GetInt("CarModel") == 3)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-30000);
			PlayerPrefs.SetInt("Car4Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 4 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car5Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 49999 && PlayerPrefs.GetInt("CarModel") == 4)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-50000);
			PlayerPrefs.SetInt("Car5Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 5 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car6Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 79999 && PlayerPrefs.GetInt("CarModel") == 5)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-80000);
			PlayerPrefs.SetInt("Car6Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 6 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car7Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 119999 && PlayerPrefs.GetInt("CarModel") == 6)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-120000);
			PlayerPrefs.SetInt("Car7Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 7 unlocked");
		}
		else if (PlayerPrefs.GetInt("Car8Unlocked") == 0 && PlayerPrefs.GetInt("Score") > 149999 && PlayerPrefs.GetInt("CarModel") == 7)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-150000);
			PlayerPrefs.SetInt("Car8Unlocked",1);
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("car 8 unlocked");
		}

		GameObject.Find("Text_Point").GetComponent<Text>().text = "" + PlayerPrefs.GetInt("Score");
	}

	void Start ()
	{
		carPrice = GameObject.Find("CarPrice").GetComponent<Text>();
		Background = GameObject.Find("CarLocker").GetComponent<Image>();
		carUnlock = GameObject.Find("CarUnlock_text").GetComponent<Text>();
		Background2 = GameObject.Find("CarUnlock").GetComponent<Image>();

		// BABLo DOpolniTELNOE
		// PlayerPrefs.SetInt("Score",20000);
	}

	void Update ()
	{
		if (PlayerPrefs.GetInt("CarModel") == 1)
		{
			carPrice.text = "Car Locked 4,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 2)
		{
			carPrice.text = "Car Locked 10,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 3)
		{
			carPrice.text = "Car Locked 30,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 4)
		{
			carPrice.text = "Car Locked 50,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 5)
		{
			carPrice.text = "Car Locked 80,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 6)
		{
			carPrice.text = "Car Locked 120,000 points needed!";
		}
		else if (PlayerPrefs.GetInt("CarModel") == 7)
		{
			carPrice.text = "Car Locked 150,000 points needed!";
		}


		if (PlayerPrefs.GetInt("Car2Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 1)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car3Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 2)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car4Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 3)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car5Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 4)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car6Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 5)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car7Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 6)
			IsLocked ();
		else if (PlayerPrefs.GetInt("Car8Unlocked") == 0 && PlayerPrefs.GetInt("CarModel") == 7)
			IsLocked ();
		else
		{
			carPrice.enabled = false;
			Background.color = new Color(1,0,0,0);
			carUnlock.enabled = false;
			Background2.color = new Color(1,0,0,0);
		}
	
	}

	void IsLocked ()
	{
		carPrice.enabled = true;
		Background.color = new Color(1,0,0,1);
		carUnlock.enabled = true;
		Background2.color = new Color(1,0,0,1);
	}
}
