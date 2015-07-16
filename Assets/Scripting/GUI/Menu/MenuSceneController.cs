using UnityEngine;
using System.Collections;

public class MenuSceneController : MonoBehaviour, IEventSubscriber
{
	void Awake() 
    {
        EventController.SubscribeToAllEvents(this);
	}

    void Start()
    {
        EventController.PostEvent("gui.hide", null);
        EventController.PostEvent("gui.screen.splash", null);

        GoogleAnalytics.Log("screen.menu.splash");
    }

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

    private bool _esc = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !_esc)
        {
            GoBack();
            _esc = true;
        }

        if (!Input.GetKey(KeyCode.Escape) && _esc)
        {
            _esc = false;
        }
    }

    private enum Screens
    {
        Splash,
        Main,
        Car,
        Color
    }

    Screens _currentScreen;

    void GoBack()
    {
        switch (_currentScreen)
        {
            case Screens.Splash:
                break;

            case Screens.Main:
                EventController.PostEvent("gui.hide",null);
                EventController.PostEvent("gui.screen.splash",null);
                break;

            case Screens.Car:
                EventController.PostEvent("gui.hide",null);
                EventController.PostEvent("gui.screen.main",null);
                break;

            case Screens.Color:
                //EventController.PostEvent("gui.hide",null);
                EventController.PostEvent("gui.screen.car",null);
                break;
        }
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "gui.screen.game":
                Application.LoadLevel("main");
                break;

            case "update.color.selected.red":
                PlayerPrefs.SetInt("CarColor",0);
                GoogleAnalytics.Log("screen.menu.selectColorRed");
                break;

            case "update.color.selected.blue":
                PlayerPrefs.SetInt("CarColor",1);
                GoogleAnalytics.Log("screen.menu.selectColorBlue");
                break;

            case "update.color.selected.yellow":
                PlayerPrefs.SetInt("CarColor",2);
                GoogleAnalytics.Log("screen.menu.selectColorYellow");
                break;

            case "update.color.selected.black":
                PlayerPrefs.SetInt("CarColor",3);
                GoogleAnalytics.Log("screen.menu.selectColorBlack");
                break;

            case "gui.screen.select.car1":
                PlayerPrefs.SetInt("CarModel",0);
                GoogleAnalytics.Log("screen.menu.selectCar1");
                break;

            case "gui.screen.select.car2":
                GoogleAnalytics.Log("screen.menu.selectCar2");
                PlayerPrefs.SetInt("CarModel",1);
                break;

            case "gui.screen.select.car3":
                GoogleAnalytics.Log("screen.menu.selectCar3");
                PlayerPrefs.SetInt("CarModel",2);
                break;

            case "gui.screen.main":
                GoogleAnalytics.Log("screen.menu.main");
                _currentScreen = Screens.Main;
                break;

            case "gui.screen.comment":
                GoogleAnalytics.Log("screen.menu.comment");
                break;

            case "gui.screen.car":
                GoogleAnalytics.Log("screen.menu.carSelecting");
                _currentScreen = Screens.Car;
                break;

            case "gui.screen.color":
                GoogleAnalytics.Log("screen.menu.colorSelecting");
                _currentScreen = Screens.Color;
                break;

            case "gui.screen.splash":
                _currentScreen = Screens.Splash;
                break;
        }
    }

    #endregion
}
