using UnityEngine;
using System.Collections;

public class CarSelecting : MonoBehaviour, IEventSubscriber
{
	public SpriteRenderer Car1;
	public SpriteRenderer Car2;
	public SpriteRenderer Car3;
	public SpriteRenderer Car4;
	public SpriteRenderer Car5;
	public SpriteRenderer Car6;
	public SpriteRenderer Car7;
	public SpriteRenderer Car8;

	public AudioClip SlideSound = null;
    public Camera MainCamera;
    public string ShowScreenEvent = "gui.screen.car";

    private Vector3[] _mainLocalPos = new Vector3[3];

    void Awake()
    {
        EventController.SubscribeToAllEvents(this);
    }

    void OnDestroy()
    {
        EventController.UnsubscribeToAllEvents(this);
    }

    void Start()
    {
        float camH = MainCamera.orthographicSize;
        float camW = (MainCamera.orthographicSize) * (MainCamera.pixelWidth/MainCamera.pixelHeight);
        Sprite sprite = Car1.sprite;
        Car1.transform.localScale = new Vector3(1,1,1) * ((camH * 2) * 1 / sprite.bounds.size.y);
        Car2.transform.localScale = new Vector3(1,1,1) * ((camH * 2) * 1 / sprite.bounds.size.y);
        Car3.transform.localScale = new Vector3(1,1,1) * ((camH * 2) * 1 / sprite.bounds.size.y);

        if (Car1.bounds.size.x * 3 > camW)
        {
            Car1.transform.localScale = new Vector3(1,1,1) * ((camW/1.5f) * (1 / sprite.bounds.size.x));
            Car2.transform.localScale = new Vector3(1,1,1) * ((camW/1.5f) * (1 / sprite.bounds.size.x));
            Car3.transform.localScale = new Vector3(1,1,1) * ((camW/1.5f) * (1 / sprite.bounds.size.x));
        }

        _mainLocalPos [1] = new Vector3(0,0,-5);
        float w = Car1.bounds.size.x;
        _mainLocalPos [0] = _mainLocalPos[1] + new Vector3(-w, 0, 0);
        _mainLocalPos [2] = _mainLocalPos[1] + new Vector3(w, 0, 0);

        float h = -Car1.bounds.size.y*2;
        Car1.transform.localPosition = _mainLocalPos [0] + new Vector3(0, h, 0);
        Car2.transform.localPosition = _mainLocalPos [1] + new Vector3(0, h, 0);
        Car3.transform.localPosition = _mainLocalPos [2] + new Vector3(0, h, 0);

        StopAllCoroutines();
    }

    IEnumerator Slide(Transform Target, Vector3  LocalFrom, Vector3 LocalTo, float T, float Wait = 0)
    {
        yield return new WaitForSeconds(Wait);
        float delta = 0;
        if (SlideSound != null)
            PlaySound(SlideSound);
        for (float time = Time.time;delta<T;delta = Time.time - time)
        {
            Target.localPosition = Vector3.Lerp(LocalFrom,LocalTo,delta/T);
            yield return new WaitForFixedUpdate();
        }   
        Target.localPosition = LocalTo;
    }

    void AnimateShow()
    {
        float h = -Car1.bounds.size.y*2;
        StartCoroutine(Slide(Car1.transform, _mainLocalPos [0] + new Vector3(0, h, 0), _mainLocalPos [0], 0.3f, 0.1f));
        StartCoroutine(Slide(Car2.transform, _mainLocalPos [1] + new Vector3(0, h, 0), _mainLocalPos [1], 0.3f, 0.4f));
        StartCoroutine(Slide(Car3.transform, _mainLocalPos [2] + new Vector3(0, h, 0), _mainLocalPos [2], 0.3f, 0.7f));
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == ShowScreenEvent)
            AnimateShow();
        else
        {
            float h = -Car1.bounds.size.y * 2;
            switch (EventName)
            {
                case "gui.hide":
                    Car1.transform.localPosition = _mainLocalPos [0] + new Vector3(0, h, 0);
                    Car2.transform.localPosition = _mainLocalPos [1] + new Vector3(0, h, 0);
                    Car3.transform.localPosition = _mainLocalPos [2] + new Vector3(0, h, 0);
                    break;

                case "gui.screen.select.car1":
                    StartCoroutine(Slide(Car1.transform, _mainLocalPos [0], _mainLocalPos [0] + new Vector3(0, -h, 0), 0.3f, 0.5f));
                    StartCoroutine(Slide(Car2.transform, _mainLocalPos [1], _mainLocalPos [1] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    StartCoroutine(Slide(Car3.transform, _mainLocalPos [2], _mainLocalPos [2] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    break;

                case "gui.screen.select.car2":
                    StartCoroutine(Slide(Car1.transform, _mainLocalPos [0], _mainLocalPos [0] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    StartCoroutine(Slide(Car2.transform, _mainLocalPos [1], _mainLocalPos [1] + new Vector3(0, -h, 0), 0.3f, 0.5f));
                    StartCoroutine(Slide(Car3.transform, _mainLocalPos [2], _mainLocalPos [2] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    break;

                case "gui.screen.select.car3":
                    StartCoroutine(Slide(Car1.transform, _mainLocalPos [0], _mainLocalPos [0] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    StartCoroutine(Slide(Car2.transform, _mainLocalPos [1], _mainLocalPos [1] + new Vector3(0, h, 0), 0.3f, 0.1f));
                    StartCoroutine(Slide(Car3.transform, _mainLocalPos [2], _mainLocalPos [2] + new Vector3(0, -h, 0), 0.3f, 0.5f));
                    break;
            }
        }
    }

    #endregion

    protected void PlaySound(AudioClip Clip)
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.clip = Clip;
        s.Play();
        Destroy(s, Clip.length);
    }
}
