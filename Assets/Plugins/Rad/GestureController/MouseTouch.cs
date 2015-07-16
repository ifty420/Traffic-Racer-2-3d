// ------------------------------------------------------------------------------
// Mouse touch class for gesture controller
// author: Radomir Slaboshpitsky 
// mail: radiys92@gmail.com
// ------------------------------------------------------------------------------

using UnityEngine;

public class MouseTouch
{
	public Vector2 position { get; set; }
	public Vector2 deltaPosition { get; set; }
	public TouchPhase phase { get; set; }
	public float Time { get; set; }
	public float deltaTime { get; set; }
	public int buttonID { get; set; }
}