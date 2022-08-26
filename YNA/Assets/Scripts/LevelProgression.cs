using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoBehaviour
{
    GameObject Player;
    GameObject Partner;

    public GameObject instructions;
    public GameObject displayMessage;

    public string nextLevel;
    public bool requiresPartner;

    bool pressed = false;
    

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Partner = GameObject.FindGameObjectWithTag("Partner");

        instructions.SetActive(false);
        displayMessage.SetActive(false);

        if(requiresPartner == false)
        {
            displayMessage = null;
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            pressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            pressed = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        instructions.SetActive(true);

        if (pressed)
        {
            if (requiresPartner == true)
            {
                if (Player.GetComponent<Grapple>().isTethered == false)
                {
                    StartCoroutine(DisplayMessage());
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


    IEnumerator DisplayMessage()
    {
        Debug.Log("You cannot proceed without your partner");
        displayMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        displayMessage.SetActive(false);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        instructions.SetActive(false);
    }
}
