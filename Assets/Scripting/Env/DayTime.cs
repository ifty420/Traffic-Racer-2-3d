using UnityEngine;
using System.Collections;

public class DayTime : MonoBehaviour 
{
    [Range(0,23)]
    public int Hours = 10;
    [Range(0,59)]
    public float Minutes = 0;

    public float MinutesPerSecond = 10;

    public override string ToString()
    {
        return string.Format("{0} : {1}",Hours,(int)Minutes);
    }

    private void UpdateRotation()
    {
        float x = (Hours / 24.0f) * 360 - 90;
        x += (Minutes / 60.0f) * ((1.0f / 24.0f) * 360);
        transform.rotation = Quaternion.Euler(new Vector3(x, 90, 90));
#if UNITY_EDITOR
        if (Application.isPlaying)
            EventController.PostEvent("update.gui.time", gameObject);
#else
        EventController.PostEvent("update.gui.time", gameObject);
#endif
    }

    void Start()
    {
        EventController.PostEvent("update.gui.time", gameObject);
    }

    void OnValidate()
    {
        UpdateRotation();
    }

    void Update()
    {
        Minutes += MinutesPerSecond * Time.deltaTime;
        if (Minutes >= 60)
        {
            Minutes-=60;
            Hours ++;
        }
        if (Hours > 23)
            Hours = 0;
        UpdateRotation();
    }
}
