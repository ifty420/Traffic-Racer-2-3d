using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class InAppButton : Button 
{
    public string Sku;
    public Sprite BlockedSrpite;

    private SpriteRenderer _renderer;
    private Sprite _baseSprite;
    private string[] _press;
    private string[] _release;

    protected override void AwakeProc()
    {
        base.AwakeProc();
        Init();
        UpdateState();
    }

    private void UpdateState()
    {
        if (!Bought())
            MakeBlocked();
        else
            MakeUnBlocked();
    }

    private void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _baseSprite = _renderer.sprite;
        _press = CallWhenPress;
        _release = CallWhenRelease;
    }

    private void MakeBlocked()
    {
        _renderer.sprite = BlockedSrpite;
        CallWhenPress = new string[0];
        CallWhenRelease = new string[0];
    }

    private void MakeUnBlocked()
    {
        _renderer.sprite = _baseSprite;
        CallWhenPress = _press;
        CallWhenRelease = _release;
    }

    private bool Bought()
    {
        return PlayerPrefs.HasKey(Sku) && PlayerPrefs.GetInt(Sku) == 1;
    }

    protected override void EventProc(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "update.inapp":
                UpdateState();
                break;

            default:
                base.EventProc(EventName, Sender);
                break;
        }
    }

    public override void OnPress(Gesture G)
    {
        base.OnPress(G);
        if (!Bought())
			if (PlayerPrefs.GetInt("Score") > 1999)
		{
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")-2000);
			PlayerPrefs.SetInt(Sku,1);

			UpdateState();
		}
            //GoogleIAB.purchaseProduct(Sku);
    }
}
