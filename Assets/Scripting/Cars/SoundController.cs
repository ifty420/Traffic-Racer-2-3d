using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarDriver))]
public class SoundController : MonoBehaviour, IEventSubscriber
{
    private CarDriver car;

    public AudioClip D = null;
    public float DVolume = 1.0f;
    public AudioClip E = null;
    public float EVolume  = 1.0f;
    public AudioClip F = null;
    public float FVolume = 1.0f;
    public AudioClip K = null;
    public float KVolume = 1.0f;
    public AudioClip L = null;
    public float LVolume = 1.0f;
    
    public AudioClip wind = null;
    public float windVolume = 1.0f;
    
    public AudioClip crashLowSpeedSound = null;
    public float crashLowVolume = 1.0f;
    public AudioClip crashHighSpeedSound = null;
    public float crashHighVolume = 1.0f;
    public AudioClip skidSound = null;

    public bool PlayBackMusic = true;
    public AudioClip BackgroundMusic = null;
    public float BackgroundMusicVolume = 0.6f;
    
    private AudioSource DAudio = null;
    private AudioSource EAudio = null;
    private AudioSource FAudio = null;
    private AudioSource KAudio = null;
    private AudioSource LAudio = null;

    private AudioSource windAudio = null;
    private AudioSource skidAudio = null;
	public AudioSource carAudio = null;
    
    public AudioSource backgroundMusic = null;
    
    private float carMaxSpeed = 55.0f;
    private float gearShiftTime = 0.1f;
    private bool shiftingGear = false;
    private int gearShiftsStarted = 0;
    private int crashesStarted = 0;
    private float crashTime = 0.2f;
    private int oneShotLimit = 8;
    
    private float idleFadeStartSpeed = 3.0f;
    private float idleFadeStopSpeed = 10.0f;
    private float idleFadeSpeedDiff = 7.0f;
    private float speedFadeStartSpeed = 0.0f;
    private float speedFadeStopSpeed = 8.0f;
    private float speedFadeSpeedDiff = 8.0f;
    
    private bool soundsSet = false;
    
    private float inputFactor = 0f;
    private int gear = 0;
    private int topGear = 6;
    
    private float idlePitch = 0.7f;
    private float startPitch = 0.85f;
    private float lowPitch = 1.17f;
    private float medPitch = 1.25f;
    private float highPitchFirst = 1.65f;
    private float highPitchSecond = 1.76f;
    private float highPitchThird = 1.80f;
    private float highPitchFourth = 1.86f;
    private float shiftPitch = 1.44f;
    
    private float prevPitchFactor = 0f;

    void Awake()
    {
        car = transform.GetComponent<CarDriver>();
        
        DVolume *= 0.4f;
        EVolume *= 0.4f;
        FVolume *= 0.4f;
        KVolume *= 0.7f;
        LVolume *= 0.4f;
        windVolume *= 0.4f;
        
        DAudio = gameObject.AddComponent<AudioSource>();
        DAudio.loop = true;
        DAudio.clip = D;
        DAudio.volume = DVolume;
        DAudio.Play();
        
        EAudio = gameObject.AddComponent<AudioSource>();
        EAudio.loop = true;
        EAudio.clip = E;
        EAudio.volume = EVolume;
        EAudio.Play();
        
        FAudio = gameObject.AddComponent<AudioSource>();
        FAudio.loop = true;
        FAudio.clip = F;
        FAudio.volume = FVolume;
        FAudio.Play();
        
        KAudio = gameObject.AddComponent<AudioSource>();
        KAudio.loop = true;
        KAudio.clip = K;
        KAudio.volume = KVolume;
        KAudio.Play();
        
        LAudio = gameObject.AddComponent<AudioSource>();
        LAudio.loop = true;
        LAudio.clip = L;
        LAudio.volume = LVolume;
        LAudio.Play();
        
        windAudio = gameObject.AddComponent<AudioSource>();
        windAudio.loop = true;
        windAudio.clip = wind;
        windAudio.volume = windVolume;
        windAudio.Play();
        
        skidAudio = gameObject.AddComponent<AudioSource>();
        skidAudio.loop = true;
        skidAudio.clip = skidSound;
        skidAudio.volume = 0.0f;
        skidAudio.Play();
        
        carAudio = gameObject.AddComponent<AudioSource>();
        carAudio.loop = false;
        carAudio.playOnAwake = false;
        carAudio.Stop();
        
        crashTime = Mathf.Max(crashLowSpeedSound.length, crashHighSpeedSound.length);
        soundsSet = false;
        
        idleFadeSpeedDiff = idleFadeStopSpeed - idleFadeStartSpeed;
        speedFadeSpeedDiff = speedFadeStopSpeed - speedFadeStartSpeed;
        
        if (PlayBackMusic)
        {
			backgroundMusic = gameObject.AddComponent<AudioSource>();
            backgroundMusic.loop = true;
            backgroundMusic.clip = BackgroundMusic;
            //  backgroundMusic.maxVolume = BackgroundMusicVolume;
            //  backgroundMusic.minVolume = BackgroundMusicVolume;
            backgroundMusic.Play();
			backgroundMusic.volume = BackgroundMusicVolume;
		}

        EventController.Subscribe("car.player.death", this);
        EventController.Subscribe("gui.screen.pause", this);
        EventController.Subscribe("gui.screen.game.show", this);
    }

    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "gui.screen.game.show":
                DAudio.Play();
                EAudio.Play();
                FAudio.Play();
                KAudio.Play();
                LAudio.Play();
                KAudio.Play();
                windAudio.Play();
                skidAudio.Play();
                carAudio.Play();
                if (PlayBackMusic)
                    backgroundMusic.Play();

