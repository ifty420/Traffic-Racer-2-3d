using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class Ads : GUIClickable
{
    public int BlockID = -1;
    public string ImageURL = "";
    public string OpenURL = "";
    public static string JSONText = "";
    protected Texture2D AdTexture = null;

    public string IASURL = "";

    protected override void AwakeProc()
    {
        base.AwakeProc();
        StartCoroutine(LoadJSON());
    }

    void Start()
    {
		//GetComponent<Renderer>().material.SetColor("_Tint",new Color(1,1,1,0));
		GetComponent<Renderer>().material.color = new Color(1,1,1,0f);
		GetComponent<Collider>().enabled = true;

        if (JSONText != "loading..." && JSONText != "")
            EventProc("OnLoadJson", null);
    }

    public override void OnPress(Gesture G)
    {
        base.OnPress(G);
        print("tada");
        Application.OpenURL(this.OpenURL);
        GoogleAnalytics.Log("ad.IAS.click." + gameObject.name);
    }

    protected override void EventProc(string EventName, GameObject Sender)
    {
        if (EventName == base.ShowOnEvent && GetComponent<Renderer>().material.GetColor("_Color").a == 1)
            GoogleAnalytics.Log("ad.IAS.show." + gameObject.name);
        switch (EventName)
        {
            case "OnLoadJson":
                StartCoroutine(LoadTexture());
                break;
                
            case "OnErrorJson":
                if (IASURL != "" && JSONText == "")
                {
                    JSONText = "loading...";
                    StartCoroutine(LoadJSON(3));
                }
                break;
                
            case "OnLoadTexture":
                if (Sender == gameObject && AdTexture)
                {
                    AdTexture.filterMode = FilterMode.Trilinear;
                    GetComponent<Renderer>().material.mainTexture = AdTexture;
                    if (GetComponent<Renderer>().enabled)
                        GoogleAnalytics.Log("ad.IAS.show." + gameObject.name);
                }
                break;

            case "OnLoadTextureFailded":
                StartCoroutine(LoadTexture(3));
                break;

			case "OnLoadTextureSucceed":
				GetComponent<Renderer>().material.color = new Color(1,1,1,1);
				break;

            case "gui.hide":
                if (GetComponent<Renderer>().enabled)
                    StartCoroutine(LoadTexture());
                break;

            case "update.inapp":
                if (!(PlayerPrefs.GetInt("inapp.car2") == 0 && PlayerPrefs.GetInt("inapp.car3") == 0))
                {
                    GetComponent<Renderer>().enabled = false;
                    GetComponent<Collider>().enabled = false;
                }
                break;
        }
        base.EventProc(EventName, Sender);
    }

    private bool _loading = false;

    IEnumerator LoadJSON(float WaitTime = 0) 
    {
        if (!_loading)
        {
            //print("start loading IAS");
            _loading = true;
            yield return new WaitForSeconds(WaitTime);
            WWW w = new WWW(IASURL);
            yield return w;
            _loading = false;
            if (w.error != null)
            {
                JSONText = "";
                //print("finish loading IAS with error ["+w.error.ToString()+"]");
                EventController.PostEvent("OnErrorJson", null);
            } else
            {
                JSONText = w.text;
                print("finish loading IAS ["+w.text+"]");
                EventController.PostEvent("OnLoadJson", null);
            }
        } //else
            //print("loading IAS alredy started");
    }

    private int GetNext(string Name,int Max)
    {
        //if (PlayerPrefs.HasKey(Name))
        //{
        //    int num = PlayerPrefs.GetInt(Name);
        //    num++;
        //    if (num >= Max)
        //        num = 0;
        //    PlayerPrefs.SetInt(Name, num);
        //    return num;
        //} else
        //{
        //    PlayerPrefs.SetInt(Name,0);
        //    return 0;
        //}
		System.Random random = new System.Random ();
		return random.Next ( Max );
    }

    [ContextMenu("Remobe Purchases")]
    void RemovePurch()
    {
        PlayerPrefs.SetInt("inapp.car2",0);
        PlayerPrefs.SetInt("inapp.car3",0);
    }

    [ContextMenu("Purchase all cars")]
    void Purch()
    {
        PlayerPrefs.SetInt("inapp.car2",1);
        PlayerPrefs.SetInt("inapp.car3",1);
    }

    IEnumerator LoadTexture(float DeltaTime = 0)
    {
		//print("BlockID: "BlockID.ToString());
        if (PlayerPrefs.GetInt("inapp.car2") == 0 && PlayerPrefs.GetInt("inapp.car3") == 0)
        {
            if (DeltaTime != 0)
                print("Wait for delta and get tex again");
            yield return new WaitForSeconds(DeltaTime);

            JSONNode j = JSON.Parse(JSONText);
            List<JSONNode> l = new List<JSONNode>();
            if (j != null)
            {
                JSONNode n = j ["slots"];
                for (int i=0; i<n.Count; i++)
                    if (n [i] ["slotid"].Value.StartsWith(BlockID.ToString()))
                        l.Add(n [i]);
                do 
                {
                    n = l [GetNext(BlockID.ToString(), l.Count)];
                    OpenURL = n ["adurl"];
                } while (OpenURL.Contains("com.i6.TrafficRacer3D"));
                //print(string.Format("GO [{0}], json parsed, start loading texture [{1}]",gameObject.name,n ["imgurl"])); 
                WWW www = new WWW(ImageURL = n ["imgurl"]);
                yield return www;
                if (www.error == null)
                {
                    AdTexture = www.texture;
                    //print(string.Format("GO [{0}], texture loaded",gameObject.name)); 
                    EventProc("OnLoadTexture", gameObject);
                } else
                {
                    //print(string.Format("GO [{0}], loading of texture failed [{1}]",gameObject.name, www.error.ToString())); 
                    EventProc("OnLoadTextureFailded", gameObject);
                }

				if (www.isDone == true)
					EventProc("OnLoadTextureSucceed", gameObject);
            } //else
                //print(string.Format("GO [{0}], json pars failed",gameObject.name));
        }
    }
}
