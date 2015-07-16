using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundParallax : MonoBehaviour 
{
    public Camera MainCamera;
    public float RelativeHeight = 1;

    [Range(0,1)]
    public float ParalaxShift = 0;

    public bool Forward = true;

    private float _camW = 0;
    private float _spriteW = 0;

    public float MinCamX = 0;
    public float MaxCamX = 100;

    void Update()
    {
        float camX = MainCamera.transform.position.x;
        ParalaxShift = (camX - MinCamX) / (MaxCamX - MinCamX);
        if (!Forward)
            ParalaxShift = 1 - ParalaxShift;

        float a = Mathf.Abs(_spriteW - _camW);
        transform.localPosition = new Vector3(-(a/2) + a*ParalaxShift, 0, 
                                              -MainCamera.transform.position.z); 
    }

	void Start () 
    {
        float camH = MainCamera.orthographicSize;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        transform.localScale = new Vector3(1,1,1) * ((camH * 2) * RelativeHeight / sprite.bounds.size.y);

        _camW = (MainCamera.orthographicSize) * (MainCamera.pixelWidth/MainCamera.pixelHeight);
        _spriteW = sprite.bounds.size.x;

        transform.parent = MainCamera.transform;
	}
}
