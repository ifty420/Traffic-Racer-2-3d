using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetScore : MonoBehaviour, IEventSubscriber
{
	ScoreSystem ScoreSys;
	int score;

	void Update ()
	{
		GameObject.Find("Text_BestScore").GetComponent<TextMesh>().text = "" + PlayerPrefs.GetInt("BestScore");
	}

	void Awake()
	{
		EventController.SubscribeToAllEvents(this);

		if (Application.loadedLevelName != "CarSelect")
			ScoreSys = GameObject.Find("Screen_GameGUI").GetComponent<ScoreSystem>();
	}
	
	void OnDestroy()
	{
		EventController.UnsubscribeToAllEvents(this);
	}
	
	#region IEventSubscriber implementation
	
	public void OnEvent(string EventName, GameObject Sender)
	{
		if (EventName == "gui.screen.findistance")
		{
			score = ScoreSys.CarsOvertaken + (int)(ScoreSys.HighSpeed) + (int)(ScoreSys.OppositeLane);

			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score") + score);

			GameObject.Find("Text_Score_Dist").GetComponent<TextMesh>().text = GameObject.Find("Distance_text").GetComponent<Text>().text;
			
			GameObject.Find("Text_Cars_Over").GetComponent<TextMesh>().text = "" + ScoreSys.CarsOvertaken;
			
			GameObject.Find("Text_highSpeed").GetComponent<TextMesh>().text = "" + (int)(ScoreSys.HighSpeed);
			
			GameObject.Find("Text_OppLane").GetComponent<TextMesh>().text = "" + (int)(ScoreSys.OppositeLane);

			GameObject.Find("Text_Score2").GetComponent<TextMesh>().text = "" + score;

			if (score > PlayerPrefs.GetInt("BestScore"))
				PlayerPrefs.SetInt("BestScore", score);

		}
	}
	#endregion

}