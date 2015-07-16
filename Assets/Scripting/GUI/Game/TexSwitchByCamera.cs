using UnityEngine;
using System.Collections;

public class TexSwitchByCamera : MonoBehaviour, IEventSubscriber
{
    public Texture Cam1Tex;
    public Texture Cam2Tex;
    public Texture Cam3Tex;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
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
            case "update.camera.switch1":
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material.SetTexture("_MainTex",Cam1Tex);
                break;
            case "update.camera.switch2":
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material.SetTexture("_MainTex",Cam2Tex);
                break;
            case "update.camera.switch3":
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().material.SetTexture("_MainTex",Cam3Tex);
                break;
        }
    }
    #endregion
}
