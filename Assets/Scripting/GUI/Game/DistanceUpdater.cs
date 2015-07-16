using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[RequireComponent(typeof(TextMesh))]
public class DistanceUpdater : MonoBehaviour, IEventSubscriber
{
    Text _text;

	void Start () 
    {
//        _text = GetComponent<TextMesh>();
		_text = GetComponent<Text>();
        EventController.Subscribe("update.gui.distance", this);
	}

    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "update.gui.distance")
        {
//            _text.text = string.Format("{0}",((int)(Sender.GetComponent<PlayerCarBehaviour>().Distance/3.25f)).ToString());
            _text.text = string.Format("{0}",((int)(Sender.GetComponent<PlayerCarBehaviour>().Distance/3.25f)).ToString());
		}
    }

    #endregion
}
