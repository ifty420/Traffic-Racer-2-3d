/*
 * GUIController v 0.0
 * 
 * Put this to GO with GUI camera
 * 
 * Require EventController and GestureController from Rad plugins to work.
 * 
 * Developed by Radomir Slaboshpitsky
 * mail: radiys92@gmail.com
 * VallVerk dev. studio, 2014
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class GUIController : MonoBehaviour
{
    void Awake()
    {
        GestureController.OnGestureStart += OnGestureStart;
        GestureController.OnGestureEnd += OnGestureEnd;
    }

    void OnDestroy()
    {
        GestureController.OnGestureStart -= OnGestureStart;
        GestureController.OnGestureEnd -= OnGestureEnd;
    }

    void OnGestureStart(Gesture g)
    {
        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(g.StartPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject go = hit.collider.gameObject;
            foreach (var com in go.GetComponents<GUIClickable>())
                com.OnPress(g);
        }
    }

    void OnGestureEnd(Gesture g)
    {
        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(g.EndPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject go = hit.collider.gameObject;
            foreach (var com in go.GetComponents<GUIClickable>())
                com.OnRelease(g);
        }
    }
}
