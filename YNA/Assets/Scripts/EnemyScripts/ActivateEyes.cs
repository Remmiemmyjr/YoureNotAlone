//*************************************************
// Project: We're Tethered Together
// File: ActivateEyes.cs
// Author/s: Emmy Berg
//           K Preston
//           Corbyn LaMar
//
// Desc: Controls the Eyes states
//
// Notes:
// -
//
// Last Edit: 8/15/2023
//
//*************************************************

using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum EyeStates
{
    SLEEPING,
    WAKING,
    ACTIVE,
    SEEN
}

public class ActivateEyes : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    [HideInInspector]
    public EyeStates status;
    [HideInInspector]
    public EyeStates prevStatus;
    
    GameObject Player;
    GameObject Partner;
    EyeProfile profile;

    Volume globalVolume;
    VolumeProfile volProf;
    Bloom bloom;

    // Eyes are to be assigned through the children
    private GameObject[] Eyes;

    //public Sprite closed;
    //public Sprite halfway;
    //public Sprite open;

    public bool timeToHide = false;

    public float min = 5f;
    public float max = 10f;
    public float minSeek = 4f;
    public float maxSeek = 6.5f;
    public float wakingTime = 3.5f;
    public float gracePeriod;

    [HideInInspector]
    public float currTime;
    [HideInInspector]
    public float seekTime;
    [HideInInspector]
    public float timeInSight;
    float maxTime;

    bool playerSpotted = false;

    // Audio variables:
    public AudioSource goodmorning;
    public AudioSource iamawake;
    public AudioSource iamwatching;
    public AudioSource ifoundyou;
    public AudioSource iseeyou;

    public AudioSource iamhere;

    [SerializeField]
    private AudioClip[] eyembiance;

    private PersistantMusic musicController;

    public string iamwatchingMGEP;
    public string iseeyouMGEP;

    [SerializeField]
    private UnityEvent wakeEvent;
    [SerializeField]
    private UnityEvent sleepEvent;
    [SerializeField]
    private UnityEvent activateEvent;
    [SerializeField]
    private UnityEvent spottedEvent;
    [SerializeField]
    private UnityEvent rehiddenEvent;

    CinemachineImpulseSource impulse;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        musicController = GameObject.FindWithTag("MusicController").GetComponent<PersistantMusic>();

        globalVolume = gameObject.GetComponent<PostProcessingManager>().GetGlobalVolume();
        volProf = globalVolume.profile;
        volProf.TryGet<Bloom>(out bloom);

        impulse = GetComponent<CinemachineImpulseSource>();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        // Get all of the eyes from the children of this Gameobject
        Eyes = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Eyes[i] = transform.GetChild(i).gameObject;
        }


        // Eye am here >:)
        iamhere.PlayOneShot(eyembiance[Random.Range(0, eyembiance.Length)]);

        SelectNewTime();

        for (int i = 0; i < Eyes.Length; i++)
        {
            profile = Eyes[i].GetComponent<EyeProfile>();
            profile.SetStatusSleeping();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        // Closed
        if (currTime >= 0 && !playerSpotted)
        {
            Sleep();
            bloom.intensity.value = SettingsControl.myBloomVal;
            status = EyeStates.SLEEPING;
        }

        // Halfway
        if (currTime <= wakingTime && currTime > 0)
        {
            BeginWake();
            prevStatus = status;
            status = EyeStates.WAKING;
        }

        // Open
        if (currTime <= 0 && status != EyeStates.ACTIVE && status != EyeStates.SEEN)
        {
            Activate();
            prevStatus = status;
            status = EyeStates.ACTIVE;
        }

        // KillCheck if the eyes are open.
        if (timeToHide)
        {
            playerSpotted = KillCheck();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // SLEEP ===============================================================
    public void Sleep()
    {
        // If the eyes just closed...
        if (timeToHide)
        {
            // Stop sounds.
            iamwatching.Stop();
            iseeyou.Stop();

            sleepEvent.Invoke();

            // Restart level music.
            StartCoroutine(musicController.LerpAudioIn(0.1f));

            for (int i = 0; i < Eyes.Length; i++)
            {
                profile = Eyes[i].GetComponent<EyeProfile>();
                profile.SetStatusSleeping();
            }
        }

        timeToHide = false;

        currTime -= Time.deltaTime;

        Info.jumpOutCounter = 0;
    }


    ////////////////////////////////////////////////////////////////////////
    // BEGIN WAKE ==========================================================
    void BeginWake()
    {
        // If we just entered this state...
        if (currTime > 3.4f)
        {
            // Stop the level music.
            StartCoroutine(musicController.LerpAudioOut(wakingTime));

            // Set the iamwatching mixer group level to 1.
            AudioMixer mg = iamwatching.outputAudioMixerGroup.audioMixer;
            mg.SetFloat(iamwatchingMGEP, -6.0f);

            // Play goodmorning
            goodmorning.Play();
            wakeEvent.Invoke();

            for (int i = 0; i < Eyes.Length; i++)
            {
                profile = Eyes[i].GetComponent<EyeProfile>();
                profile.SetStatusWaking();
            }
        }

        // Set the iamwatching mixer group level to 1.
        AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
        iamwatchingMG.SetFloat(iamwatchingMGEP, -6.0f);

        // Set the iseeyou mixer group level to 0.
        AudioMixer iseeyouMG = iseeyou.outputAudioMixerGroup.audioMixer;
        iseeyouMG.SetFloat(iseeyouMGEP, -80.0f);
    }


    ////////////////////////////////////////////////////////////////////////
    // ACTIVATE ============================================================
    void Activate()
    {
        // If the eyes just opened...
        if (!timeToHide)
        {
            // Pulse
            CameraShake.manager.Shake(impulse, 0.25f);

            // Reset timeInSight timer to the gracePeriod.
            timeInSight = gracePeriod;

            // Play iamawake.
            iamawake.Play();
            activateEvent.Invoke();

            // Play both sounds.
            iamwatching.Play();
            iseeyou.Play();

            // Set the iamwatching mixer group level to 1.
            AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
            iamwatchingMG.SetFloat(iamwatchingMGEP, -6.0f);

            // Set the iseeyou mixer group level to 0.
            AudioMixer iseeyouMG = iseeyou.outputAudioMixerGroup.audioMixer;
            iseeyouMG.SetFloat(iseeyouMGEP, -80.0f);
        }

        timeToHide = true;

        for (int i = 0; i < Eyes.Length; i++)
        {
            profile = Eyes[i].GetComponent<EyeProfile>();
            profile.SetStatusActive();
        }

        prevStatus = status;
    }


    ////////////////////////////////////////////////////////////////////////
    // KILL CHECK ==========================================================
    // Returns true if the player or partner is visible, and acts accordingly.
    bool KillCheck()
    {
        // Temp variable
        bool playerHidden = Player.GetComponent<Hide>().isHidden;
        bool partnerHidden = true;

        if (Partner)
            partnerHidden = Partner.GetComponent<Hide>().isHidden;

        // If the player or partner are visible...
        if (!playerHidden || !partnerHidden)
        {
            status = EyeStates.SEEN;

            // Camera shake on seen
            CameraShake.manager.Shake(impulse, 0.15f);

            // If they've just been spotted...
            if (!playerSpotted)
            {
                // If while hiding, the player jumps out 3 times, kill them)
                Info.jumpOutCounter++;
                if(Info.jumpOutCounter == 3)
                {
                    Info.isDead = true;
                    Info.eyeDeath = true;
                }

                // Play ifoundyou
                ifoundyou.Play();

                spottedEvent.Invoke();

                // Set the iamwatching mixer group level to 0.
                AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
                iamwatchingMG.SetFloat(iamwatchingMGEP, -80.0f);

                // Set the iseeyou mixer group level to 1.
                AudioMixer iseeyouMG = iseeyou.outputAudioMixerGroup.audioMixer;
                iseeyouMG.SetFloat(iseeyouMGEP, -6.0f);
            }

            // If timeInSight has depleted the gracePeriod, kill the player.
            if(timeInSight <= 0)
            {
                Info.isDead = true;
                Info.eyeDeath = true;
            }
            else
            {
                timeInSight -= Time.deltaTime;
            }

            // Have each eye look in the direction of who is visible
            for (int i = 0; i < Eyes.Length; i++)
            {
                Animator eyeAnim = Eyes[i].GetComponent<Animator>();

                // Look at the player if they are visible
                if (playerHidden == false)
                {
                    LookAtTarget(Player, Eyes[i], eyeAnim);
                }
                // Only the partner must be visible, so look at them
                else
                {
                    LookAtTarget(Partner, Eyes[i], eyeAnim);
                }
            }
            return true;
        }

        else
        {
            // If the player was previously visible...
            if(playerSpotted && !Info.isDead)
            {
                status = EyeStates.ACTIVE;

                // Fade out iseeyou
                StartCoroutine(FadeMixerGroup.StartFade(iseeyou.outputAudioMixerGroup.audioMixer, iseeyouMGEP, 0.25f, 0.0f));
                // Fade in iamwatching
                StartCoroutine(FadeMixerGroup.StartFade(iamwatching.outputAudioMixerGroup.audioMixer, iamwatchingMGEP, 0.25f, 1.0f));

                // Go back to active fx
                rehiddenEvent.Invoke();

                // Have each eye resume looking around
                for (int i = 0; i < Eyes.Length; i++)
                {
                    Animator eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.SetTrigger("Open");
                    eyeAnim.SetFloat("CycleOffset", Random.Range(0.0f, 1.0f));
                    // eyeAnim.Play("Eyeball_1_Open", -1, Random.Range(0.0f, 1.0f));
                }

                // Reset grace period
                timeInSight = gracePeriod;
            }

            // Only reset while player is not visible
            if (seekTime <= 0)
            {
                SelectNewTime();
            }
        }

        seekTime -= Time.deltaTime;

        return false;
    }


    ////////////////////////////////////////////////////////////////////////
    // SELECT NEW TIME =====================================================
    // Pick a new random sleep time-period
    void SelectNewTime()
    {
        seekTime = Random.Range(minSeek, maxSeek);
        maxTime = Random.Range(min, max);
        currTime = maxTime;
    }


    ////////////////////////////////////////////////////////////////////////
    // LOOK AT TARGET ======================================================
    // Make the eye's look at the highest prioritized visible target
    void LookAtTarget(GameObject target, GameObject eye, Animator anim)
    {
        // Left
        if (target.transform.position.x <= eye.transform.position.x)
        {
            // Top
            if (target.transform.position.y >= eye.transform.position.y)
            {
                // anim.Play("Eyeball_1_TopL");
                anim.SetTrigger("TopL");
            }
            else // Bottom
            {
                // anim.Play("Eyeball_1_BotL");
                anim.SetTrigger("BotL");
            }
        }
        else // Right
        {
            // Top
            if (target.transform.position.y >= eye.transform.position.y)
            {
                // anim.Play("Eyeball_1_TopR");
                anim.SetTrigger("TopR");
            }
            else // Bottom
            {
                // anim.Play("Eyeball_1_BotR");
                anim.SetTrigger("BotR");
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PAUSE EYE AUDIO =====================================================
    public void PauseEyeAudio(bool pause)
    {
        if (pause) // Paused
        {
            switch(status)
            {
                case EyeStates.SLEEPING:
                    iamhere.Pause();
                    break;
                case EyeStates.WAKING:
                    goodmorning.Pause();
                    iamhere.Pause();
                    break;
                case EyeStates.ACTIVE:
                    iamawake.Pause();
                    iamwatching.Pause();
                    iamhere.Pause();
                    iamhere.Pause();
                    break;
                case EyeStates.SEEN:
                    ifoundyou.Pause();
                    iseeyou.Pause();
                    iamhere.Pause();
                    break;
            }
        }

        else // Unpaused
        {
            switch (status)
            {
                case EyeStates.SLEEPING:
                    iamhere.UnPause();
                    break;
                case EyeStates.WAKING:
                    iamhere.UnPause();
                    goodmorning.UnPause();
                    break;
                case EyeStates.ACTIVE:
                    iamhere.UnPause();
                    iamawake.UnPause();
                    iamwatching.UnPause();
                    break;
                case EyeStates.SEEN:
                    iamhere.UnPause();
                    ifoundyou.UnPause();
                    iseeyou.UnPause();
                    break;
            }
        }
    }
}
