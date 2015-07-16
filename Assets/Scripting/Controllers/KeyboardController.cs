using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{
    private bool _up = false;
    private bool _down = false;
    private bool _left = false;
    private bool _right = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !_up)
        {
            _up = true;
            EventController.PostEvent("input.keydown.up", null);
        } else
        if (!Input.GetKey(KeyCode.UpArrow) && _up)
        {
            _up = false;
            EventController.PostEvent("input.keyup.up", null);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !_down)
        {
            _down = true;
            EventController.PostEvent("input.keydown.down", null);
        } else
            if (!Input.GetKey(KeyCode.DownArrow) && _down)
        {
            _down = false;
            EventController.PostEvent("input.keyup.down", null);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !_left)
        {
            _left = true;
            EventController.PostEvent("input.keydown.left", null);
        } else
            if (!Input.GetKey(KeyCode.LeftArrow) && _left)
        {
            _left = false;
            EventController.PostEvent("input.keyup.left", null);
        }

        if (Input.GetKey(KeyCode.RightArrow) && !_right)
        {
            _right = true;
            EventController.PostEvent("input.keydown.right", null);
        } else
            if (!Input.GetKey(KeyCode.RightArrow) && _right)
        {
            _right = false;
            EventController.PostEvent("input.keyup.right", null);
        }
    }
}
