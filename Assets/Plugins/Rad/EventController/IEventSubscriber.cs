using System;
using UnityEngine;

public interface IEventSubscriber
{
    void OnEvent(string EventName, GameObject Sender);
}