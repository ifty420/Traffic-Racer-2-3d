using UnityEngine;
using System.Collections;

public class GUIClickable : GUIObject 
{
    private int _pressedGID = -1;

    public virtual void OnPress(Gesture G) 
    { 
        _pressedGID = G.ID;
        GestureController.OnGestureEnd += OnGestureEnd;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GestureController.OnGestureEnd -= OnGestureEnd;
    }

    void OnGestureEnd(Gesture g)
    {
        if (g.ID == _pressedGID)
        {
            OnRelease(g);
        }
    }

    public virtual void OnRelease(Gesture G) 
    {
        if (_pressedGID == -1)
            return;
        _pressedGID = -1;
        GestureController.OnGestureEnd -= OnGestureEnd;
    }
}
