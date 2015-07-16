using System;
using UnityEngine;

public class MCircle : MO
{
    Vector2 center;
    float radius;

    public MCircle(Vector2 Center,float Radius)
    {
        radius = Radius;
        center = Center;
    }

    public override Vector2 GetNearestPoint(Vector2 point)
    {
        //Debug.Log(string.Format("Center = {0} Radius = {1} Vector.norm = {2}",center,radius,));
        return (point - center).normalized * radius + center;
    }

    public override Vector2 GetCenterPoint()
    {
        return center;
    }

    public override float Dostance()
    {
        return Mathf.PI * radius * 2;
    }
}

