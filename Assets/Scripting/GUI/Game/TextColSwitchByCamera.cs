using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextColSwitchByCamera : MonoBehaviour, IEventSubscriber
{
    public Color Cam1Col;
    public Color Cam2Col;
    public Color Cam3Col;

    private TextMesh _text;

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
        _text = GetComponent<TextMesh>();
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
                _text.color = Cam1Col;
                break;
            case "update.camera.switch2":
                _text.color = Cam2Col;
                break;
            case "update.camera.switch3":
                _text.color = Cam3Col;
                break;
        }
    }
    #endregion
}
