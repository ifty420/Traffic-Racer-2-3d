using UnityEngine;
using System.Collections.Generic;

public class MagnetObjects : MonoBehaviour {

    List<MO> k;

    public MagnetObjects()
    {
        k = new List<MO>();
    }

    public void AddLine(Vector2 StartPoint,Vector2 EndPoint)
    {
        k.Add(new MLine(StartPoint, EndPoint));
    }

    public void AddCircle(Vector2 Center,float Radius)
    {
        k.Add(new MCircle(Center, Radius));
    }

    public void AddPoint(Vector2 point)
    {
        k.Add(new MPoint(point));
    }

    public void AddRectangle(Vector2 p1,Vector2 p2)
    {
        float ltx = Mathf.Min(p1.x, p2.x);
        float rbx = Mathf.Max(p1.x, p2.x);
        float lty = Mathf.Min(p1.y, p2.y);
        float rby = Mathf.Max(p1.y, p2.y);
        k.Add(new MLine(new Vector2(ltx,lty),new Vector2(rbx,lty)));
        k.Add(new MLine(new Vector2(ltx,rby),new Vector2(rbx,rby)));
        k.Add(new MLine(new Vector2(ltx,lty),new Vector2(ltx,rby)));
        k.Add(new MLine(new Vector2(rbx,lty),new Vector2(rbx,rby)));
    }

    public Vector2 GetMagnetPoint(Vector2 point)
    {
        if (k.Count == 0)
            return point;
        Vector2 res = k[0].GetNearestPoint(point);
        for (int i = 1; i < k.Count; i++)
            if (Vector2.Distance(res, point) > Vector2.Distance(k[i].GetNearestPoint(point), point))
                res = k[i].GetNearestPoint(point);
        return res;
    }

    public Vector2 GetCenterPoint()
    {
        if (k.Count == 0)
            return Vector2.zero;
        Vector2 res = Vector2.zero;
        for (int i = 0; i < k.Count; i++)
        {
            //print(k[i].GetCenterPoint());
            res += k[i].GetCenterPoint();
        }
        return res / k.Count;
    }

    public float Distance
    {
        get 
        {
            float res = 0;
            foreach (MO m in k)
                res += m.Dostance();
            return res;
        }
    }

    public void Clear()
    {
        k.Clear();
    }
}
