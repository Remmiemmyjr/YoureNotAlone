using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    public string nextLevel;
    public bool requiresPartner;

    bool pressed = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            pressed = true;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            pressed = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(Player.GetComponent<Grapple>().isTethered);

        if (pressed)
        {
            Debug.Log("boop");
            if (requiresPartner == true)
            {
                if (Player.GetComponent<Grapple>().isTethered == false)
                {
                    Debug.Log("You cannot proceed without your partner");
                }
                else
                {
                    SceneManager.LoadScene(nextLevel);
                }
            }
            else
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
