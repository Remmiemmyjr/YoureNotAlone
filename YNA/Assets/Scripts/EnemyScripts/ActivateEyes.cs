//*************************************************
// Project: We're Tethered Together
// File: ActivateEyes.cs
// Author/s: Emmy Berg
//           K Preston
//
// Desc: Controls the Eyes states
//
// Last Edit: 5/11/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

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

    public GameObject[] Eyes;
    public Sprite closed;
    public Sprite halfway;
    public Sprite open;

    public bool timeToHide = false;

    public float min = 5f;
    public float max = 10f;
    public float minSeek = 4f;
    public float maxSeek = 6.5f;
    public float wakingTime = 3.5f;
    public float gracePeriod;
    
    float timeInSight;
    float maxTime;
    float currTime;
    float seekTime;

    bool playerSpotted = false;

    // Audio variables:
    public AudioSource goodmorning;
    public AudioSource iamawake;
    public AudioSource iamwatching;
    public AudioSource ifoundyou;
    public AudioSource iseeyou;

    public string iamwatchingMGEP;
    public string iseeyouMGEP;

    [SerializeField]
    private UnityEvent wakeEvent;
    [SerializeField]
    private UnityEvent sleepEvent;
    //[SerializeField]
    //private UnityEvent<CameraFX> sleepEvent;
    // *********************************************************************





    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
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
        if (currTime >= 0)
        {
            Sleep();
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
        if (currTime <= 0)
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
    void Sleep()
    {
        // If the eyes just closed...
        if (timeToHide)
        {
            // Stop sounds.
            iamwatching.Stop();
            iseeyou.Stop();

            sleepEvent.Invoke();



            // Restart level music.
            GameObject.Find("MusicController").GetComponent<AudioSource>().Play();
        }

        timeToHide = false;

        for (int i = 0; i < Eyes.Length; i++)
        {
            profile = Eyes[i].GetComponent<EyeProfile>();
            profile.SetStatusSleeping();
        }

        currTime -= Time.deltaTime;
    }


    ////////////////////////////////////////////////////////////////////////
    // BEGIN WAKE ==========================================================
    void BeginWake()
    {
        // If we just entered this state...
        if (currTime > 3.4f)
        {
            // Set the iamwatching mixer group level to 1.
            AudioMixer mg = iamwatching.outputAudioMixerGroup.audioMixer;
            mg.SetFloat(iamwatchingMGEP, 0.0f);

            // Play goodmorning
            goodmorning.Play();


        }

        for (int i = 0; i < Eyes.Length; i++)
        {
            profile = Eyes[i].GetComponent<EyeProfile>();
            profile.SetStatusWaking();
        }
        // Set the iamwatching mixer group level to 1.
        AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
        iamwatchingMG.SetFloat(iamwatchingMGEP, 0.0f);
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
            // Reset timeInSight timer to the gracePeriod.
            timeInSight = gracePeriod;

            // Play iamawake.
            iamawake.Play();
            wakeEvent.Invoke();


            // Play both sounds.
            iamwatching.Play();
            iseeyou.Play();
            // Stop the level music.
            GameObject.Find("MusicController").GetComponent<AudioSource>().Stop();

            // Set the iamwatching mixer group level to 1.
            AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
            iamwatchingMG.SetFloat(iamwatchingMGEP, 0.0f);
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
        StartCoroutine(LookAround());
    }


    ////////////////////////////////////////////////////////////////////////
    // KILL CHECK ==========================================================
    // Returns true if the player or partner is visible.
    bool KillCheck()
    {
        // If the player or partner are visible...
        if (Player.GetComponent<Stats>().isHidden == false || Partner.GetComponent<Stats>().isHidden == false)
        {
            status = EyeStates.SEEN;
            // If they've just been spotted...
            if (!playerSpotted)
            {
                // Play ifoundyou
                ifoundyou.Play();

                // Set the iamwatching mixer group level to 0.
                AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
                iamwatchingMG.SetFloat(iamwatchingMGEP, -80.0f);
                // Set the iseeyou mixer group level to 1.
                AudioMixer iseeyouMG = iseeyou.outputAudioMixerGroup.audioMixer;
                iseeyouMG.SetFloat(iseeyouMGEP, 0.0f);
            }

            // If timeInSight has depleted the gracePeriod, kill the player.
            if(timeInSight <= 0)
            {
                Debug.Log("YOURE DEAD");
                Player.GetComponent<Stats>().isDead = true;
                Partner.GetComponent<Stats>().isDead = true;
            }
            else
            {
                timeInSight -= Time.deltaTime;
            }

            return true;
        }
        else
        {
            // If the player was previously visible...
            if(playerSpotted)
            {
                // Fade out iseeyou
                StartCoroutine(FadeMixerGroup.StartFade(iseeyou.outputAudioMixerGroup.audioMixer, iseeyouMGEP, 0.5f, 0.0f));
                // Fade in iamwatching
                StartCoroutine(FadeMixerGroup.StartFade(iamwatching.outputAudioMixerGroup.audioMixer, iamwatchingMGEP, 0.5f, 1.0f));
            }
        }

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
    // LOOK AROUND =========================================================
    // Loop for specified time duration
    IEnumerator LookAround()
    {
        while (playerSpotted)
        {
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(seekTime);

        SelectNewTime();
    }
}
