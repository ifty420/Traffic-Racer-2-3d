using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GUIObject : MonoBehaviour, IEventSubscriber
{
    protected virtual void AwakeProc() {}
    protected virtual void EventProc(string EventName, GameObject Sender) {}

    public string ShowOnEvent = "";

    void Awake()
    {
        AwakeProc();
        EventController.SubscribeToAllEvents(this);
    }

    public void OnEvent(string EventName, GameObject Sender)
    {
        EventProc(EventName, Sender);
        if (EventName == "gui.hide")
        {
            if (gameObject != null)
            {
                if (GetComponent<Renderer>())
                    GetComponent<Renderer>().enabled = false;
                if (GetComponent<Collider>())
                    GetComponent<Collider>().enabled = false;
            }
        } else if (EventName == ShowOnEvent)
        {
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().enabled = true;
            if (GetComponent<Collider>())
                GetComponent<Collider>().enabled = true;
        }
    }

    protected virtual void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }
}
