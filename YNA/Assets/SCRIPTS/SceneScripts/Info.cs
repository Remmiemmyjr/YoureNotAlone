//*************************************************
// Project: We're Tethered Together
// File: Info.cs
// Author/s: Emmy Berg
//
// Desc: Store a reference to all important objects
//
// Last Edit: 8/11/2023
//
//*************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Info : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////
    // VARIABLES ===========================================================
    static public GameObject player;
    static public GameObject partner;
    static public GameObject mainCam;
    static public PersistantMusic musicManager;

    static public Stats stats;
    static public PlayerController movement;
    static public Grapple grapple;
    static public Latch latch;

    public static bool isDead;
    public static bool isPaused;
    public static bool eyeDeath;

    // *********************************************************************


    ////////////////////////////////////////////////////////////////////////
    // AWAKE ===============================================================
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        partner = GameObject.FindGameObjectWithTag("Partner");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        movement = player.GetComponent<PlayerController>();
        grapple = player.GetComponent<Grapple>();
        latch = partner?.GetComponentInChildren<Latch>();

        stats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Stats>();
    }


    ////////////////////////////////////////////////////////////////////////
    // START ===============================================================
    private void Start()
    {
        isDead = false;
        isPaused = false;

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            player.GetComponent<SpriteRenderer>().enabled = true;

            if (partner)
            {
                partner.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (isPaused)
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }
}
