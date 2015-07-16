using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour 
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
			GetComponent<Text>().text = string.Format("{0}", PlayerPrefs.GetInt("Score"));
        else
			GetComponent<Text>().text = string.Format("{0}", 0);
    }
}
