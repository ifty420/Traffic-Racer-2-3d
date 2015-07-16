using UnityEngine;
using System.Collections;

public class FPS_Viewer : MonoBehaviour 
{
    public void OnGUI()
    {
        GUILayout.Label(((int)(1f / Time.deltaTime)).ToString());
    }
}
