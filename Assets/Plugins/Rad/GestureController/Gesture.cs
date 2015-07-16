// ------------------------------------------------------------------------------
// Gesture class for gesture controller
// author: Radomir Slaboshpitsky 
// mail: radiys92@gmail.com
// ------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class Gesture
{
	public delegate void func(Gesture g);
	
	public func OnGestureStay;
	
	private void onaddingtouch(Gesture g){}
	
	public List<MouseTouch> Frames { get; set; }
	
	public int FramesCount { get { return Frames.Count; } }
	
	public int ID { get { return Frames.Count > 0 ? Frames[0].buttonID : -1; } }
	
	public Vector2 StartPoint { get { return Frames.Count > 0 ? Frames[0].position : Vector2.zero; } }
	public Vector2 EndPoint { get { return Frames.Count > 0 ? Frames[Frames.Count-1].position : Vector2.zero; } }
	public Vector2 CenterPoint
	{
		get
		{
			Vector2 res = Frames[0].position;
			for (int i = 1; i < Frames.Count; i++)
				res += Frames[i].position;
			res /= Frames.Count;
			return res;
		}
	}
	
	private float Angle(Vector2 point)
	{
		/*
         *      270
         *       |
         *   0 --|-- 180
         *       |
         *       90
         */
		return Mathf.Atan2(point.y, point.x) * (180 / Mathf.PI) + 180;
	}
	
	public float FirsLastAngle()
	{
		return Angle(EndPoint - StartPoint);
	}
	
	public int TurnsCount(float minAngle,int countOfFrames)
	{
		if (Frames.Count == 0)
			return 0;
		int res = 0;
		
		//float delta = Angle(Frames[0].deltaPosition);
		//Debug.Log(Angle(Frames[1].deltaPosition, Frames[0].deltaPosition));
		
		for (int i = 1; i < Frames.Count-countOfFrames-1; i++)
		{
			for (int j = 0; j < countOfFrames; j++)
			{
				if (//Frames[i].deltaPosition != Frames[i + j].deltaPosition &&
				    Frames[i].phase == TouchPhase.Moved &&
				    Frames[i + j].phase == TouchPhase.Moved)
				{
					if (Vector2.Angle(Frames[i].deltaPosition, Frames[i + j].deltaPosition) > minAngle)
					{
						res++;
						i += countOfFrames;
						break;
					}
				}
			}
		}
		
		return res;
	}
	
	public string Code
	{
		get
		{
			string res = "";
			foreach (MouseTouch t in Frames)
				if (t.deltaPosition.sqrMagnitude != 0)
			{
				float dat = Angle(t.deltaPosition) + 210;
				if (dat > 180)
					dat -= 180;
				dat /= 60;
				res += (int)dat;
			}
			return res;
		}
	}
	
	public Gesture()
	{
		Frames = new List<MouseTouch>();
		OnGestureStay = onaddingtouch;
	}
	
	public void AddTouch(MouseTouch touch)
	{
		Frames.Add(touch);
		OnGestureStay(this);
	}
	
	public float GestureTime 
	{
		get
		{
			float d = 0;
			foreach (MouseTouch t in Frames)
				d += t.deltaTime;
			return d;
		}
	}
	
	public float Distance
	{
		get
		{
			float l = 0;
			foreach (MouseTouch t in Frames)
				l += t.deltaPosition.magnitude;
			return l;
		}
	}
	
	// can throw 'OutOfRangeException'....
	public MouseTouch FirstTouch { get { return Frames[0]; } }
	public MouseTouch LastTouch { get { return Frames[Frames.Count - 1]; } }
}
