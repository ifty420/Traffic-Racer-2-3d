using UnityEngine;
using System;

    public class MLine : MO
{
    Vector2 start;
    Vector2 end;

    public MLine(Vector2 Start,Vector2 End)
    {
        start = Start;
        end = End;
        if (end.x < start.x)
        {
            Vector2 buf = start;
            start = end;
            end = buf;
        }
        if (end.y < start.y)
        {
            Vector2 buf = start;
            start = end;
            end = buf;
        }
    }

    public override Vector2 GetNearestPoint(Vector2 point)
    {

        Vector2 a = end - start;
        Vector2 b = point - start;
        float len= (Vector2.Dot(a, b)/Vector2.Dot(a,a));
        if (len < 0)
            return start;
        if (len > 1)
            return end;
        return start + a * len;
    }

    public override Vector2 GetCenterPoint()
    {
        return (start + end) / 2;
    }

    public override float Dostance()
    {
        return Vector2.Distance(start, end);
    }
}


