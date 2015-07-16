using System;
using UnityEngine;

public class MPoint : MO
{
    Vector3 _point;

    public MPoint(Vector2 Point)
    {
        _point = Point;
    }

    public override Vector2 GetNearestPoint(Vector2 point)
    {
        return _point;
    }

    public override Vector2 GetCenterPoint()
    {
        return _point;
    }

    public override float Dostance()
    {
        return 0;
    }
}

