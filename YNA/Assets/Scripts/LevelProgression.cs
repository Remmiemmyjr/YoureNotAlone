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
        if(requiresPartner)
            Partner = GameObject.FindGameObjectWithTag("Partner");

        instructions.SetActive(false);
        displayMessage?.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (atExit)
            {
                if (requiresPartner == true)
                {
                    if (Player.GetComponent<Grapple>().isTethered == false)
                    {
                        StartCoroutine(DisplayMessage());
                    }
                    else
                    {
                        CheckpointController.ResetLevel();
                        SceneManager.LoadScene(nextLevel);
                    }
                }
                else
                {
                    CheckpointController.ResetLevel();
                    SceneManager.LoadScene(nextLevel);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            pressed = false;
        }


    }

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        //instructions.SetActive(true);


    }


    IEnumerator DisplayMessage()
    {
        Debug.Log("You cannot proceed without your partner");
        displayMessage?.SetActive(true);
        yield return new WaitForSeconds(2f);
        displayMessage?.SetActive(false);
    }

    bool atExit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        instructions.SetActive(true);
        atExit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        atExit = false;
        instructions.SetActive(false);

    }
}
