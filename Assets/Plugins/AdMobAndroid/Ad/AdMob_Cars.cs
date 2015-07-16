using UnityEngine;
using System;
using System.Collections;

public class AdMob_Cars : MonoBehaviour, IEventSubscriber
{
#if UNITY_ANDROID

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
		EventController.SubscribeToAllEvents(this);
		
		AdMobAndroid.init("pub-9255742339770963");

		if (Application.loadedLevelName == "StartScreen" || Application.loadedLevelName == "SplashScreen" || Application.loadedLevelName == "MapScreen")
			RequestInter2();
	}

	void Start()
	{
		if (Application.loadedLevelName == "CarSelect")
			ShowInterstital();
	}
/*
	void RequestInter0()
	{
		AdMobAndroid.requestInterstital("ca-app-pub-9255742339770963/3186411494");
		
		if (GoogleAnalytics.instance)
			GoogleAnalytics.instance.LogScreen("ad.requestInterstital");
	}
*/	
	void RequestInter1()
	{
		AdMobAndroid.requestInterstital("ca-app-pub-9255742339770963/5144767096");
		
		if (GoogleAnalytics.instance)
			GoogleAnalytics.instance.LogScreen("ad.requestInterstital");
	}
	
	void RequestInter2()
	{
		AdMobAndroid.requestInterstital("ca-app-pub-9255742339770963/8098233497");
		
		if (GoogleAnalytics.instance)
			GoogleAnalytics.instance.LogScreen("ad.requestInterstital");
	}

	void ShowInterstital()
	{
		if (AdMobAndroid.isInterstitalReady())
		{
			AdMobAndroid.displayInterstital();
			if (GoogleAnalytics.instance)
				GoogleAnalytics.instance.LogScreen("ad.showInterstital");
		}
	}

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
/*            case "gui.screen.game":
                //StartCoroutine(IntersitialLoop());
                break;

            case "level.load.menu":
                StopAllCoroutines();
                break;

            case "level.restart":
                StopAllCoroutines();
                break;
*/
			case "car.player.death":
			PlayerPrefs.SetInt("AdsCounter",PlayerPrefs.GetInt("AdsCounter")+1);

			if (PlayerPrefs.GetInt("AdsCounter") > 2)
			{
				ShowInterstital();

				RequestInter2();
				PlayerPrefs.SetInt("AdsCounter",0);
			}
		break;
        }
    }

    #endregion

    IEnumerator IntersitialLoop()
    {
        while (true)
        {
//            yield return new WaitForSeconds(150f);
//            if (PlayerPrefs.GetInt("inapp.car2") == 0 && PlayerPrefs.GetInt("inapp.car3") == 0)
//                RequestInter();
        }
    }
	
    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

/*    
    void OnEnable()
    {
        AdMobAndroidManager.interstitialReceivedAdEvent += interstitialReceivedAdEvent;
    }
    
    
    void OnDisable()
    {
        AdMobAndroidManager.interstitialReceivedAdEvent -= interstitialReceivedAdEvent;
    }
   
    void interstitialReceivedAdEvent()
    {
        if (AdMobAndroid.isInterstitalReady())
        {
            AdMobAndroid.displayInterstital();
            if (GoogleAnalytics.instance)
                GoogleAnalytics.instance.LogScreen("ad.showInterstital");
        }
    }
*/
    #endif
}
