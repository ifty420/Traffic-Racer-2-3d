using System;
using UnityEngine;

public abstract class MO
{
    public abstract float Dostance();
    public abstract Vector2 GetNearestPoint(Vector2 point);
    public abstract Vector2 GetCenterPoint();
}
