using UnityEngine;
using System.Collections;

public class TimeUpdater : MonoBehaviour,IEventSubscriber
{
    TextMesh _text;
    Renderer _heart;
    
    void Awake () 
    {
        _text = GetComponent<TextMesh>();
        _heart = transform.GetChild(0).GetComponent<Renderer>();
        EventController.Subscribe("update.car.health", this);
        EventController.Subscribe("car.player.immortal.start", this);
        EventController.Subscribe("car.player.immortal.end", this);
    }
    
    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }
    
    #region IEventSubscriber implementation

    bool immortal = false;
    int health = 3;

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch(EventName)
        {
            case "update.car.health":
                health = Sender.GetComponent<PlayerCarBehaviour>().Lifes;
                break;

            case "car.player.immortal.start":
                immortal = true;
                StartCoroutine(HeartBlink());
                break;

            case "car.player.immortal.end":
                immortal = false;
                StopAllCoroutines();
                _heart.enabled = true;
                break;
        }
        UpdateText();
    }

    IEnumerator HeartBlink()
    {
        while (immortal)
        {
            _heart.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _heart.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
        _heart.enabled = true;
    }

    private void UpdateText()
    {
        if (health>=0)
            _text.text = string.Format("{0}",health.ToString());
        else
            _text.text = string.Format("{0}","Death");
    }
    
    #endregion
}
