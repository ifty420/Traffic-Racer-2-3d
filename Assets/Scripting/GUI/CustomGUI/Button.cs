using UnityEngine;
using System.Collections;

public class Button : GUIClickable
{
    public bool ChangeTexture = true;
    public Texture2D MainTexture = null;
    public Texture2D ActiveTexture = null;

    public AudioClip PressSound = null;
    public AudioClip ReleaseSound = null;

    public string[] CallWhenPress;
    public string[] CallWhenRelease;

    protected override void AwakeProc()
    {
        base.AwakeProc();
        if (ChangeTexture)
            GetComponent<Renderer>().material.SetTexture("_MainTex", MainTexture);
    }

    public override void OnPress(Gesture G)
    {
        base.OnPress(G);

        if (ChangeTexture)
            GetComponent<Renderer>().material.SetTexture("_MainTex", ActiveTexture);
        if (PressSound != null)
            PlaySound(PressSound);
        foreach (string e in CallWhenPress)
            EventController.PostEvent(e,null);
    }

    public override void OnRelease(Gesture G)
    {
        base.OnRelease(G);

        if (ChangeTexture)
            GetComponent<Renderer>().material.SetTexture("_MainTex", MainTexture);
        if (ReleaseSound != null)
            PlaySound(ReleaseSound);
        foreach (string e in CallWhenRelease)
            EventController.PostEvent(e,null);
    }

    protected void PlaySound(AudioClip Clip)
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.clip = Clip;
        s.Play();
        Destroy(s, Clip.length);
    }
}
