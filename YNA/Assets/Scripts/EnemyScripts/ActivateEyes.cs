//*************************************************
// Project: We're Tethered Together
// File: ActivateEyes.cs
// Author/s: Emmy Berg
//           K Preston
//
// Desc: Controls the Eyes three states
//
// Notes:
//  + Holy shit we need more helper functions lol
//
// Last Edit: 5/3/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EyeStates
{
    SLEEPING,
    WAKING,
    ACTIVE
}

public class ActivateEyes : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    EyeStates status;
    
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
    public float gracePeriod;
    float timeInSight;

    float maxTime;
    float currTime;

    bool playerSpotted = false;

    // Audio variables:
    public AudioSource goodmorning;
    public AudioSource iamawake;
    public AudioSource iamwatching;
    public AudioSource ifoundyou;
    public AudioSource iseeyou;

    public string iamwatchingMGEP;
    public string iseeyouMGEP;
    // *********************************************************************

    private bool started = false;
    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        started = true;

        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

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
        if(!started)
        {
            Start();
        }

        // Closed
        if (currTime >= 0)
        {
            status = EyeStates.SLEEPING;
            Sleep();
        }

        // Halfway
        if (currTime <= 3.5f && currTime > 0)
        {
            status = EyeStates.WAKING;
            BeginWake();
        }

        // Open
        if (currTime <= 0)
        {
            status = EyeStates.ACTIVE;
            Activate();
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
            // If they've just been spotted...
            if(!playerSpotted)
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
        maxTime = Random.Range(min, max);
        currTime = maxTime;
    }


    ////////////////////////////////////////////////////////////////////////
    // SELECT NEW TIME =====================================================
    // Pick a new random sleep time-period
    IEnumerator LookAround()
    {
        yield return new WaitForSeconds(3.5f);

        while(playerSpotted)
        {
            yield return new WaitForSeconds(1.5f);
        }
        SelectNewTime();
    }
}
