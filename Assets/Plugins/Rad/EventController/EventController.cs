/*
 * Event Controller (for Unity3D) v 0.4
 * 
 * author: Radomir Slaboshpitsky, VallVerk game dev. studio, 2014
 * e-mail: radiys92@gamil.com
 */

// v0.3 preview:
// 1. fixed bugs when reloading scene

/* v0.4 update:
 * Addet PostEvent(... , delay) 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventController : Singleton<EventController> 
{
    public delegate void Func(string EventName, GameObject Sender);

    private struct EventPair
    {
        public string Name;
        public Func Events;
    }

    private EventPair[] _pairs;
    private Func _onAll = (a,b) => {};

    public EventController()
    {
        _pairs = new EventPair[0];
    }

    public static void Subscribe(string EventName, IEventSubscriber Subscriber)
    {
        bool been = false;
        for (int i=0; i<Instance._pairs.Length; i++)
        {
			if (Instance._pairs[i].Name == EventName)
            {
				Instance._pairs[i].Events += Subscriber.OnEvent;
                been = true;
                break;
            }
        }
        if (!been)
        {
            EventPair x = new EventPair();
            x.Name = EventName;
            x.Events = (a,b) => {};
            x.Events += Subscriber.OnEvent;
			Array.Resize<EventPair>(ref Instance._pairs,Instance._pairs.Length+1);
			Instance._pairs[Instance._pairs.Length-1] = x;
        }
    }

    public static void SubscribeToAllEvents(IEventSubscriber Subscciber)
    {
		Instance._onAll += Subscciber.OnEvent;
    }

    public static void UnsubscribeToAllEvents(IEventSubscriber Subscciber)
    {
        if (Instance)
		    Instance._onAll -= Subscciber.OnEvent;
    }

    public static void Unsubscribe(string EventName, IEventSubscriber Subscriber)
    {
        if (!Instance)
            return;
		for (int i=0; i<Instance._pairs.Length; i++)
        {
			if (Instance._pairs[i].Name == EventName)
            {
				Instance._pairs[i].Events -= Subscriber.OnEvent;
                break;
            }
        }
    }

    public static void Unsubscribe(IEventSubscriber Subscriber)
    {
        if (Instance != null)
            for (int i=0; i<Instance._pairs.Length; i++)
                Instance._pairs [i].Events -= Subscriber.OnEvent;
    }

    public static void PostEvent(string EventName,GameObject Sender)
    {
		for (int i=0; i<Instance._pairs.Length; i++)
        {
			if (Instance._pairs[i].Name == EventName)
            {
				Instance._pairs[i].Events(EventName,Sender);
                break;
            }
        }
		Instance._onAll(EventName, Sender);
    }

    public static void PostEvent(string EventName,GameObject Sender, float Delay)
    {
        EventController.Instance.StartCoroutine(EventController.Instance.PostEventWithDelay(EventName, Sender, Delay));
    }

    private IEnumerator PostEventWithDelay(string EventName, GameObject Sender, float Delay)
    {
        yield return new WaitForSeconds(Delay);
        PostEvent(EventName, Sender);
    }
}