                break;

            default:
                if (DAudio.isPlaying)
                {
                    DAudio.Pause();
                    EAudio.Pause();
                    FAudio.Pause();
                    KAudio.Pause();
                    LAudio.Pause();
                    KAudio.Pause();
                    windAudio.Pause();
                    skidAudio.Pause();
                    carAudio.Pause();
                    if (PlayBackMusic)
                        backgroundMusic.Pause();
                }
                break;
        }
    }

    #endregion

    void Update()
    {
        float carSpeed = car.GetComponent<Rigidbody>().velocity.magnitude;
        float carSpeedFactor = Mathf.Clamp01(carSpeed / car.MaxSpeed);
        
        KAudio.volume = Mathf.Lerp(0, KVolume, carSpeedFactor);
        windAudio.volume = Mathf.Lerp(0, windVolume, carSpeedFactor * 2);
        
        if(shiftingGear)
            return;
        
        float pitchFactor = Sinerp(0, topGear, carSpeedFactor);
        int newGear = (int)pitchFactor;
        
        pitchFactor -= newGear;
        float throttleFactor = pitchFactor;
        pitchFactor *= 0.3f;
        pitchFactor += throttleFactor * (0.7f) * Mathf.Clamp01(car.throttle * 2);
        
        if(newGear != gear)
        {
            if(newGear > gear)
                GearShift(prevPitchFactor, pitchFactor, gear, true);
            else
                GearShift(prevPitchFactor, pitchFactor, gear, false);
            gear = newGear;
        }
        else
        {
            float newPitch = 0;
            if(gear == 0)
                newPitch = Mathf.Lerp(idlePitch, highPitchFirst, pitchFactor);
            else
                if(gear == 1)
                    newPitch = Mathf.Lerp(startPitch, highPitchSecond, pitchFactor);
            else
                if(gear == 2)
                    newPitch = Mathf.Lerp(lowPitch, highPitchThird, pitchFactor);
            else
                newPitch = Mathf.Lerp(medPitch, highPitchFourth, pitchFactor);
            SetPitch(newPitch);
            SetVolume(newPitch);
        }
        prevPitchFactor = pitchFactor;
    }

    float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }
    
    float Sinerp(float start,float end,float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }
    
    void SetPitch(float pitch)
    {
        DAudio.pitch = pitch;
        EAudio.pitch = pitch;
        FAudio.pitch = pitch;
        LAudio.pitch = pitch;
    }
    
    void SetVolume(float pitch)
    {
        float pitchFactor = Mathf.Lerp(0, 1, (pitch - startPitch) / (highPitchSecond - startPitch));
        DAudio.volume = Mathf.Lerp(0, DVolume, pitchFactor);
        float fVolume = Mathf.Lerp(FVolume * 0.8f, FVolume, pitchFactor);
        FAudio.volume = fVolume * 0.7f + fVolume * 0.3f * Mathf.Clamp01(car.throttle);
        float eVolume = Mathf.Lerp(EVolume * 0.89f, EVolume, pitchFactor);
        EAudio.volume = eVolume * 0.8f + eVolume * 0.2f * Mathf.Clamp01(car.throttle);
    }
    
    IEnumerator GearShift(float oldPitchFactor,float newPitchFactor,int gear,bool shiftUp)
    {
        shiftingGear = true;
        
        float timer = 0;
        float pitchFactor = 0;
        float newPitch = 0;
        
        if(shiftUp)
        {
            while(timer < gearShiftTime)
            {
                pitchFactor = Mathf.Lerp(oldPitchFactor, 0, timer / gearShiftTime);
                if(gear == 0)
                    newPitch = Mathf.Lerp(lowPitch, highPitchFirst, pitchFactor);
                else
                    newPitch = Mathf.Lerp(lowPitch, highPitchSecond, pitchFactor);
                SetPitch(newPitch);
                SetVolume(newPitch);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while(timer < gearShiftTime)
            {
                pitchFactor = Mathf.Lerp(0, 1, timer / gearShiftTime);
                newPitch = Mathf.Lerp(lowPitch, shiftPitch, pitchFactor);
                SetPitch(newPitch);
                SetVolume(newPitch);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        
        shiftingGear = false;
    }
    
    void Skid(bool play,float volumeFactor)
    {
        if(!skidAudio)
            return;
        if(play)
        {
            skidAudio.volume = Mathf.Clamp01(volumeFactor + 0.3f);
        }
        else
            skidAudio.volume = 0.0f;
    }
    
    // Checks if the max amount of crash sounds has been started, and
    // if the max amount of total one shot sounds has been started.
    public IEnumerator Crash(float volumeFactor)
    {
        if (!(crashesStarted > 3 || OneShotLimitReached()))
        {
            if (volumeFactor > 0.6f)
                carAudio.PlayOneShot(crashHighSpeedSound, Mathf.Clamp01((0.5f + volumeFactor * 0.5f) * crashHighVolume));
            carAudio.PlayOneShot(crashLowSpeedSound, Mathf.Clamp01(volumeFactor * crashLowVolume));
            crashesStarted++;
        
            yield return new WaitForSeconds(crashTime);
        
            crashesStarted--;
        }
    }
    
    // A function for testing if the maximum amount of OneShot AudioClips
    // has been started.
    bool OneShotLimitReached()
    {
        return (crashesStarted + gearShiftsStarted) > oneShotLimit;
    }
}
