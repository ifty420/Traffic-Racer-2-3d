using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem:MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    public Text ScoreText;
    public float Score { get; private set; }

	Text score_50;
	Text score_hs;
	Text score_ol;
	float score_hs_text;
	float score_ol_text;

	float score_50_alpha;
	GameController control;

	public float HighSpeed;
	public float OppositeLane;
	public int CarsOvertaken;

	private void Start()
    {
        Instance = this;

		score_50 = GameObject.Find("Score_50").GetComponent<Text>();
		score_hs = GameObject.Find("Score_highspeed").GetComponent<Text>();
		score_ol = GameObject.Find("Score_oppositeLane").GetComponent<Text>();

		control = GameObject.Find("Controllers").GetComponent<GameController>();
	}


    private void Update()
    {
        if (PlayerIsOnRoad)
        {
            if (PlayerDoingHighSpeed)
            {
                Score += 3*Time.deltaTime*(PlayerSpeed/70f);
				HighSpeed += 3*Time.deltaTime*(PlayerSpeed/70f);

				// High Speed UI
				score_hs.color = new Color(0.1f,0.1f,0.1f,1);
				score_hs_text += Time.deltaTime;
				score_hs.text = Math.Round(score_hs_text * 10)/10 + " sec Going over 100 km/h";

				if (PlayerPrefs.GetInt("FirstStart") == 0)
				{
					GameObject.Find("PanelHighSpeed").transform.localScale = new Vector3(1,1,1);
					//control.pausedCounter = -1;
					PlayerPrefs.SetInt("FirstStart",1);
				}
            }
			else
			{
				score_hs_text = 0;
				score_hs.color = new Color(0.1f,0.1f,0.1f,0);
			}
        }
        ScoreText.text = Score.ToString("F0");

		if (score_50_alpha > 0)
			score_50_alpha -= 0.01f;

		score_50.color = new Color(0.1f,0.1f,0.1f,score_50_alpha);

		//Going opposite lane
		if (PlayerIsOppositeLane)
		{
			Score += 6*Time.deltaTime*(PlayerSpeed/70f);
			OppositeLane += 6*Time.deltaTime*(PlayerSpeed/70f);

			score_ol.color = new Color(0.1f,0.1f,0.1f,1);
			score_ol_text += Time.deltaTime;
			score_ol.text = Math.Round(score_ol_text * 10)/10 + " sec Going opposite lane";
		}
		else
		{
			score_ol_text = 0;
			score_ol.color = new Color(0.1f,0.1f,0.1f,0);
		}
	}

    public void CarOvertook()
    {
        if (PlayerIsOnRoad)
        {
            Score += 50;
			CarsOvertaken += 50;

			score_50_alpha = 1;
        }
    }


    public bool PlayerDoingHighSpeed
    {
        get { return PlayerSpeed > 75; }
    }

    private bool PlayerIsOnRoad
    {
        get { return PlayerX > -9.3f && PlayerX < 9.7f; }
    }

	private bool PlayerIsOppositeLane
	{
		get { return PlayerX > -9.3f && PlayerX < 0; }
	}
	
	private float PlayerSpeed
    {
        get { return PlayerCarBehaviour.Instance.GetComponent<Rigidbody>().velocity.z; }
    }


    private float PlayerX
    {
        get { return PlayerCarBehaviour.Instance.transform.position.x; }
    }
}
