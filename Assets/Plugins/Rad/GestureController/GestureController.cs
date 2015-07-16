// ------------------------------------------------------------------------------
// Gesture Controller v0.5
// Class, which implements tools for control gestures from touches and mouse.
// Enabled multitouch.
// Implemented as singleton, don't need initialization.
// For uses functional, use OnGestureStart/OnGestureEnd delegats.
//
// author: Radomir Slaboshpitsky 
// mail: radiys92@gmail.com
// 2014
// ------------------------------------------------------------------------------

// v0.4 preview:
// 1. added StopGesture function
// 2. added Windows Phone 8 support
// 3. fixed bugs when reloading scene

/*
 * v 0.5 upd:
 * 1. updated looper funtion (start in coroutine)
 *      Need to be independent within Time.scale
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureController : Singleton<GestureController> 
{
    Vector2 _pos;
    float _time;
    bool _pressed;
    Gesture _lg;
    List<Gesture> g;

    public int CountOfGestures 
    {
        get
        {
            if (g==null)
                return 1;
            else
                return g.Count;
        }
    }

    public delegate void func(Gesture g);
    public delegate void Ffunc ();
    private func _onGestureStart;
    private func _onGestureEnd;

    public static func OnGestureStart
    {
        get { return Instance == null? null: Instance._onGestureStart; }
        set { if (Instance!=null) Instance._onGestureStart = value; }
    }

    public static func OnGestureEnd
    {
        get { return Instance == null? null : Instance._onGestureEnd; }
        set { if (Instance!=null) Instance._onGestureEnd = value; }
    }

    private Ffunc FUpdate;

    public override void OnDestroy ()
    {
        base.OnDestroy ();
        _onGestureEnd = (a) => {};
        _onGestureStart = (a) => {};
        foreach (Gesture gest in g)
            gest.OnGestureStay = (a) => {};
    }

    public GestureController()
    {
        _pressed = false;
        g = new List<Gesture>();
        _onGestureEnd = (a) => {};
        _onGestureStart = (a) => {};
    }

    private MouseTouch GetDeltaTouch()
    {
        MouseTouch touch = new MouseTouch();
        touch.position = Input.mousePosition;
        touch.deltaPosition = touch.position-_pos;
        touch.Time = Time.time;
        touch.deltaTime = touch.Time - _time;
        touch.phase = TouchPhase.Moved;
        touch.buttonID = 0;
        _time = touch.Time;
        _pos = touch.position;
        return touch;
    }

    private Vector2 v3tov2 (Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    private MouseTouch GetTouch(UnityEngine.Touch t)
    {
        MouseTouch touch = new MouseTouch();
        touch.position = t.position;
        touch.deltaPosition = t.deltaPosition;
        touch.deltaTime = t.deltaTime;
        touch.Time = Time.time;
        touch.phase = t.phase;
        touch.buttonID = t.fingerId;
        return touch;
    }

    private bool _break = false;

    void FixedMouseUpdate()
    {
        if (_pressed)
        if (v3tov2(Input.mousePosition) != _pos)
            _lg.AddTouch(GetDeltaTouch());

        if (_break && _pressed)
        {
            _pressed = false;
            // on finish gesture
            MouseTouch touch = GetDeltaTouch();
            touch.phase = TouchPhase.Ended;
            _lg.AddTouch(touch);
            _onGestureEnd(_lg);
            return;
        }

        if (!Input.GetMouseButton(0) && _break)
            _break = false;
        else if (_break)
            return;

        if (Input.GetMouseButton(0) && !_pressed)
        {
            _pressed = true;
            _pos = Input.mousePosition;
            _time = Time.time;
            // on start gesture
            _lg = new Gesture();
            MouseTouch touch = new MouseTouch();
            touch.position = _pos;
            touch.deltaTime = 0;
            touch.deltaPosition = Vector2.zero;
            touch.phase = TouchPhase.Began;
            touch.buttonID = 0;
            touch.Time = Time.time;
            _lg.AddTouch(touch);
            _onGestureStart(_lg);
        }
        if (!Input.GetMouseButton(0) && _pressed)
        {
            _pressed = false;
            // on finish gesture
            MouseTouch touch = GetDeltaTouch();
            touch.phase = TouchPhase.Ended;
            _lg.AddTouch(touch);
            _onGestureEnd(_lg);
        }
    }

    public Gesture GetGByID(int id)
    {
        foreach (Gesture t in g)
            if (t.ID == id)
                return t;
        return null;
    }

    void FixedTouchUpdate()
    {
        Gesture temp;
        foreach (UnityEngine.Touch t in Input.touches)
        {
            temp = GetGByID(t.fingerId);
            if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
            {
                if (temp != null)
                {
                    temp.AddTouch(GetTouch(t));
                    _onGestureEnd((Gesture)temp);
                    g.Remove(temp);
                }
            }
            else
            {
                if (temp != null)
                    temp.AddTouch(GetTouch(t));
                else if (t.phase == TouchPhase.Began)
                {
                    temp = new Gesture();
                    temp.AddTouch(GetTouch(t));
                    g.Add(temp);
                    _onGestureStart(temp);
                }
            } 
        }
    }

    private void CancelGesture(Gesture G)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.WP8Player)
        {
            Gesture temp = GetGByID(G.ID);
            if (temp != null)
            {
                //print("Stop");
                _onGestureEnd((Gesture)temp);
                g.Remove(temp);
            }
        } else
        {
            _break = true;
        }
    }

    public static void StopGesture(Gesture g)
    {
        GestureController.Instance.CancelGesture(g);
    }

    IEnumerator Loop()
    {
        while (true)
        {
            FUpdate();
            yield return null;
        }
    }

    void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.WP8Player)
            FUpdate = FixedTouchUpdate;
        else
            FUpdate = FixedMouseUpdate;
        StartCoroutine("Loop");
        Input.multiTouchEnabled = true;
    }

}