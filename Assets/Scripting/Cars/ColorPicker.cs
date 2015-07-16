using UnityEngine;
using System.Collections;

public class ColorPicker : MonoBehaviour 
{
    public enum Colors
    {
        Red = 0,
        Blue,
        Yellow,
        Black
    }

    public GameObject CarBody;
    public int MaterialIndex = 0;

    public Texture[] Textures;

    public Colors CarColor;

    public void UpdateColor()
    {
        if (CarBody != null && CarBody.GetComponent<Renderer>() && CarBody.GetComponent<Renderer>().sharedMaterials.Length > MaterialIndex && Textures.Length == 4 && Textures [(int)CarColor] != null)
            CarBody.GetComponent<Renderer>().sharedMaterials [MaterialIndex].SetTexture("_MainTex", Textures [(int)CarColor]); 
    }
}
