//*************************************************
// Project: We're Tethered Together
// File: Stats.cs
// Author/s: Emmy Berg
//           Corbyn Lamar
//
// Desc: Manage stats for the scene
//
// Notes:
// - god help us
//
// Last Edit: 7/2/2023
//
//*************************************************

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Stats : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    //[HideInInspector]
    //public bool isDead;
    //public bool isPaused = false;

    [SerializeField]
    private bool isMainMenu = false;

    public PlayerInput playerInput;
    DeathShader stoneShader;

    // Transitions
    private Animator transitionCanvas;

    GameObject pauseUI;
    GameObject pauseMan;
    private GameObject settingsCanvas;

    // Eye Manager for pause fx
    GameObject eyeManager;

    // Music controller for pause fx
    GameObject musicController;

    static bool dontRepeat;

    // Music FX
    [SerializeField]
    private AudioClip[] killFX;
    public AudioSource audioKillSRC;

    // Camera FX
    CinemachineImpulseSource impulse;
    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        transitionCanvas = GameObject.FindWithTag("Transition")?.GetComponentInChildren<Animator>();
        impulse = GetComponent<CinemachineImpulseSource>();

        pauseUI = GameObject.FindWithTag("Pause");
        pauseMan = GameObject.FindWithTag("PauseManager");
        settingsCanvas = GameObject.FindWithTag("SettingsCanvas");
        eyeManager = GameObject.FindWithTag("EyeManager");
        musicController = GameObject.FindWithTag("MusicController");
        stoneShader = gameObject.GetComponent<DeathShader>();

        Info.isDead = false;
        Info.isPaused = false;

        dontRepeat = false;
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    void Start()
    {
        if (pauseUI)
        {
            // Hide Pause UI
            pauseUI.SetActive(false);
        }

        if (settingsCanvas)
        {
            // Hide Pause UI
            settingsCanvas.SetActive(false);
        }

        // CHANGE BACK TO T1
        if (SceneManager.GetActiveScene().name == "Tutorial-1")
        {
            // Reset eye deaths on fresh gameplay
            PlayerPrefs.SetInt("eyeDeathCounter", 0);
            PlayerPrefs.Save();
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // UPDATE ==============================================================
    void Update()
    {
        if (Info.isDead)
        {
            StartCoroutine(EyeDeathSequence());
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // RESET =============================================================
    public void ResetLevel(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (!isMainMenu && ctx.performed && !Info.isDead &&
            !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            Info.isDead = true;
            Info.eyeDeath = false;

            StartCoroutine(ObstacleDeathSequence());
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PAUSE =============================================================
    public void Pause(InputAction.CallbackContext ctx)
    {
        // Action performed and not in a cutscene
        if (ctx.performed && !isMainMenu && !Info.isDead && !GameObject.FindGameObjectWithTag("CutsceneCanvas").GetComponent<CutsceneManager>().GetIsCurrentlyPlaying())
        {
            // Paused -> Unpaused
            if (Info.isPaused && !pauseMan.GetComponent<PauseManager>().GetInSubMenu())
            {
                Info.isPaused = false;

                if (pauseUI)
                {
                    // Hide Pause UI
                    pauseUI.SetActive(false);
                }

                if (eyeManager)
                {
                    eyeManager.GetComponent<ActivateEyes>().PauseEyeAudio(false);
                }

                if (musicController)
                {
                    musicController.GetComponent<PersistantMusic>().ApplyPauseEffects(false);
                }

                // Reset timescale
                Time.timeScale = 1;
            }

            // Unpaused -> Paused
            else if (!Info.isPaused)
            {
                Info.isPaused = true;

                if (pauseUI)
                {
                    // Show Pause UI
                    pauseUI.SetActive(true);

                    // Set first button as being active
                    GameObject resumeButton = pauseUI.transform.Find("Resume").gameObject;

                    if (resumeButton)
                    {
                        EventSystem.current.SetSelectedGameObject(resumeButton);
                    }
                }

                if (eyeManager)
                {
                    eyeManager.GetComponent<ActivateEyes>().PauseEyeAudio(true);
                }

                if (musicController)
                {
                    musicController.GetComponent<PersistantMusic>().ApplyPauseEffects(true);
                }

                // Set paused timescale
                Time.timeScale = 0;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // PROGRESS ============================================================
    public void Progress(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject cutCanv = GameObject.FindGameObjectWithTag("CutsceneCanvas");

            if (cutCanv)
            {
                cutCanv.GetComponent<CutsceneManager>().StartSkipCutscene();
            }
        }

        if (ctx.canceled)
        {
            GameObject cutCanv = GameObject.FindGameObjectWithTag("CutsceneCanvas");

            if (cutCanv)
            {
                cutCanv.GetComponent<CutsceneManager>().InterruptSkipCutscene();
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TOGGLE PAUSE ========================================================
    public void TogglePause()
    {
        Info.isPaused = !Info.isPaused;
    }


    ////////////////////////////////////////////////////////////////////////
    // TRANSITION EYES =====================================================
    private IEnumerator EyeDeathSequence()
    {
        if (!dontRepeat)
        {
            dontRepeat = true;

            if (stoneShader && Info.eyeDeath)
            {
                PlayerPrefs.SetInt("eyeDeathCounter", PlayerPrefs.GetInt("eyeDeathCounter") + 1);
                PlayerPrefs.Save();

                if (Info.partner)
                {
                    // Ensure rope detatch
                    Info.player.GetComponent<Grapple>().Tethered(false);

                    if (Info.latch.isLatched)
                    {
                        Info.latch.ReleaseObject();
                    }

                    Info.partner.transform.SetParent(null);

                    Info.partner.GetComponentInChildren<ParticleSystem>().Play();
                    Info.partner.GetComponent<SpriteRenderer>().enabled = false;
                    Info.partner.GetComponentInChildren<Latch>().ReleaseObject();
                }

                StartCoroutine(stoneShader.Lerp(1));
                CameraShake.manager.Shake(impulse, 0.25f);
                audioKillSRC.PlayOneShot(killFX[0]);
            }

            Info.player.transform.SetParent(null);

            yield return new WaitForSeconds(1.5f);


            if (transitionCanvas)
            {
                transitionCanvas.SetTrigger("EyeDeath");

                yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
            }

            // Could change back to using levelname if needed
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    ////////////////////////////////////////////////////////////////////////
    // TRANSITION OBSTACLE =================================================
    public IEnumerator ObstacleDeathSequence()
    {
        Info.isDead = true;
        Info.eyeDeath = false;

        if (!dontRepeat)
        {
            dontRepeat = true;
            CameraShake.manager.Shake(impulse, 0.45f);
            audioKillSRC.PlayOneShot(killFX[1]);

            if (Info.partner)
            {
                // Ensure rope detatch
                Info.player.GetComponent<Grapple>().Tethered(false);

                if (Info.latch.isLatched)
                {
                    Info.latch.ReleaseObject();
                }

                // Partner stuff
                Info.partner.transform.SetParent(null);

                Info.partner.GetComponentInChildren<ParticleSystem>().Play();
                Info.partner.GetComponent<SpriteRenderer>().enabled = false;
                Info.partner.GetComponentInChildren<Latch>().ReleaseObject();
            }

            Info.player.transform.SetParent(null);

            Info.player.GetComponent<ParticleSystem>().Play();
            Info.player.GetComponent<SpriteRenderer>().enabled = false;
            Info.player.GetComponent<LineRenderer>().enabled = false;

            yield return new WaitForSeconds(1.5f);


            if (transitionCanvas)
            {
                transitionCanvas.SetTrigger("EyeDeath");

                yield return new WaitForSeconds(transitionCanvas.GetCurrentAnimatorClipInfo(0).Length);
            }

            // Could change back to using levelname if needed
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
