using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SoundController))]
[RequireComponent(typeof(CarDriver))]
public class CrashController : MonoBehaviour 
{
    private SoundController sound;

    void Awake()
    {
        sound = GetComponent<SoundController>();
    }
    
    void OnCollisionEnter(Collision collInfo)
    {
		if(enabled && collInfo.contacts.Length > 0 && (!collInfo.gameObject.CompareTag("Ground") && !collInfo.gameObject.CompareTag("Road") && !collInfo.gameObject.CompareTag("OutWorld")))
        {
            float volumeFactor = Mathf.Clamp01(collInfo.relativeVelocity.magnitude * 0.08f);
            volumeFactor *= Mathf.Clamp01(0.3f + Mathf.Abs(Vector3.Dot(collInfo.relativeVelocity.normalized, collInfo.contacts[0].normal)));
            volumeFactor = volumeFactor * 0.5f + 0.5f;
            StartCoroutine(sound.Crash(volumeFactor));
        }
    }
}
