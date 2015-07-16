using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedUpdater : MonoBehaviour, IEventSubscriber
{
	Text _text;

	float CarSpeed;

	void Start () 
	{
		_text = GetComponent<Text>();
		EventController.Subscribe("update.gui.speed", this);

		CarSpeed = GameObject.FindGameObjectWithTag("PlayerCar").GetComponent<Rigidbody>().velocity.z;
	}
	
	void OnDestroy()
	{
		EventController.Unsubscribe(this);
	}
	
	#region IEventSubscriber implementation
	
	public void OnEvent(string EventName, GameObject Sender)
	{
		if (EventName == "update.gui.speed")
		{
			if (Sender.GetComponent<CarDriver>()._rigidbody.velocity.z > 1)
				_text.text = string.Format("{0}",((int)(Sender.GetComponent<CarDriver>()._rigidbody.velocity.z*1.35f)).ToString());
			else
				_text.text = "0";
		}
	}
	
	#endregion

}