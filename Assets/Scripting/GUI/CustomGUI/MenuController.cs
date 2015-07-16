using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
	GameObject[] SelectedCar;
	GameController control;

	public void Start ()
	{

		if (Application.loadedLevel == 1)
		{
//		PlayerPrefs.SetInt("CarModel",0);
			SelectedCar = GameObject.FindGameObjectsWithTag("PlayerCar");
			
			UpdateColor ();

			GameObject.Find("1Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("2Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("3Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("4Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("5Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("6Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("7Car 1").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("8Car 1").transform.localScale = new Vector3(0,0,0);

			// Preselected Car
			switch (PlayerPrefs.GetInt("CarModel"))
			{
			case 0:
				GameObject.Find("1Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 1:
				GameObject.Find("2Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 2:
				GameObject.Find("3Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 3:
				GameObject.Find("4Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 4:
				GameObject.Find("5Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 5:
				GameObject.Find("6Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 6:
				GameObject.Find("7Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;
			case 7:
				GameObject.Find("8Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
				break;

				control = GameObject.Find("Controllers").GetComponent<GameController>();
			}
		}
	}

	public void CarPrev ()
	{
		if (GameObject.Find("1Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("8Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("1Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",7);
		}
		else if (GameObject.Find("2Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("1Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("2Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",0);
		}
		else if (GameObject.Find("3Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("2Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("3Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",1);
		}
		else if (GameObject.Find("4Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("3Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("4Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",2);
		}
		else if (GameObject.Find("5Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("4Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("5Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",3);
		}
		else if (GameObject.Find("6Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("5Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("6Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",4);
		}
		else if (GameObject.Find("7Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("6Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("7Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",5);
		}
		else if (GameObject.Find("8Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("7Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("8Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",6);
		}
	}

	public void CarNext ()
	{
		if (GameObject.Find("1Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("2Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("1Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",1);
		}
		else if (GameObject.Find("2Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("3Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("2Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",2);
		}
		else if (GameObject.Find("3Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("4Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("3Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",3);
		}
		else if (GameObject.Find("4Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("5Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("4Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",4);
		}
		else if (GameObject.Find("5Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("6Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("5Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",5);
		}
		else if (GameObject.Find("6Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("7Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("6Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",6);
		}
		else if (GameObject.Find("7Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("8Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("7Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",7);
		}
		else if (GameObject.Find("8Car 1").transform.localScale.x == 8.5f)
		{
			GameObject.Find("1Car 1").transform.localScale = new Vector3(8.5f,8.5f,8.5f);
			GameObject.Find("8Car 1").transform.localScale = new Vector3(0,0,0);
			PlayerPrefs.SetInt("CarModel",0);
		}
	}

	public void LoadMap()
	{
		if (PlayerPrefs.GetInt("Car2Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 1)
				Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car3Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 2)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car4Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 3)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car5Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 4)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car6Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 5)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car7Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 6)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("Car8Unlocked") == 1 && PlayerPrefs.GetInt("CarModel") == 7)
			Application.LoadLevel("MapScreen");
		else if (PlayerPrefs.GetInt("CarModel") == 0)
			Application.LoadLevel("MapScreen");
	}

	public void SelectColorRed ()
	{
		PlayerPrefs.SetInt("CarColor",0);
		UpdateColor ();
	}
	
	public void SelectColorYellow ()
	{
		PlayerPrefs.SetInt("CarColor",1);
		UpdateColor ();
	}
	
	public void SelectColorBlue ()
	{
		PlayerPrefs.SetInt("CarColor",2);
		UpdateColor ();
	}
	
	public void SelectColorBlack ()
	{
		PlayerPrefs.SetInt("CarColor",3);
		UpdateColor ();
	}

	void UpdateColor ()
	{
			int i;

		for (i = 0; i < SelectedCar.Length; i++)
			switch (PlayerPrefs.GetInt("CarColor"))
			{
			case 0:
				SelectedCar[i].GetComponent<Renderer>().material.color = new Color(1,0,0);
				break;
			case 1:
				SelectedCar[i].GetComponent<Renderer>().material.color = new Color(1,0.5f,0);
				break;
			case 2:
				SelectedCar[i].GetComponent<Renderer>().material.color = new Color(0,0,1);
				break;
			case 3:
				SelectedCar[i].GetComponent<Renderer>().material.color = new Color(0.1f,0.1f,0.1f);
				break;
		}

	}

	public void LoadMenu ()
	{
		PlayerPrefs.SetInt("AdsCounter",0);
		PlayerPrefs.SetInt("level.loading",1);
		Application.LoadLevel("SplashScreen");
	}

	public void LoadMap1 ()
	{
		PlayerPrefs.SetInt("level.loading",2);
		Application.LoadLevel("SplashScreen");
	}

	public void LoadMap2 ()
	{
		PlayerPrefs.SetInt("level.loading",3);
		Application.LoadLevel("SplashScreen");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void OtherGamesLink ()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=i6+Games");
	}
	
	public void hideTutorial ()
	{
		GameObject.Find("PanelTutorial").transform.localScale = new Vector3(0,0,0);
		//control.pausedCounter = 0;
	}
	
	public void hideTutorial2 ()
	{
		GameObject.Find("PanelTutorial2").transform.localScale = new Vector3(0,0,0);
		//control.pausedCounter = 0;
	}
	
	public void hideTutorialHS ()
	{
		GameObject.Find("PanelHighSpeed").transform.localScale = new Vector3(0,0,0);
		//control.pausedCounter = 0;
	}
}
