using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour
{
	void Start ()
	{
		GameObject.Find("ExitDialog").transform.localScale = new Vector3(0,0,0);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			GameObject.Find("ExitDialog").transform.localScale = new Vector3(2,2,1);
	}
	
	public void ExitYes ()
	{
		if (PlayerPrefs.GetInt("FirstStart") == 0)
		PlayerPrefs.SetInt("FirstStart",1);
		Application.Quit();
	}

	public void ExitNo ()
	{
		GameObject.Find("ExitDialog").transform.localScale = new Vector3(0,0,0);
	}

}
