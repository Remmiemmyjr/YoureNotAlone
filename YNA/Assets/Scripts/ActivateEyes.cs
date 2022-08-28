using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ActivateEyes : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    public GameObject[] Eyes;
    public Sprite closed;
    public Sprite halfway;
    public Sprite open;

    Animator eyeAnim;
    SpriteRenderer eyeRenderer;


    bool timeToHide = false;
    [HideInInspector]
    public bool canActivate = true;

    public float min = 5f;
    public float max = 10f;

    // The amount of time the player can be seen by the eyes before dying.
    public float gracePeriod;

    float maxTime;
    float currTime;

    bool playerSpotted = false;

    // The amount of time the player has been in sight of the eyes.
    // This currently resets each time the eyes reopen.
    float timeInSight;

    // Audio variables:
    public AudioSource iamawake;
    public AudioSource iamwatching;
    public AudioSource ifoundyou;
    public AudioSource iseeyou;

    public string iamwatchingMGEP;
    public string iseeyouMGEP;

    void Start()
    {
        for(int i = 0; i < Eyes.Length; i++)
        {
            eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
            eyeRenderer.sprite = closed;

            eyeAnim = Eyes[i].GetComponent<Animator>();
            eyeAnim.Play("EyeballClosed");
            eyeAnim.SetBool("isOpen", timeToHide);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        SelectNewTime();

    }

    void Update()
    {
        if (canActivate)
        {
            // Closed
            if (currTime >= 0)
            {
                // If the eyes just closed...
                if(timeToHide)
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
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = closed;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.SetBool("isOpen", timeToHide);
                    eyeAnim.Play("EyeballClosed");
                }

                currTime -= Time.deltaTime;
            }

            // Halfway
            if (currTime <= 3.5f && currTime > 0)
            {
                for (int i = 0; i < Eyes.Length; i++)
                {
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = halfway;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.Play("EyeballHalfway");
                }

                // Set the iamwatching mixer group level to 1.
                AudioMixer iamwatchingMG = iamwatching.outputAudioMixerGroup.audioMixer;
                iamwatchingMG.SetFloat(iamwatchingMGEP, 0.0f);
                // Set the iseeyou mixer group level to 0.
                AudioMixer iseeyouMG = iseeyou.outputAudioMixerGroup.audioMixer;
                iseeyouMG.SetFloat(iseeyouMGEP, -80.0f);
            }

            // Open
            if (currTime <= 0)
            {
                // If the eyes just opened...
                if(!timeToHide)
                {
                    // Reset timeInSight timer to the gracePeriod.
                    timeInSight = gracePeriod;

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
                    eyeRenderer = Eyes[i].GetComponent<SpriteRenderer>();
                    eyeRenderer.sprite = open;

                    eyeAnim = Eyes[i].GetComponent<Animator>();
                    eyeAnim.SetBool("isOpen", timeToHide);
                    eyeAnim.Play("EyeballOpen");
                }

                StartCoroutine(LookAround());
            }

            // KillCheck if the eyes are open.
            if (timeToHide)
            {
                playerSpotted = KillCheck();
            }

        }

        else 
        {
            currTime = maxTime;
        }
    }

    // Returns true if the player or partner are visible.
    bool KillCheck()
    {
        // If the player or partner are visible...
        if (Player.GetComponent<Stats>().isHidden == false || Partner.GetComponent<Stats>().isHidden == false)
        {
            // If they've just been spotted...
            if(!playerSpotted)
            {
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

    void SelectNewTime()
    {
        maxTime = Random.Range(min, max);
        currTime = maxTime;
        Debug.Log("new max time: " + maxTime);
    }

    IEnumerator LookAround()
    {
        yield return new WaitForSeconds(3.5f);
        SelectNewTime();
    }
}
