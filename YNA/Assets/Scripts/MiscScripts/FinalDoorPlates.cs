// Project: We're Tethered Together
// File: FinalDoorPlates.cs
// Author/s: Corbyn LaMar
//
// Desc: The final exit door
//
// Notes:
//  - 
//
// Last Edit: 7/2/2023
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class FinalDoorPlates : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================

    [SerializeField]
    private bool isPlayerPlate;

    [SerializeField]
    private GameObject doorLightFade;
    [SerializeField]
    private Light2D doorBeamLight;

    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private GameObject gem;
    [SerializeField]
    private GameObject gemLight;

    [SerializeField]
    private GameObject partnerGemLight;

    Color ogColor;
    SpriteRenderer gemSR;

    Animator doorAnimator;

    [SerializeField]
    private AudioClip audioDoorOpened;
    private AudioSource doorAS;

    [SerializeField]
    public UnityEvent FadeToBlack;

    // The list of colliders currently inside the first trigger
    List<Collider2D> TriggerList = new List<Collider2D>();

    static bool dontRepeat;


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        dontRepeat = false;

        doorAS = doorLightFade.GetComponent<AudioSource>();
        doorAnimator = doorLightFade.GetComponent<Animator>();

        gemSR = gem.GetComponent<SpriteRenderer>();
        ogColor = gemSR.color;

        gemLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = activeColor;
        gemLight.SetActive(false);
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER STAY ========================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the object is not already in the list
        if (!TriggerList.Contains(collision))
        {
            // Add the object to the list
            TriggerList.Add(collision);

            if (TriggerList.Count == 1)
            {
                gemSR.color = activeColor;
                gemLight.SetActive(true);

                if (!dontRepeat && isPlayerPlate)
                {
                    dontRepeat = true;
                    StartCoroutine(FinalTransitionSequence());
                }
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRIGGER EXIT ========================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the object is in the list
        if (TriggerList.Contains(collision))
        {
            // Remove it from the list
            TriggerList.Remove(collision);

            if (TriggerList.Count == 0)
            {
                gemSR.color = ogColor;
                gemLight.SetActive(false);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////
    // TRANSITION ==========================================================
    private IEnumerator FinalTransitionSequence()
    {
        // Play pitched down door Audio
        doorAS.PlayOneShot(audioDoorOpened);

        // Make sure door transition exists
        if (doorAnimator)
        {
            // Play the trigger for the animation and wait to go to next scene
            doorAnimator.SetTrigger("PlateReached");

            //doorBeamLight.intensity = 0.0f;
            gemLight.SetActive(false);
            partnerGemLight.SetActive(false);

            yield return new WaitForSeconds(3.0f);

            FadeToBlack.Invoke();
            GetComponent<SteamForceAwardAchievement>().AwardAchievement();

            yield return new WaitForSeconds(1.0f);
        }

        // Load credits
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
