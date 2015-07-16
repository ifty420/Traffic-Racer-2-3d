using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IEventSubscriber
{
	private bool _deathScreen;
	public int pausedCounter;
	private TextMesh counter;
	private bool FirstStart = true;
	SoundController Car;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);

		counter = GameObject.Find("Text_Counter").GetComponent<TextMesh>();
	}

    void Start()
    {
        EventController.PostEvent("gui.hide", null);
        EventController.PostEvent("gui.screen.game.show", null);
        GoogleAnalytics.Log("screen.game.start");
	}

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "gui.screen.pause":
                if (!_deathScreen)
                {
                    Time.timeScale = 0;
					pausedCounter = -1;
                }
                GoogleAnalytics.Log("screen.main.pause");
				GameObject.Find("NitroBar").GetComponent<Image>().enabled = false;
				GameObject.Find("PanelSettings").transform.localScale = new Vector3(1,1,1);
			break;

            case "gui.screen.game.show":
				if (FirstStart)
				{
					Car = GameObject.FindGameObjectWithTag("PlayerCar").GetComponent<SoundController>();
					AudioUpdate ();

					FirstStart = false;
					pausedCounter = 0;
					GameObject.Find("PanelSettings").transform.localScale = new Vector3(0,0,0);
				}
				else
				{
					pausedCounter = 230;
					GameObject.Find("NitroBar").GetComponent<Image>().enabled = true;
					GameObject.Find("PanelSettings").transform.localScale = new Vector3(0,0,0);
				}
			// Controls Switch
			if (PlayerPrefs.GetInt("TiltButtons") == 1)
			{
				GameObject.Find("Screen_GameGUI2").transform.localPosition = new Vector3(0,-1000,0);
				GameObject.Find("Button_Breaking").transform.localPosition = new Vector3(-711,-396,-32);
			}
			else
			{
				GameObject.Find("Screen_GameGUI2").transform.localPosition = new Vector3(0,0,0);
				GameObject.Find("Button_Breaking").transform.localPosition = new Vector3(441,-396,-32);
			}	
				AudioUpdate ();

			//Tutorial
			if (PlayerPrefs.GetInt("FirstStart") == 0)
			{
				if (PlayerPrefs.GetInt("TiltButtons") == 1)
					{
						GameObject.Find("PanelTutorial").transform.localScale = new Vector3(1,1,1);
						//Time.timeScale = 0;
						//pausedCounter = -1;
					}
				else
				{
					GameObject.Find("PanelTutorial2").transform.localScale = new Vector3(1,1,1);
					//Time.timeScale = 0;
					//pausedCounter = -1;
				}
			}
			break;
			
			case "level.restart":
                Time.timeScale = 1;
                GoogleAnalytics.Log("screen.main.restart");
                Application.LoadLevel(Application.loadedLevel);
                break;

            case "level.load.menu":
                Time.timeScale = 1;
                GoogleAnalytics.Log("screen.main.loadMenu");
                Application.LoadLevel("CarSelect");
                break;

            case "car.player.death":
                Time.timeScale = 0;
				_deathScreen = true;

                int cs = (int)Sender.GetComponent<PlayerCarBehaviour>().Distance;
                int os = PlayerPrefs.HasKey("Score")?PlayerPrefs.GetInt("Score"):0;
//				PlayerPrefs.SetInt("Score",Mathf.Max(cs,os));
				GoogleAnalytics.Log("screen.main.playerDeath");
                EventController.PostEvent("gui.hide",null);
                EventController.PostEvent("gui.screen.findistance",null);
                EventController.PostEvent("gui.screen.pause",null);
				GameObject.Find("NitroBar").GetComponent<Image>().enabled = false;

			PlayerPrefs.SetInt("FirstStart",1);
			GameObject.Find("PanelSettings").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("myUI").transform.localScale = new Vector3(0,0,0);
			break;

		case "level.more":
			Application.OpenURL("https://play.google.com/store/apps/developer?id=i6+Games");
			break;

		}
    }

	void Update()
	{

		if (pausedCounter > 0)
		{
			pausedCounter -= 2;
			counter.text = "" + pausedCounter/60;
			counter.color = new Color(1,1,1,1);
		}
		else if (pausedCounter != -1)
		{
			if (!_deathScreen) Time.timeScale = 1;
				counter.color = new Color(1,1,1,0);
		}

	}

    #endregion
	

	void AudioUpdate ()
	{
		//Adjust Volume Settings From Menu
		Car.DVolume = 0.4f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.EVolume = 0.4f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.FVolume = 0.4f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.KVolume = 0.7f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.LVolume = 0.4f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.windVolume = 0.4f*(float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.crashLowVolume = (float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.crashHighVolume = (float)(PlayerPrefs.GetInt("SFXVolume"))/100;
		Car.backgroundMusic.volume = 0.6f*(float)(PlayerPrefs.GetInt("MusicVolume"))/100;
		Car.carAudio.volume = (float)(PlayerPrefs.GetInt("SFXVolume"))/100;
	}
}
